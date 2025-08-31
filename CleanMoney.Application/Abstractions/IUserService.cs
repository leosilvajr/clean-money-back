using CleanMoney.Application.DTOs;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Abstractions;

public interface IUserService
{
    Task<Result<string>> RegisterAsync(RegisterUserRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<Result<Profile>> ProfileAsync(string userId, CancellationToken ct = default);
}
