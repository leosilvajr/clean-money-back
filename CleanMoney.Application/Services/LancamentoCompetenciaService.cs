using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared;              // QueryParams, QueryResult, PaginationOutput
using CleanMoney.Shared.Responses;
using System.Linq;

namespace CleanMoney.Application.Services;

public class LancamentoCompetenciaService : ILancamentoCompetenciaService
{
    private readonly ILancamentoCompetenciaRepository _repo;
    private readonly ICompetenciaRepository _competencias;
    private readonly IGrupoRepository _grupos;

    public LancamentoCompetenciaService(
        ILancamentoCompetenciaRepository repo,
        ICompetenciaRepository competencias,
        IGrupoRepository grupos)
    {
        _repo = repo;
        _competencias = competencias;
        _grupos = grupos;
    }

    public async Task<Result<LancamentoResponse>> CreateAsync(Guid userId, LancamentoCreateRequest req, CancellationToken ct = default)
    {
        var comp = await _competencias.GetByIdAsync(req.CompetenciaId, ct);
        if (comp is null || comp.UserId != userId)
            return Result<LancamentoResponse>.Fail("Competência inválida para o usuário.");

        var grupo = await _grupos.GetByIdAsync(req.GrupoId, ct);
        if (grupo is null || grupo.UsuarioId != userId)
            return Result<LancamentoResponse>.Fail("Grupo inválido para o usuário.");

        var e = new LancamentoCompetencia
        {
            UsuarioId = userId,
            CompetenciaId = req.CompetenciaId,
            GrupoId = req.GrupoId,
            Data = req.Data,
            Descricao = req.Descricao.Trim(),
            Valor = req.Valor
        };

        await _repo.AddAsync(e, ct);
        await _repo.SaveAsync(ct);

        return Result<LancamentoResponse>.Ok(new LancamentoResponse(
            e.Id, e.UsuarioId, e.CompetenciaId, e.GrupoId, e.Data, e.Descricao, e.Valor));
    }

    public async Task<Result<LancamentoResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<LancamentoResponse>.Fail("Lançamento não encontrado.");
        return Result<LancamentoResponse>.Ok(new LancamentoResponse(
            e.Id, e.UsuarioId, e.CompetenciaId, e.GrupoId, e.Data, e.Descricao, e.Valor));
    }

    // ✅ Novo: paginação + busca + ordenação
    public Task<Result<QueryResult<LancamentoResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default)
    {
        IQueryable<LancamentoCompetencia> q = _repo.QueryByUser(userId);

        // (Opcional) Search em Descricao
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim().ToLower();
            q = q.Where(l => (l.Descricao ?? string.Empty).ToLower().Contains(s));
        }

        // (Opcional) Ordering — só o primeiro item (MVP)
        if (query.Ordering?.Items is { Count: > 0 })
        {
            var item = query.Ordering.Items[0];
            var field = (item.Field ?? string.Empty).Trim().ToLower();
            var desc = item.Direction == CleanMoney.Shared.SortDirection.Desc;

            q = field switch
            {
                "valor" => desc ? q.OrderByDescending(l => l.Valor) : q.OrderBy(l => l.Valor),
                "descricao" => desc ? q.OrderByDescending(l => l.Descricao) : q.OrderBy(l => l.Descricao),
                "data" => desc ? q.OrderByDescending(l => l.Data) : q.OrderBy(l => l.Data),
                _ => desc ? q.OrderByDescending(l => l.Data) : q.OrderBy(l => l.Data),
            };
        }
        else
        {
            // padrão: Data desc
            q = q.OrderByDescending(l => l.Data);
        }

        // Total antes da paginação
        var total = q.Count(); // síncrono para não depender de EF Core na camada Application

        // Paginação
        var pageNumber = query.Pagination?.PageNumber > 0 ? query.Pagination!.PageNumber : 1;
        var pageSize = query.Pagination?.PageSize is int ps && ps >= 0 ? ps : 10;

        if (pageSize > 0)
            q = q.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var items = q
            .Select(l => new LancamentoResponse(l.Id, l.UsuarioId, l.CompetenciaId, l.GrupoId, l.Data, l.Descricao, l.Valor))
            .ToList(); // síncrono

        var result = new QueryResult<LancamentoResponse>(query)
        {
            Items = items
        };

        result.Pagination ??= new PaginationOutput { PageNumber = pageNumber, PageSize = pageSize };
        result.Pagination.TotalItems = total;
        result.Pagination.Calculate();

        return Task.FromResult(Result<QueryResult<LancamentoResponse>>.Ok(result));
    }

    public async Task<Result<LancamentoResponse>> UpdateAsync(Guid id, Guid userId, LancamentoUpdateRequest req, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<LancamentoResponse>.Fail("Lançamento não encontrado.");

        var comp = await _competencias.GetByIdAsync(req.CompetenciaId, ct);
        if (comp is null || comp.UserId != userId)
            return Result<LancamentoResponse>.Fail("Competência inválida para o usuário.");

        var grupo = await _grupos.GetByIdAsync(req.GrupoId, ct);
        if (grupo is null || grupo.UsuarioId != userId)
            return Result<LancamentoResponse>.Fail("Grupo inválido para o usuário.");

        e.CompetenciaId = req.CompetenciaId;
        e.GrupoId = req.GrupoId;
        e.Data = req.Data;
        e.Descricao = req.Descricao.Trim();
        e.Valor = req.Valor;

        _repo.Update(e);
        await _repo.SaveAsync(ct);

        return Result<LancamentoResponse>.Ok(new LancamentoResponse(
            e.Id, e.UsuarioId, e.CompetenciaId, e.GrupoId, e.Data, e.Descricao, e.Valor));
    }

    public async Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UsuarioId != userId) return Result<string>.Fail("Lançamento não encontrado.");
        _repo.Remove(e);
        await _repo.SaveAsync(ct);
        return Result<string>.Ok("Lançamento removido.");
    }
}
