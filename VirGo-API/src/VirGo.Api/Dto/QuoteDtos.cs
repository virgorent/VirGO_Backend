namespace VirGo.Api.Dto;

public sealed record CreateQuoteRequest(
    QuoteTargetRequest? Target,
    string? EventId,
    string? EventType,
    string? EventDate,
    int Guests,
    string[]? RequestedServices,
    string? Message);

public sealed record QuoteTargetRequest(string? Type, string? Id);
