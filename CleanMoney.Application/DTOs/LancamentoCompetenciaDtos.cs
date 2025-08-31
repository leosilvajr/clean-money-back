namespace CleanMoney.Application.DTOs;

public record LancamentoCreateRequest(
    Guid CompetenciaId, Guid GrupoId, DateTime Data, string Descricao, decimal Valor);

public record LancamentoUpdateRequest(
    Guid CompetenciaId, Guid GrupoId, DateTime Data, string Descricao, decimal Valor);

public record LancamentoResponse(
    Guid Id, Guid UsuarioId, Guid CompetenciaId, Guid GrupoId, DateTime Data, string Descricao, decimal Valor);
