using VirGo.Domain.Events;

namespace VirGo.Application.Events;

// Solo el dueño (ownerId) lee y modifica su evento; cualquier otro usuario obtiene null (404).
public interface IEventRepository
{
    Task<IReadOnlyList<VirGoEvent>> ListForOwnerAsync(string ownerId, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> FindForOwnerAsync(string id, string ownerId, CancellationToken cancellationToken = default);
    Task<VirGoEvent> CreateAsync(VirGoEvent item, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> UpdateForOwnerAsync(VirGoEvent item, string ownerId, CancellationToken cancellationToken = default);
}

public sealed class CreateEventRequest
{
    public string? Name { get; init; }
    public string? EventType { get; init; }
    public string? Date { get; init; }
    public int? Guests { get; init; }
    public decimal? Budget { get; init; }
    public int? Hours { get; init; }
    public string? VenueId { get; init; }
}
