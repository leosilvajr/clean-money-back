using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMoney.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        // Tabela com "U" maiúsculo. Npgsql vai quotar -> "Usuario"
        b.ToTable("Usuario");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnType("uuid");

        b.Property(x => x.FullName)
            .HasMaxLength(150)
            .IsRequired();

        b.Property(x => x.Username)
            .HasMaxLength(100)
            .IsRequired();

        b.Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired();

        b.HasIndex(x => x.Username).IsUnique();
        b.HasIndex(x => x.Email).IsUnique();

        b.Property(x => x.PasswordHash)
            .HasMaxLength(200)
            .IsRequired();

        b.Property(x => x.IsActive)
            .HasDefaultValue(true);

        b.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");

        b.Property(x => x.UpdatedAt);
    }
}
