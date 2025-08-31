using CleanMoney.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanMoney.Infrastructure.Persistence.Configurations;

public class LancamentoCompetenciaConfiguration : IEntityTypeConfiguration<LancamentoCompetencia>
{
    public void Configure(EntityTypeBuilder<LancamentoCompetencia> b)
    {
        b.ToTable("LancamentoCompetencia");

        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnType("uuid");

        b.Property(x => x.UsuarioId).IsRequired();
        b.Property(x => x.CompetenciaId).IsRequired();
        b.Property(x => x.GrupoId).IsRequired();

        b.Property(x => x.Data).IsRequired();

        b.Property(x => x.Descricao)
            .HasMaxLength(250)
            .IsRequired();

        // numeric(18,2) para valores financeiros
        b.Property(x => x.Valor)
            .HasColumnType("numeric(18,2)");

        b.HasOne(x => x.Usuario)
            .WithMany(u => u.Lancamentos)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Competencia)
            .WithMany(c => c.Lancamentos)
            .HasForeignKey(x => x.CompetenciaId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Grupo)
            .WithMany(g => g.Lancamentos)
            .HasForeignKey(x => x.GrupoId)
            .OnDelete(DeleteBehavior.Restrict); // evita excluir grupo com lançamentos
    }
}
