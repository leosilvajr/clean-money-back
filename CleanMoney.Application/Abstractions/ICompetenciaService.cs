using CleanMoney.Application.DTOs;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Abstractions;

public interface ICompetenciaService
{
    Task<Result<CompetenciaResponse>> CreateAsync(Guid userId, CompetenciaCreateRequest req, CancellationToken ct = default);
    Task<Result<CompetenciaResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<CompetenciaResponse>>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    Task<Result<CompetenciaResponse>> UpdateAsync(Guid id, Guid userId, CompetenciaUpdateRequest req, CancellationToken ct = default);
    Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}
