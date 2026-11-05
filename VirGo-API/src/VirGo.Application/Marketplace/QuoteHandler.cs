using VirGo.Application.Abstractions;
using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed class QuoteHandler(IVenueRepository venues, IQuoteRepository quotes, IClock clock)
{
    public async Task<Quote?> HandleAsync(CreateQuoteCommand command, CancellationToken cancellationToken = default)
    {
        if (!string.Equals(command.TargetType, "venue", StringComparison.OrdinalIgnoreCase)
            || command.Guests < 1
            || command.EventDate < DateOnly.FromDateTime(clock.UtcNow))
        {
            return null;
        }

        var venue = await venues.FindPublishedAsync(command.TargetId, cancellationToken);
        if (venue is null)
        {
            return null;
        }

        var now = clock.UtcNow;
        var quote = new Quote
        {
            Id = Guid.NewGuid().ToString("D"),
            ClientId = command.ClientId,
            ClientName = command.ClientName,
            TargetType = "venue",
            TargetId = venue.Id,
            TargetName = venue.Name,
            EventId = string.IsNullOrWhiteSpace(command.EventId) ? null : command.EventId,
            EventType = command.EventType.Trim(),
            EventDate = command.EventDate.ToString("yyyy-MM-dd"),
            Guests = command.Guests,
            RequestedServices = command.RequestedServices,
            Message = command.Message,
            Status = "sent",
            CreatedAt = now,
            UpdatedAt = now
        };
        await quotes.AddAsync(quote, cancellationToken);
        return quote;
    }
}
