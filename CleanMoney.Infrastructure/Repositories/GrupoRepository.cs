using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.Infrastructure.Repositories;

public class GrupoRepository : IGrupoRepository
{
    private readonly AppDbContext _db;
    public GrupoRepository(AppDbContext db) => _db = db;

    public Task<Grupo?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Grupos.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Grupo?> GetByUserAndNameAsync(Guid userId, string nome, CancellationToken ct = default)
        => _db.Grupos.FirstOrDefaultAsync(x => x.UsuarioId == userId && x.Nome == nome, ct);

    public IQueryable<Grupo> QueryByUser(Guid userId)
        => _db.Grupos.AsNoTracking().Where(x => x.UsuarioId == userId);

    public Task AddAsync(Grupo entity, CancellationToken ct = default)
        => _db.Grupos.AddAsync(entity, ct).AsTask();

    public void Update(Grupo entity) => _db.Grupos.Update(entity);
    public void Remove(Grupo entity) => _db.Grupos.Remove(entity);
    public Task SaveAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
