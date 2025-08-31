namespace CleanMoney.Application.DTOs;

public class AuthResponse
{
    public string Token { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string FullName { get; set; } = default!;
}
