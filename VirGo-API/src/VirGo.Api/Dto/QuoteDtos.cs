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

public sealed record QuoteResponseRequest(decimal? Total, decimal? Deposit, string? Message);

/// <summary>
/// Lo que la API devuelve de una cotización: los mismos campos del dominio salvo <c>clientId</c>,
/// que es un dato interno (el propietario veía el id del cliente). La app usa <c>clientName</c>.
/// </summary>
public sealed record QuoteView(
    string Id,
    string ClientName,
    string TargetType,
    string TargetId,
    string TargetName,
    string? EventId,
    string EventType,
    string EventDate,
    int Guests,
    string[] RequestedServices,
    string? Message,
    VirGo.Domain.Marketplace.QuoteResponse? Response,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static QuoteView From(VirGo.Domain.Marketplace.Quote quote) => new(
        quote.Id,
        quote.ClientName,
        quote.TargetType,
        quote.TargetId,
        quote.TargetName,
        quote.EventId,
        quote.EventType,
        quote.EventDate,
        quote.Guests,
        quote.RequestedServices,
        quote.Message,
        quote.Response,
        quote.Status,
        quote.CreatedAt,
        quote.UpdatedAt);
}
