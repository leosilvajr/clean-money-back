using CleanMoney.Application.Abstractions;    // IPasswordHasher
using CleanMoney.Domain.Entities;            // Usuario, Grupo, Competencia, LancamentoCompetencia
using CleanMoney.Infrastructure.Persistence;  // AppDbContext
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.API.Configure;

public static class SeedConfig
{
    // Helpers para garantir DateTime sempre em UTC
    private static DateTime MonthUtc(int year, int month)
        => new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);

    private static DateTime UtcDate(int daysOffset = 0)
    {
        // .Date mantém Kind=Utc no UtcNow, mas reforçamos por segurança
        var d = DateTime.UtcNow.Date.AddDays(daysOffset);
        return DateTime.SpecifyKind(d, DateTimeKind.Utc);
    }

    /// <summary>
    /// Cria o banco (se não existir), aplica migrations e garante:
    /// - Usuário admin:admin
    /// - 5 grupos genéricos
    /// - 5 competências (mês atual e 4 anteriores), sempre UTC
    /// - 5 lançamentos de exemplo na competência mais recente
    /// Chame esta extensão no Program.cs (comente se não quiser executar).
    /// </summary>
    public static async Task SeedAdminUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Garante que o banco e o schema existem (e aplica migrations)
        await db.Database.MigrateAsync();

        // =========================
        // Usuário admin
        // =========================
        var admin = await db.Usuarios.FirstOrDefaultAsync(u => u.Username == "admin");
        if (admin == null)
        {
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            admin = new Usuario
            {
                FullName = "Administrador do Sistema",
                Email = "admin@local",
                Username = "admin",
                PasswordHash = hasher.Hash("admin"),
                IsActive = true
            };

            db.Usuarios.Add(admin);
            await db.SaveChangesAsync();
        }

        // =========================
        // Grupos genéricos
        // =========================
        var gruposSeed = new[]
        {
            new Grupo { UsuarioId = admin.Id, Nome = "Alimentação", Cor = "#FF9800" },
            new Grupo { UsuarioId = admin.Id, Nome = "Moradia",     Cor = "#3F51B5" },
            new Grupo { UsuarioId = admin.Id, Nome = "Transporte",  Cor = "#009688" },
            new Grupo { UsuarioId = admin.Id, Nome = "Lazer",       Cor = "#E91E63" },
            new Grupo { UsuarioId = admin.Id, Nome = "Saúde",       Cor = "#4CAF50" }
        };

        foreach (var g in gruposSeed)
        {
            var existsGroup = await db.Grupos.AnyAsync(x => x.UsuarioId == admin.Id && x.Nome == g.Nome);
            if (!existsGroup)
                db.Grupos.Add(g);
        }

        await db.SaveChangesAsync();

        // Vamos carregar os grupos já existentes/novos em dicionário por nome (seguro contra First falhando)
        var gruposAdmin = await db.Grupos
            .Where(x => x.UsuarioId == admin.Id)
            .ToDictionaryAsync(x => x.Nome, x => x);

        // =========================
        // Competências (últimos 5 meses), sempre UTC
        // =========================
        var baseMonthUtc = MonthUtc(DateTime.UtcNow.Year, DateTime.UtcNow.Month);
        var competenciasSeed = Enumerable.Range(0, 5)
            .Select(i => new Competencia
            {
                UserId = admin.Id,
                DataCompetencia = baseMonthUtc.AddMonths(-i)
            })
            .ToList();

        foreach (var c in competenciasSeed)
        {
            var exists = await db.Competencias.AnyAsync(x => x.UserId == admin.Id && x.DataCompetencia == c.DataCompetencia);
            if (!exists)
                db.Competencias.Add(c);
        }

        await db.SaveChangesAsync();

        // Competência mais recente do admin
        var competenciaAtual = await db.Competencias
            .Where(c => c.UserId == admin.Id)
            .OrderByDescending(c => c.DataCompetencia)
            .FirstAsync();

        // =========================
        // Lançamentos genéricos (sempre UTC)
        // =========================
        // Só cria se tivermos os grupos esperados:
        bool Has(string nome) => gruposAdmin.ContainsKey(nome);

        var lancamentos = new List<LancamentoCompetencia>();

        if (Has("Alimentação"))
        {
            lancamentos.Add(new LancamentoCompetencia
            {
                UsuarioId = admin.Id,
                CompetenciaId = competenciaAtual.Id,
                GrupoId = gruposAdmin["Alimentação"].Id,
                Data = UtcDate(-2),
                Descricao = "Supermercado",
                Valor = 250.75m
            });
        }

        if (Has("Moradia"))
        {
            lancamentos.Add(new LancamentoCompetencia
            {
                UsuarioId = admin.Id,
                CompetenciaId = competenciaAtual.Id,
                GrupoId = gruposAdmin["Moradia"].Id,
                Data = UtcDate(-5),
                Descricao = "Aluguel",
                Valor = 1200m
            });
        }

        if (Has("Transporte"))
        {
            lancamentos.Add(new LancamentoCompetencia
            {
                UsuarioId = admin.Id,
                CompetenciaId = competenciaAtual.Id,
                GrupoId = gruposAdmin["Transporte"].Id,
                Data = UtcDate(-10),
                Descricao = "Combustível",
                Valor = 180.40m
            });
        }

        if (Has("Lazer"))
        {
            lancamentos.Add(new LancamentoCompetencia
            {
                UsuarioId = admin.Id,
                CompetenciaId = competenciaAtual.Id,
                GrupoId = gruposAdmin["Lazer"].Id,
                Data = UtcDate(-12),
                Descricao = "Cinema",
                Valor = 65m
            });
        }

        if (Has("Saúde"))
        {
            lancamentos.Add(new LancamentoCompetencia
            {
                UsuarioId = admin.Id,
                CompetenciaId = competenciaAtual.Id,
                GrupoId = gruposAdmin["Saúde"].Id,
                Data = UtcDate(-15),
                Descricao = "Farmácia",
                Valor = 89.90m
            });
        }

        // Evita duplicar lançamentos se já existirem (mesma Competência, Descrição e Valor)
        foreach (var l in lancamentos)
        {
            var exists = await db.LancamentosCompetencia.AnyAsync(x =>
                x.UsuarioId == l.UsuarioId &&
                x.CompetenciaId == l.CompetenciaId &&
                x.Descricao == l.Descricao &&
                x.Valor == l.Valor &&
                x.Data == l.Data);
            if (!exists)
                db.LancamentosCompetencia.Add(l);
        }

        await db.SaveChangesAsync();
    }
}
