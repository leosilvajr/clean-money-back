using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var user = modelBuilder.Entity<User>();
        user.ToTable("users"); // snake_case é comum no Postgres
        user.HasKey(x => x.Id);
        user.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid");

        user.Property(x => x.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(100);
        user.HasIndex(x => x.Username).IsUnique();

        user.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(200);

        user.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        user.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        user.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
    }
}
