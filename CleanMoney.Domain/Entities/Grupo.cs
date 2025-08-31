namespace CleanMoney.Domain.Entities;

/// <summary>
/// Agrupa lançamentos (ex.: "Moradia", "Transporte", "Lazer").
/// Cor opcional para UI (tags/legendas).
/// Por padrão ligamos Grupo a um Usuário, para isolar dados por conta.
/// </summary>
public class Grupo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UsuarioId { get; set; }
    public string Nome { get; set; } = default!;
    public string? Cor { get; set; }
    public Usuario Usuario { get; set; } = default!;

    public ICollection<LancamentoCompetencia> Lancamentos { get; set; } = new List<LancamentoCompetencia>();
}
