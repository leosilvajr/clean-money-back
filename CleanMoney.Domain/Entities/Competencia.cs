namespace CleanMoney.Domain.Entities;

/// <summary>
/// Representa a "competência" (mês de referência) do controle financeiro.
/// Armazenamos uma data representando o MÊS (ex.: normalizar para 1º dia do mês).
/// Recomendação: sempre gravar DataCompetencia no primeiro dia do mês (ex.: 2025-08-01).
/// </summary>
public class Competencia
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateTime DataCompetencia { get; set; }
    public Usuario Usuario { get; set; } = default!;

    // Navegação opcional inversa
    public ICollection<LancamentoCompetencia> Lancamentos { get; set; } = new List<LancamentoCompetencia>();
}
