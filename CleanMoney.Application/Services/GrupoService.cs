using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared;              // QueryParams, QueryResult, PaginationOutput
using CleanMoney.Shared.Responses;
using Microsoft.EntityFrameworkCore;


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

    // ✅ Novo: retorna QueryResult<GrupoResponse> com paginação
    public async Task<Result<QueryResult<GrupoResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default)
    {
        // Base query
        IQueryable<Grupo> q = _repo.QueryByUser(userId);

        // (Opcional) Search por Nome
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            q = q.Where(g => g.Nome.ToLower().Contains(s));
        }

        // (Opcional) Ordering - usa somente o primeiro item (MVP)
        if (query.Ordering?.Items is { Count: > 0 })
        {
            var item = query.Ordering.Items[0];
            var field = (item.Field ?? string.Empty).Trim();
            var desc = item.Direction == CleanMoney.Shared.SortDirection.Desc;

            q = (field.ToLower()) switch
            {
                "cor" => desc ? q.OrderByDescending(g => g.Cor) : q.OrderBy(g => g.Cor),
                // default: Nome
                _ => desc ? q.OrderByDescending(g => g.Nome) : q.OrderBy(g => g.Nome),
            };
        }
        else
        {
            // Ordem padrão estável
            q = q.OrderBy(g => g.Nome);
        }

        // Total antes da paginação
        var total = await q.CountAsync(ct);

        // Paginação
        var pageNumber = query.Pagination?.PageNumber > 0 ? query.Pagination!.PageNumber : 1;
        var pageSize = query.Pagination?.PageSize is int ps && ps >= 0 ? ps : 10;

        if (pageSize > 0)
            q = q.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var items = await q
            .Select(g => new GrupoResponse(g.Id, g.UsuarioId, g.Nome, g.Cor))
            .ToListAsync(ct);

        // Monta o QueryResult com os metadados do QueryParams
        var result = new QueryResult<GrupoResponse>(query)
        {
            Items = items
        };

        // Preenche a paginação e calcula derivados
        result.Pagination ??= new PaginationOutput { PageNumber = pageNumber, PageSize = pageSize };
        result.Pagination.TotalItems = total;
        result.Pagination.Calculate();

        return Result<QueryResult<GrupoResponse>>.Ok(result);
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
