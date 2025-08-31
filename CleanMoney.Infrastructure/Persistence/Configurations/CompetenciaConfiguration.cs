using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMoney.Infrastructure.Persistence.Configurations;

public class CompetenciaConfiguration : IEntityTypeConfiguration<Competencia>
{
    public void Configure(EntityTypeBuilder<Competencia> b)
    {
        b.ToTable("Competencia");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnType("uuid");

        b.Property(x => x.UserId).IsRequired();

        // Importante: manter DataCompetencia normalizada (1º dia do mês)
        b.Property(x => x.DataCompetencia).IsRequired();

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.Competencias)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Garante 1 competência por usuário por mês (assumindo normalização na gravação)
        b.HasIndex(x => new { x.UserId, x.DataCompetencia }).IsUnique();
    }
}
