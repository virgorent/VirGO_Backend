using VirGo.Application.Abstractions;
using VirGo.Domain.Notifications;

namespace VirGo.Application.Jobs;

/// <summary>Creates the in-app notice for a new booking. Idempotent: the notice id derives from the booking id.</summary>
public sealed class BookingCreatedHandler(INoticeRepository notices, IClock clock) : IJobHandler
{
    private static readonly TimeSpan Retention = TimeSpan.FromDays(90);

    public string Type => JobTypes.BookingCreated;

    public async Task HandleAsync(JobMessage message, CancellationToken cancellationToken = default)
    {
        var bookingId = message.Get("bookingId");
        var now = clock.UtcNow;
        var notice = new Notice
        {
            Id = $"booking-created-{bookingId}",
            UserId = message.Get("clientId"),
            Kind = "booking",
            Title = "Reservación registrada",
            Body = $"Tu reservación {message.Get("folio")} para el {message.Get("date")} quedó registrada y está pendiente de pago.",
            Link = $"/bookings/{bookingId}",
            Read = false,
            CreatedAt = now,
            PurgeAt = now.Add(Retention)
        };

        await notices.AddIfAbsentAsync(notice, cancellationToken);
    }
}
