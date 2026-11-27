namespace VirGo.Api.Dto;

public sealed record LoginRequest(string? Email, string? Password);

/// <summary>`Role` es opcional: `client` (por omisión), `owner` o `provider`. Nunca `admin`.</summary>
public sealed record RegisterRequest(string? Name, string? Email, string? Password, string? Phone, string? Role = null);

public sealed record PublicUserResponse(string Id, string Name, string Role, bool IsVerified);
