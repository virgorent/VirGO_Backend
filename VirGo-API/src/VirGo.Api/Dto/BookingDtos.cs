namespace VirGo.Api.Dto;

public sealed record CreateBookingRequest(
    string? QuoteId,
    string? EventId,
    string? VenueId,
    string? Date,
    int Hours,
    decimal? Deposit);

public sealed record CreatePaymentRequest(decimal? Amount);
