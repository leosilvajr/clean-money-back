using CleanMoney.Application.DTOs;
using CleanMoney.Shared;
using CleanMoney.Shared.Responses;

namespace CleanMoney.Application.Abstractions;

public interface IGrupoService
{
    Task<Result<QueryResult<GrupoResponse>>> ListByUserAsync(Guid userId, QueryParams query, CancellationToken ct = default);
    Task<Result<GrupoResponse>> GetAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Result<GrupoResponse>> CreateAsync(Guid userId, GrupoCreateRequest req, CancellationToken ct = default);
    Task<Result<GrupoResponse>> UpdateAsync(Guid id, Guid userId, GrupoUpdateRequest req, CancellationToken ct = default);
    Task<Result<string>> DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}

