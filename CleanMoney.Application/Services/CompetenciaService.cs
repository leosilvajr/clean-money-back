using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared.Responses;

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

    public Task<Result<IReadOnlyList<CompetenciaResponse>>> ListByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var list = _repo.QueryByUser(userId)
                        .OrderByDescending(c => c.DataCompetencia)
                        .Select(c => new CompetenciaResponse(c.Id, c.UserId, c.DataCompetencia))
                        .ToList()
                        .AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<CompetenciaResponse>>.Ok(list));
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
