namespace CleanMoney.Domain.Entities;

/// <summary>
/// Lançamentos financeiros ligados a uma Competência (mês).
/// Mínimo viável: Data, Descricao, Valor e vínculos (Usuário, Competência, Grupo).
/// </summary>
public class LancamentoCompetencia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UsuarioId { get; set; }
    public Guid CompetenciaId { get; set; }
    public Guid GrupoId { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = default!;
    public decimal Valor { get; set; }

    // Navegações
    public Competencia Competencia { get; set; } = default!;
    public Usuario Usuario { get; set; } = default!;
    public Grupo Grupo { get; set; } = default!;
}
