namespace VirGo.Api.Dto;

public sealed record CreateBookingRequest(
    string? QuoteId,
    string? EventId,
    string? VenueId,
    string? Date,
    int Hours,
    decimal? Deposit);

/// <summary>
/// Reservación tal como se expone: sin el id del cliente ni la clave de idempotencia,
/// que son datos internos del servidor.
/// </summary>
public sealed record BookingResponse(
    string Id,
    string Folio,
    string ClientName,
    string? QuoteId,
    string? EventId,
    string VenueId,
    string VenueName,
    string Date,
    int Hours,
    decimal Total,
    decimal Deposit,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static BookingResponse From(VirGo.Domain.Marketplace.Booking booking) => new(
        booking.Id,
        booking.Folio,
        booking.ClientName,
        booking.QuoteId,
        booking.EventId,
        booking.VenueId,
        booking.VenueName,
        booking.Date,
        booking.Hours,
        booking.Total,
        booking.Deposit,
        booking.Status,
        booking.CreatedAt,
        booking.UpdatedAt);
}
