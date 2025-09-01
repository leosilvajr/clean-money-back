using CleanMoney.API.Configure;
using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Shared; // QueryParams
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanMoney.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompetenciasController : ControllerBase
{
    private readonly ICompetenciaService _service;
    public CompetenciasController(ICompetenciaService service) => _service = service;

    // GET /api/competencias?pagination.pageNumber=1&pagination.pageSize=12&ordering.items[0].field=DataCompetencia&ordering.items[0].direction=Desc
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] QueryParams query, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.ListByUserAsync(userId, query, ct);
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
    public async Task<IActionResult> Create([FromBody] CompetenciaCreateRequest req, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var r = await _service.CreateAsync(userId, req, ct);
        return r.Success ? Ok(r.Data) : BadRequest(new { error = r.Error });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CompetenciaUpdateRequest req, CancellationToken ct)
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
