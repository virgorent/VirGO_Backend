using VirGo.Domain.Events;

namespace VirGo.Application.Events;

public interface IEventGuestRepository
{
    Task<IReadOnlyList<EventGuest>> ListAsync(string eventId, CancellationToken cancellationToken = default);
    Task<EventGuest> CreateAsync(EventGuest guest, CancellationToken cancellationToken = default);
    Task<EventGuest?> UpdateAsync(EventGuest guest, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string eventId, string guestId, CancellationToken cancellationToken = default);
}

public sealed class CreateGuestRequest
{
    public string? Name { get; init; }
    public string? Phone { get; init; }
    public int? Companions { get; init; }
    public string? Status { get; init; }
    public string? Table { get; init; }
}
