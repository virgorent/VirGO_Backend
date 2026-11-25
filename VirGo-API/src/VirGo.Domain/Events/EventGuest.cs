namespace VirGo.Domain.Events;

public sealed class EventGuest
{
    public required string Id { get; init; }
    public required string EventId { get; init; }
    public required string Name { get; init; }
    public required string Phone { get; init; }
    public required int Companions { get; init; }
    public required string Status { get; init; }
    public string? Table { get; init; }
}
