namespace CleanMoney.Application.DTOs;

public record CompetenciaCreateRequest(DateTime DataCompetencia);
public record CompetenciaUpdateRequest(DateTime DataCompetencia);
public record CompetenciaResponse(Guid Id, Guid UserId, DateTime DataCompetencia);
