using CleanMoney.Application.Abstractions;   // IPasswordHasher
using CleanMoney.Domain.Entities;           // User
using CleanMoney.Infrastructure.Persistence; // AppDbContext
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.API.Configure;

public static class SeedConfig
{
    /// <summary>
    /// Cria o banco (se não existir), aplica migrations e garante um usuário admin:admin.
    /// Chame esta extensão no Program.cs. Comente a chamada para desativar.
    /// </summary>
    public static async Task SeedAdminUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Garante que o banco e o schema existem (e aplica migrations)
        await db.Database.MigrateAsync();

        // Se já existir o usuário 'admin', não faz nada.
        var exists = await db.Usuarios.AnyAsync(u => u.Username == "admin");
        if (!exists)
        {
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            var admin = new Usuario
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
    }
}
