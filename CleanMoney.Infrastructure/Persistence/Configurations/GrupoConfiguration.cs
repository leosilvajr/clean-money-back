using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMoney.Infrastructure.Persistence.Configurations;

public class GrupoConfiguration : IEntityTypeConfiguration<Grupo>
{
    public void Configure(EntityTypeBuilder<Grupo> b)
    {
        b.ToTable("Grupo");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnType("uuid");

        b.Property(x => x.UsuarioId).IsRequired();

        b.Property(x => x.Nome)
            .HasMaxLength(120)
            .IsRequired();

        b.Property(x => x.Cor)
            .HasMaxLength(30);

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.Grupos)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Evitar grupos duplicados por usuário
        b.HasIndex(x => new { x.UsuarioId, x.Nome }).IsUnique();
    }
}
