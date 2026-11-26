using System.Globalization;
using VirGo.Application.Abstractions;
using VirGo.Application.Jobs;
using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed class BookingHandler(IVenueRepository venues, IBookingRepository bookings, IQuoteRepository quotes, IClock clock, IJobPublisher jobs)
{
    public async Task<BookingCreationResult> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken = default)
    {
        var existing = await bookings.FindByIdempotencyKeyAsync(command.ClientId, command.IdempotencyKey, cancellationToken);
        if (existing is not null)
        {
            return new(existing, false);
        }

        if (!Guid.TryParse(command.VenueId, out _)
            || command.IdempotencyKey.Length > 128
            || command.Hours < 1
            || !DateOnly.TryParseExact(command.Date, "yyyy-MM-dd", out var date))
        {
            return new(null, true);
        }

        var venue = await venues.FindPublishedAsync(command.VenueId, cancellationToken);
        if (venue is null)
        {
            return new(null, false);
        }

        if (date < DateOnly.FromDateTime(clock.UtcNow))
        {
            return new(null, true);
        }

        var now = clock.UtcNow;
        if (command.Hours < venue.MinimumHours)
        {
            return new(null, true);
        }

        // Con cotización, el precio es el que respondió el propietario y la fecha es la cotizada.
        Quote? quote = null;
        if (!string.IsNullOrWhiteSpace(command.QuoteId))
        {
            quote = await quotes.FindVisibleAsync(command.QuoteId, command.ClientId, [], cancellationToken);
            if (!IsBookable(quote, command, venue.Id))
            {
                return new(null, true);
            }
        }

        var billingUnits = string.Equals(venue.PriceUnit, "day", StringComparison.OrdinalIgnoreCase)
            ? (int)Math.Ceiling(command.Hours / 24m)
            : command.Hours;
        var total = quote?.Response?.Total ?? venue.BasePrice * billingUnits;
        var deposit = command.Deposit
            ?? quote?.Response?.Deposit
            ?? Math.Round(total * 0.30m, 2, MidpointRounding.AwayFromZero);
        if (deposit < 0 || deposit > total)
        {
            return new(null, true);
        }

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
            Deposit = deposit,
            Folio = folio,
            Status = "pending_payment",
            CreatedAt = now,
            UpdatedAt = now
        };
        // Reservar con la cotización equivale a aceptarla. Se aparta antes de guardar la reserva
        // (de answered a accepted, solo si sigue answered): así una cotización no genera dos reservas
        // aunque lleguen varias peticiones a la vez con claves distintas.
        if (quote is not null && await quotes.UpdateAsync(Accepted(quote, now), "answered", cancellationToken) is null)
        {
            var replay = await bookings.FindByIdempotencyKeyAsync(command.ClientId, command.IdempotencyKey, cancellationToken);
            return replay is not null ? new(replay, false) : new(null, true);
        }

        try
        {
            await bookings.AddAsync(booking, cancellationToken);
        }
        catch (DuplicateBookingIdempotencyException)
        {
            return new(await bookings.FindByIdempotencyKeyAsync(command.ClientId, command.IdempotencyKey, cancellationToken), false);
        }
        catch when (quote is not null)
        {
            // La reserva no se guardó: la cotización vuelve a estar disponible.
            await quotes.UpdateAsync(Reverted(quote, clock.UtcNow), "accepted", CancellationToken.None);
            throw;
        }

        await jobs.PublishAsync(
            new JobMessage(
                JobTypes.BookingCreated,
                new Dictionary<string, string>
                {
                    ["bookingId"] = booking.Id,
                    ["clientId"] = booking.ClientId,
                    ["folio"] = booking.Folio,
                    ["date"] = booking.Date
                },
                command.CorrelationId),
            cancellationToken);

        return new(booking, false);
    }

    private static bool IsBookable(Quote? quote, CreateBookingCommand command, string venueId) =>
        quote is not null
        && quote.ClientId == command.ClientId
        && quote.Status == "answered"
        && quote.Response is not null
        && string.Equals(quote.TargetType, "venue", StringComparison.OrdinalIgnoreCase)
        && quote.TargetId == venueId
        && quote.EventDate == command.Date;

    private static Quote Accepted(Quote quote, DateTime now) => WithStatus(quote, "accepted", now);

    private static Quote Reverted(Quote quote, DateTime now) => WithStatus(quote, "answered", now);

    private static Quote WithStatus(Quote quote, string status, DateTime now) => new()
    {
        Id = quote.Id,
        ClientId = quote.ClientId,
        ClientName = quote.ClientName,
        TargetType = quote.TargetType,
        TargetId = quote.TargetId,
        TargetName = quote.TargetName,
        EventId = quote.EventId,
        EventType = quote.EventType,
        EventDate = quote.EventDate,
        Guests = quote.Guests,
        RequestedServices = quote.RequestedServices,
        Message = quote.Message,
        Response = quote.Response,
        Status = status,
        CreatedAt = quote.CreatedAt,
        UpdatedAt = now
    };
}
