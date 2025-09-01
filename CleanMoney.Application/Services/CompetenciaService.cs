using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared;              // QueryParams, QueryResult, PaginationOutput
using CleanMoney.Shared.Responses;
using System.Globalization;
using System.Linq;

namespace CleanMoney.Application.Services;

public class CompetenciaService : ICompetenciaService
{
    private readonly ICompetenciaRepository _repo;

    public CompetenciaService(ICompetenciaRepository repo) => _repo = repo;

    private static DateTime NormalizeToMonth(DateTime d) => new DateTime(d.Year, d.Month, 1, 0, 0, 0, DateTimeKind.Utc);

    public async Task<Result<CompetenciaResponse>> CreateAsync(Guid userId, CompetenciaCreateRequest req, CancellationToken ct = default)
    {
        var month = NormalizeToMonth(req.DataCompetencia);
        var exists = await _repo.GetByUserAndMonthAsync(userId, month, ct);
        if (exists is not null) return Result<CompetenciaResponse>.Fail("Competência já existe para este mês.");

        var entity = new Competencia { UserId = userId, DataCompetencia = month };
        await _repo.AddAsync(entity, ct);
        await _repo.SaveAsync(ct);

        return Result<CompetenciaResponse>.Ok(new CompetenciaResponse(entity.Id, entity.UserId, entity.DataCompetencia));
    }

    public async Task<Result<CompetenciaResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UserId != userId) return Result<CompetenciaResponse>.Fail("Competência não encontrada.");
        return Result<CompetenciaResponse>.Ok(new CompetenciaResponse(e.Id, e.UserId, e.DataCompetencia));
    }

    // ✅ Novo: paginação + (opcional) busca + ordenação
    public Task<Result<QueryResult<CompetenciaResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default)
    {
        IQueryable<Competencia> q = _repo.QueryByUser(userId);

        // (Opcional) Search por ano "YYYY" ou mês/ano "MM/YYYY" (ou "YYYY-MM")
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.Trim();

            // ano (YYYY)
            if (s.Length == 4 && int.TryParse(s, out var ano))
            {
                q = q.Where(c => c.DataCompetencia.Year == ano);
            }
            else
            {
                // mes/ano "MM/YYYY"
                if (DateTime.TryParseExact(s, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var mmyyyy))
                {
                    q = q.Where(c => c.DataCompetencia.Year == mmyyyy.Year && c.DataCompetencia.Month == mmyyyy.Month);
                }
                // "YYYY-MM"
                else if (DateTime.TryParseExact(s, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var yyyymm))
                {
                    q = q.Where(c => c.DataCompetencia.Year == yyyymm.Year && c.DataCompetencia.Month == yyyymm.Month);
                }
                // formatos não reconhecidos → ignora
            }
        }

        // (Opcional) Ordering — apenas o primeiro item (MVP)
        if (query.Ordering?.Items is { Count: > 0 })
        {
            var item = query.Ordering.Items[0];
            var field = (item.Field ?? string.Empty).Trim().ToLower();
            var desc = item.Direction == CleanMoney.Shared.SortDirection.Desc;

            q = field switch
            {
                "datacompetencia" => desc ? q.OrderByDescending(c => c.DataCompetencia) : q.OrderBy(c => c.DataCompetencia),
                _ => desc ? q.OrderByDescending(c => c.DataCompetencia) : q.OrderBy(c => c.DataCompetencia),
            };
        }
        else
        {
            // padrão: DataCompetencia desc
            q = q.OrderByDescending(c => c.DataCompetencia);
        }

        // Total antes da paginação
        var total = q.Count(); // síncrono (não exige EF no Application)

        // Paginação
        var pageNumber = query.Pagination?.PageNumber > 0 ? query.Pagination!.PageNumber : 1;
        var pageSize = query.Pagination?.PageSize is int ps && ps >= 0 ? ps : 10;

        if (pageSize > 0)
            q = q.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var items = q
            .Select(c => new CompetenciaResponse(c.Id, c.UserId, c.DataCompetencia))
            .ToList(); // síncrono

        var result = new QueryResult<CompetenciaResponse>(query)
        {
            Items = items
        };

        result.Pagination ??= new PaginationOutput { PageNumber = pageNumber, PageSize = pageSize };
        result.Pagination.TotalItems = total;
        result.Pagination.Calculate();

        return Task.FromResult(Result<QueryResult<CompetenciaResponse>>.Ok(result));
    }

    public async Task<Result<CompetenciaResponse>> UpdateAsync(Guid id, Guid userId, CompetenciaUpdateRequest req, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UserId != userId) return Result<CompetenciaResponse>.Fail("Competência não encontrada.");
        var month = NormalizeToMonth(req.DataCompetencia);

        var dup = await _repo.GetByUserAndMonthAsync(userId, month, ct);
        if (dup is not null && dup.Id != id) return Result<CompetenciaResponse>.Fail("Já existe competência para este mês.");

        e.DataCompetencia = month;
        _repo.Update(e);
        await _repo.SaveAsync(ct);

        return Result<CompetenciaResponse>.Ok(new CompetenciaResponse(e.Id, e.UserId, e.DataCompetencia));
    }

    public async Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e is null || e.UserId != userId) return Result<string>.Fail("Competência não encontrada.");
        _repo.Remove(e);
        await _repo.SaveAsync(ct);
        return Result<string>.Ok("Competência removida.");
    }
}
