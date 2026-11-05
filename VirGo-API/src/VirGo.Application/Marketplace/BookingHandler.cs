using System.Globalization;
using VirGo.Application.Abstractions;
using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed class BookingHandler(IVenueRepository venues, IBookingRepository bookings, IClock clock)
{
    public async Task<Booking?> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await bookings.FindByIdempotencyKeyAsync(command.ClientId, command.IdempotencyKey, cancellationToken);
        if (existing is not null)
        {
            return existing;
        }

        if (!Guid.TryParse(command.VenueId, out _)
            || command.IdempotencyKey.Length > 128
            || command.Hours < 1
            || !DateOnly.TryParseExact(command.Date, "yyyy-MM-dd", out var date))
        {
            return null;
        }

        var venue = await venues.FindPublishedAsync(command.VenueId, cancellationToken);
        if (venue is null || date < DateOnly.FromDateTime(clock.UtcNow))
        {
            return null;
        }

        var now = clock.UtcNow;
        var total = venue.BasePrice * command.Hours;
        var folio = await bookings.NextFolioAsync(now.Year, cancellationToken);
        var booking = new Booking
        {
            Id = Guid.NewGuid().ToString("D"),
            ClientId = command.ClientId,
            IdempotencyKey = command.IdempotencyKey,
            ClientName = command.ClientName,
            QuoteId = string.IsNullOrWhiteSpace(command.QuoteId) ? null : command.QuoteId,
            EventId = string.IsNullOrWhiteSpace(command.EventId) ? null : command.EventId,
            VenueId = venue.Id,
            VenueName = venue.Name,
            Date = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            Hours = command.Hours,
            Total = total,
            Deposit = command.Deposit is > 0 and var deposit ? Math.Min(deposit, total) : 0,
            Folio = folio,
            Status = "pending_payment",
            CreatedAt = now,
            UpdatedAt = now
        };
        try
        {
            await bookings.AddAsync(booking, cancellationToken);
        }
        catch (DuplicateBookingIdempotencyException)
        {
            return await bookings.FindByIdempotencyKeyAsync(command.ClientId, command.IdempotencyKey, cancellationToken);
        }

        return booking;
    }
}
