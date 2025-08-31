using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Services;

public class GrupoService : IGrupoService
{
    private readonly IGrupoRepository _repo;

    public GrupoService(IGrupoRepository repo) => _repo = repo;

    public async Task<Result<GrupoResponse>> CreateAsync(Guid userId, GrupoCreateRequest req, CancellationToken ct = default)
    {
        var dup = await _repo.GetByUserAndNameAsync(userId, req.Nome, ct);
        if (dup is not null) return Result<GrupoResponse>.Fail("Já existe um grupo com esse nome.");

        var e = new Grupo { UsuarioId = userId, Nome = req.Nome.Trim(), Cor = req.Cor };
        await _repo.AddAsync(e, ct);
        await _repo.SaveAsync(ct);

        return Result<GrupoResponse>.Ok(new GrupoResponse(e.Id, e.UsuarioId, e.Nome, e.Cor));
    }

    public async Task<Result<GrupoResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<GrupoResponse>.Fail("Grupo não encontrado.");
        return Result<GrupoResponse>.Ok(new GrupoResponse(e.Id, e.UsuarioId, e.Nome, e.Cor));
    }

    public Task<Result<IReadOnlyList<GrupoResponse>>> ListByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var list = _repo.QueryByUser(userId)
                        .OrderBy(g => g.Nome)
                        .Select(g => new GrupoResponse(g.Id, g.UsuarioId, g.Nome, g.Cor))
                        .ToList()
                        .AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<GrupoResponse>>.Ok(list));
    }

    public async Task<Result<GrupoResponse>> UpdateAsync(Guid id, Guid userId, GrupoUpdateRequest req, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<GrupoResponse>.Fail("Grupo não encontrado.");

        var dup = await _repo.GetByUserAndNameAsync(userId, req.Nome, ct);
        if (dup is not null && dup.Id != id) return Result<GrupoResponse>.Fail("Já existe um grupo com esse nome.");

        e.Nome = req.Nome.Trim();
        e.Cor = req.Cor;
        _repo.Update(e);
        await _repo.SaveAsync(ct);

        return Result<GrupoResponse>.Ok(new GrupoResponse(e.Id, e.UsuarioId, e.Nome, e.Cor));
    }

    public async Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<string>.Fail("Grupo não encontrado.");
        _repo.Remove(e);
        await _repo.SaveAsync(ct);
        return Result<string>.Ok("Grupo removido.");
    }
}
