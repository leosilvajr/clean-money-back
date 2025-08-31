using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.Infrastructure.Repositories;

public class LancamentoCompetenciaRepository : ILancamentoCompetenciaRepository
{
    private readonly AppDbContext _db;
    public LancamentoCompetenciaRepository(AppDbContext db) => _db = db;

    public Task<LancamentoCompetencia?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.LancamentosCompetencia
              .Include(l => l.Competencia)
              .Include(l => l.Grupo)
              .FirstOrDefaultAsync(x => x.Id == id, ct);

    public IQueryable<LancamentoCompetencia> QueryByUser(Guid userId)
        => _db.LancamentosCompetencia.AsNoTracking().Where(x => x.UsuarioId == userId);

    public Task AddAsync(LancamentoCompetencia entity, CancellationToken ct = default)
        => _db.LancamentosCompetencia.AddAsync(entity, ct).AsTask();

    public void Update(LancamentoCompetencia entity) => _db.LancamentosCompetencia.Update(entity);
    public void Remove(LancamentoCompetencia entity) => _db.LancamentosCompetencia.Remove(entity);
    public Task SaveAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
