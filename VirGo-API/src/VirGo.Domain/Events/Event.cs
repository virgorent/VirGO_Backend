namespace VirGo.Domain.Events;

public sealed class VirGoEvent
{
    public required string Id { get; init; }
    public required string OwnerId { get; init; }
    public required string Name { get; init; }
    public required string EventType { get; init; }
    public required string Date { get; init; }
    public required int Guests { get; init; }
    public required decimal Budget { get; init; }
    public required int Hours { get; init; }
    public EventVenue? Venue { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
}

public sealed record EventVenue(string VenueId, string Name, string PhotoUrl);
