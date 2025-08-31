using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanMoney.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Competencia> Competencias => Set<Competencia>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<LancamentoCompetencia> LancamentosCompetencia => Set<LancamentoCompetencia>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todas as IEntityTypeConfiguration<> deste assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
