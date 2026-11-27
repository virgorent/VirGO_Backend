namespace VirGo.Domain.Events;

public sealed class EventTask
{
    public required string Id { get; init; }
    public required string EventId { get; init; }
    public required string Title { get; init; }
    public required string DueDate { get; init; }
    public required bool Done { get; init; }
    public required bool Suggested { get; init; }
}
