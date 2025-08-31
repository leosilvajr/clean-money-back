using CleanMoney.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace CleanMoney.API.Configure.Examples;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
        => new LoginRequest
        {
            Username = "admin",
            Password = "admin"
        };
}
