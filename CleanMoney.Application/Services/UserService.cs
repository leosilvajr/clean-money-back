using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public UserService(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<Result<string>> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default)
    {
        var existing = await _users.GetByUsernameAsync(request.Username, ct);
        if (existing is not null)
            return Result<string>.Fail("Usuário já existe.");

        var user = new Usuario
        {
            Username = request.Username,
            PasswordHash = _hasher.Hash(request.Password),
            FullName = request.FullName,
            Email = request.Email,
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return Result<string>.Ok("Usuário criado com sucesso.");
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(request.Username, ct);
        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash) || !user.IsActive)
            return Result<AuthResponse>.Fail("Credenciais inválidas.");

        var token = _jwt.GenerateToken(user);
        return Result<AuthResponse>.Ok(new AuthResponse { 
            FullName = user.FullName, 
            Username = user.Username, 
            Token = token 
        });
    }

    public async Task<Result<Profile>> ProfileAsync(string userId, CancellationToken ct = default)
    {
        var user = await _users.GetByUsernameAsync(userId, ct);
        if (user is null) return Result<Profile>.Fail("Usuário não encontrado.");
        return Result<Profile>.Ok(new Profile { 
            FullName = user.FullName, 
            Username = user.Username, 
            Email = user.Email 
        });
    }
}
