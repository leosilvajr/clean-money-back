using CleanMoney.API.Configure.Examples;
using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace CleanMoney.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService) => _userService = userService;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken ct)
    {
        var result = await _userService.RegisterAsync(request, ct);
        if (!result.Success) return BadRequest(new { error = result.Error });
        return Ok(new { message = result.Data });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await _userService.LoginAsync(request, ct);
        if (!result.Success) return Unauthorized(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirst("nameidentifier")?.Value
                     ?? User.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Usuário não identificado.");

        var user = await _userService.ProfileAsync(userId);
        return Ok(user);
    }

}
