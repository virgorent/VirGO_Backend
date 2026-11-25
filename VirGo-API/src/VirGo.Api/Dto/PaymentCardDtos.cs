namespace VirGo.Api.Dto;

public sealed record PaymentCardRequest(string? Brand, string? Last4, int ExpMonth, int ExpYear, string? GatewayToken, bool IsDefault);
