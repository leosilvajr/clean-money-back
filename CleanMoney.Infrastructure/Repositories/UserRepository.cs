using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanMoney.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public Task<Usuario?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, ct);

    public async Task AddAsync(Usuario user, CancellationToken ct = default) =>
        await _db.Usuarios.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);


}
