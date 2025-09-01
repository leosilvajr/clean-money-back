using CleanMoney.Application.DTOs;
using CleanMoney.Shared;
using CleanMoney.Shared.Responses;

public interface ILancamentoCompetenciaService
{
    Task<Result<QueryResult<LancamentoResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default);
    Task<Result<LancamentoResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Result<LancamentoResponse>> CreateAsync(Guid userId, LancamentoCreateRequest req, CancellationToken ct = default);
    Task<Result<LancamentoResponse>> UpdateAsync(Guid id, Guid userId, LancamentoUpdateRequest req, CancellationToken ct = default);
    Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}
