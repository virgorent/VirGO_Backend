namespace VirGo.Domain.Marketplace;

public sealed class Review
{
    public required string Id { get; init; }
    public required string TargetType { get; init; }
    public required string TargetId { get; init; }
    public required string AuthorId { get; init; }
    public required string AuthorName { get; init; }
    public required int Stars { get; init; }
    public required string Text { get; init; }
    public string? BookingId { get; init; }
    public ReviewReply? Reply { get; init; }
    public required DateTime CreatedAt { get; init; }
}

public sealed class ReviewReply
{
    public required string AuthorId { get; init; }
    public required string Text { get; init; }
    public required DateTime CreatedAt { get; init; }
}
