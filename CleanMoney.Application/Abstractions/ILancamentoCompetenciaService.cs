using CleanMoney.Application.DTOs;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Abstractions;

public interface ILancamentoCompetenciaService
{
    Task<Result<LancamentoResponse>> CreateAsync(Guid userId, LancamentoCreateRequest req, CancellationToken ct = default);
    Task<Result<LancamentoResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<LancamentoResponse>>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    Task<Result<LancamentoResponse>> UpdateAsync(Guid id, Guid userId, LancamentoUpdateRequest req, CancellationToken ct = default);
    Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}
