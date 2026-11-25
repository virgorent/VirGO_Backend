namespace VirGo.Domain.Assistant;

public sealed class AssistantMessage
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Role { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime PurgeAt { get; init; }
}
