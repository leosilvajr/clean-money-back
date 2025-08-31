using CleanMoney.API.Configure;
using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMoney.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GruposController : ControllerBase
{
    private readonly IGrupoService _service;
    public GruposController(IGrupoService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.ListByUserAsync(userId, ct);
        return r.Success ? Ok(r.Data) : BadRequest(new { error = r.Error });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.GetAsync(id, userId, ct);
        return r.Success ? Ok(r.Data) : NotFound(new { error = r.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create(GrupoCreateRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.CreateAsync(userId, req, ct);
        return r.Success ? Ok(r.Data) : BadRequest(new { error = r.Error });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, GrupoUpdateRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.UpdateAsync(id, userId, req, ct);
        return r.Success ? Ok(r.Data) : BadRequest(new { error = r.Error });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.DeleteAsync(id, userId, ct);
        return r.Success ? Ok(new { message = r.Data }) : NotFound(new { error = r.Error });
    }
}
