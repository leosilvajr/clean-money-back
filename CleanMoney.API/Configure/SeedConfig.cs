using CleanMoney.Application.Abstractions;   // IPasswordHasher
using CleanMoney.Domain.Entities;           // Usuario, Grupo
using CleanMoney.Infrastructure.Persistence; // AppDbContext
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.API.Configure;

public static class SeedConfig
{
    /// <summary>
    /// Cria o banco (se não existir), aplica migrations e garante um usuário admin:admin.
    /// Também cria grupos genéricos de despesas para o admin.
    /// Chame esta extensão no Program.cs. Comente a chamada para desativar.
    /// </summary>
    public static async Task SeedAdminUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Garante que o banco e o schema existem (e aplica migrations)
        await db.Database.MigrateAsync();

        // Se já existir o usuário 'admin', não faz nada.
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

        // Cria alguns grupos genéricos de despesas se ainda não existirem
        var grupos = new[]
        {
            new Grupo { UsuarioId = admin.Id, Nome = "Alimentação", Cor = "#FF9800" },
            new Grupo { UsuarioId = admin.Id, Nome = "Moradia", Cor = "#3F51B5" },
            new Grupo { UsuarioId = admin.Id, Nome = "Transporte", Cor = "#009688" },
            new Grupo { UsuarioId = admin.Id, Nome = "Lazer", Cor = "#E91E63" },
            new Grupo { UsuarioId = admin.Id, Nome = "Saúde", Cor = "#4CAF50" }
        };

        foreach (var g in grupos)
        {
            var existsGroup = await db.Grupos.AnyAsync(x => x.UsuarioId == admin.Id && x.Nome == g.Nome);
            if (!existsGroup)
                db.Grupos.Add(g);
        }

        await db.SaveChangesAsync();
    }
}
