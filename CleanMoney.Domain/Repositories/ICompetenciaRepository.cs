using CleanMoney.Domain.Entities;

namespace CleanMoney.Domain.Repositories;

public interface ICompetenciaRepository
{
    Task<Competencia?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Competencia?> GetByUserAndMonthAsync(Guid userId, DateTime month, CancellationToken ct = default);
    IQueryable<Competencia> QueryByUser(Guid userId);
    Task AddAsync(Competencia entity, CancellationToken ct = default);
    void Update(Competencia entity);
    void Remove(Competencia entity);
    Task SaveAsync(CancellationToken ct = default);
}
