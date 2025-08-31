using CleanMoney.Application.Abstractions;
using CleanMoney.Application.DTOs;
using CleanMoney.Domain.Entities;
using CleanMoney.Domain.Repositories;
using CleanMoney.Shared.Responses;

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

    public Task<Result<IReadOnlyList<LancamentoResponse>>> ListByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var list = _repo.QueryByUser(userId)
                        .OrderByDescending(l => l.Data)
                        .Select(l => new LancamentoResponse(l.Id, l.UsuarioId, l.CompetenciaId, l.GrupoId, l.Data, l.Descricao, l.Valor))
                        .ToList()
                        .AsReadOnly();
        return Task.FromResult(Result<IReadOnlyList<LancamentoResponse>>.Ok(list));
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
