using VirGo.Domain.Providers;

namespace VirGo.Application.Providers;

public sealed record ProviderSearch(
    string? Category,
    string? Zone,
    decimal? MinPrice,
    decimal? MaxPrice,
    decimal? Rating,
    string? Query,
    string? Cursor,
    int Limit);

public sealed class CreateProviderRequest
{
    public string? BusinessName { get; init; }
    public string? Category { get; init; }
    public string? Description { get; init; }
    public string[]? Zones { get; init; }
    public ProviderPackageRequest[]? Packages { get; init; }
    public string[]? Gallery { get; init; }
    public string? CoverUrl { get; init; }
    public string? Pricing { get; init; }
    public string? ResponseTime { get; init; }
}

public sealed class ProviderPackageRequest
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public decimal? Price { get; init; }
    public string[]? Includes { get; init; }
}

public interface IProviderRepository
{
    Task<IReadOnlyList<Provider>> SearchAsync(ProviderSearch search, CancellationToken cancellationToken = default);
    Task<Provider?> FindPublishedAsync(string id, CancellationToken cancellationToken = default);
    Task<Provider?> FindByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Provider> CreateAsync(Provider provider, CancellationToken cancellationToken = default);
    Task<Provider?> UpdateAsync(Provider provider, CancellationToken cancellationToken = default);
}
