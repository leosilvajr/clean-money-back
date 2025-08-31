using CleanMoney.Domain.Entities;

namespace CleanMoney.Domain.Repositories;

public interface IUserRepository
{
    Task<Usuario?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task AddAsync(Usuario user, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
