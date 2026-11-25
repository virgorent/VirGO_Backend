namespace VirGo.Domain.Marketplace;

public sealed class Conversation
{
    public required string Id { get; init; }
    public required string ClientId { get; init; }
    public required string TargetType { get; init; }
    public required string TargetId { get; init; }
    public required string TargetUserId { get; init; }
    public required string TargetName { get; init; }
    public string? QuoteId { get; init; }
    public required string LastMessage { get; init; }
    public required DateTime LastAt { get; init; }
}

public sealed class ConversationMessage
{
    public required string Id { get; init; }
    public required string ConversationId { get; init; }
    public required string SenderId { get; init; }
    public required string Text { get; init; }
    public required DateTime SentAt { get; init; }
    public DateTime? ReadAt { get; init; }
}
