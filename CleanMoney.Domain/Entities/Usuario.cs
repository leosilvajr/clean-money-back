namespace CleanMoney.Domain.Entities;

/// <summary>
/// Representa um usuário do sistema (credenciais e dados básicos).
/// Optamos por manter Username e Email únicos para autenticação e comunicação.
/// </summary>
public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navegações
    public ICollection<Competencia> Competencias { get; set; } = new List<Competencia>();
    public ICollection<LancamentoCompetencia> Lancamentos { get; set; } = new List<LancamentoCompetencia>();
    public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
