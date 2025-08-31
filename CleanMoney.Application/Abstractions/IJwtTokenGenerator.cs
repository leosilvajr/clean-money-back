using CleanMoney.Domain.Entities;

namespace CleanMoney.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
