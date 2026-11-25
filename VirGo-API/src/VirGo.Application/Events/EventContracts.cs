using VirGo.Domain.Events;

namespace VirGo.Application.Events;

public interface IEventRepository
{
    Task<IReadOnlyList<VirGoEvent>> ListForUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> FindForUserAsync(string id, string userId, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> FindForEditAsync(string id, string userId, CancellationToken cancellationToken = default);
    Task<VirGoEvent> CreateAsync(VirGoEvent item, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> UpdateForUserAsync(VirGoEvent item, string userId, CancellationToken cancellationToken = default);
    Task<VirGoEvent?> UpdateCollaboratorsAsync(string eventId, string ownerId, EventCollaborator[] collaborators, CancellationToken cancellationToken = default);
}

public sealed class CollaboratorRequest
{
    public string? UserId { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Permission { get; init; }
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
