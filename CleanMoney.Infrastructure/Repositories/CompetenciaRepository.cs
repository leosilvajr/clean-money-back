using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.Infrastructure.Repositories;

public class CompetenciaRepository : ICompetenciaRepository
{
    private readonly AppDbContext _db;
    public CompetenciaRepository(AppDbContext db) => _db = db;

    public Task<Competencia?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Competencias.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Competencia?> GetByUserAndMonthAsync(Guid userId, DateTime month, CancellationToken ct = default)
        => _db.Competencias.FirstOrDefaultAsync(x => x.UserId == userId && x.DataCompetencia == month, ct);

    public IQueryable<Competencia> QueryByUser(Guid userId)
        => _db.Competencias.AsNoTracking().Where(x => x.UserId == userId);

    public Task AddAsync(Competencia entity, CancellationToken ct = default)
        => _db.Competencias.AddAsync(entity, ct).AsTask();

    public void Update(Competencia entity) => _db.Competencias.Update(entity);
    public void Remove(Competencia entity) => _db.Competencias.Remove(entity);
    public Task SaveAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
