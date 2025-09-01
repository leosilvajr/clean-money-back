using CleanMoney.Application.DTOs;
using CleanMoney.Shared;
using CleanMoney.Shared.Responses;

public interface ICompetenciaService
{
    Task<Result<QueryResult<CompetenciaResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default);
    Task<Result<CompetenciaResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Result<CompetenciaResponse>> CreateAsync(Guid userId, CompetenciaCreateRequest req, CancellationToken ct = default);
    Task<Result<CompetenciaResponse>> UpdateAsync(Guid id, Guid userId, CompetenciaUpdateRequest req, CancellationToken ct = default);
    Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}
