using CleanMoney.Domain.Entities;

namespace CleanMoney.Domain.Repositories;

public interface ILancamentoCompetenciaRepository
{
    Task<LancamentoCompetencia?> GetByIdAsync(Guid id, CancellationToken ct = default);
    IQueryable<LancamentoCompetencia> QueryByUser(Guid userId);
    Task AddAsync(LancamentoCompetencia entity, CancellationToken ct = default);
    void Update(LancamentoCompetencia entity);
    void Remove(LancamentoCompetencia entity);
    Task SaveAsync(CancellationToken ct = default);
}
