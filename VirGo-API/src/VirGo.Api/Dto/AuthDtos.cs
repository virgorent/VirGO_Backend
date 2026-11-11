namespace VirGo.Api.Dto;

public sealed record LoginRequest(string? Email, string? Password);

public sealed record RegisterRequest(string? Name, string? Email, string? Password, string? Phone);
