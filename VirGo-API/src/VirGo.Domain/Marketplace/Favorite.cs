namespace VirGo.Domain.Marketplace;

public sealed class Favorite
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string TargetType { get; init; }
    public required string TargetId { get; init; }
    public required string Name { get; init; }
    public string? PhotoUrl { get; init; }
    public required decimal Rating { get; init; }
    public required DateTime CreatedAt { get; init; }
}
