namespace VirGo.Domain.Providers;

public sealed class Provider
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string BusinessName { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public required string[] Zones { get; init; }
    public required ProviderPackage[] Packages { get; init; }
    public required string[] Gallery { get; init; }
    public string? CoverUrl { get; init; }
    public required decimal RatingAverage { get; init; }
    public required int RatingCount { get; init; }
    public required string Pricing { get; init; }
    public required string ResponseTime { get; init; }
    public required string Status { get; init; }
}

public sealed record ProviderPackage(string Id, string Name, decimal Price, string[] Includes);
