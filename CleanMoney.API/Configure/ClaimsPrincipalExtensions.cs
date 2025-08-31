using System.Security.Claims;

namespace CleanMoney.API.Configure;

public static class ClaimsPrincipalExtensions
{
    /// <summary>Obtém o userId (Guid) do JWT. No token usamos ClaimTypes.NameIdentifier.</summary>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var guid))
            throw new UnauthorizedAccessException("UserId ausente ou inválido no token.");
        return guid;
    }
}
