using CleanMoney.Domain.Entities;

namespace CleanMoney.Domain.Repositories;

public interface IGrupoRepository
{
    Task<Grupo?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Grupo?> GetByUserAndNameAsync(Guid userId, string nome, CancellationToken ct = default);
    IQueryable<Grupo> QueryByUser(Guid userId);
    Task AddAsync(Grupo entity, CancellationToken ct = default);
    void Update(Grupo entity);
    void Remove(Grupo entity);
    Task SaveAsync(CancellationToken ct = default);
}
