namespace CleanMoney.Application.DTOs;

public record GrupoCreateRequest(string Nome, string? Cor);
public record GrupoUpdateRequest(string Nome, string? Cor);
public record GrupoResponse(Guid Id, Guid UsuarioId, string Nome, string? Cor);
