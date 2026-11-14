namespace VirGo.Domain.Notifications;

public sealed class Notice
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Kind { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    public required string Link { get; init; }
    public required bool Read { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime PurgeAt { get; init; }
}
