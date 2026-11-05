using System.Globalization;
using VirGo.Application.Abstractions;
using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed class BookingHandler(IVenueRepository venues, IBookingRepository bookings, IClock clock)
{
    public async Task<Booking?> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        if (command.Hours < 1 || !DateOnly.TryParseExact(command.Date, "yyyy-MM-dd", out var date))
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
        await bookings.AddAsync(booking, cancellationToken);
        return booking;
    }
}
