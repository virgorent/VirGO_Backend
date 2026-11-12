using System.Security.Cryptography;
using System.Text;
using VirGo.Application.Abstractions;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure.Caching;

public sealed class CachedVenueRepository(IVenueRepository inner, ICacheService cache) : IVenueRepository
{
    private static readonly TimeSpan ListTimeToLive = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan DetailTimeToLive = TimeSpan.FromSeconds(60);

    public async Task<IReadOnlyList<Venue>> ListPublishedAsync(
        string? zone,
        string? category,
        CancellationToken cancellationToken = default)
    {
        var venues = await cache.GetOrCreateAsync(
            ListKey(zone, category),
            async token => (await inner.ListPublishedAsync(zone, category, token)).ToArray(),
            ListTimeToLive,
            cancellationToken);
        return venues;
    }

    public async Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        var entry = await cache.GetOrCreateAsync(
            DetailKey(id),
            async token => new VenueEntry(await inner.FindPublishedAsync(id, token)),
            DetailTimeToLive,
            cancellationToken);
        return entry.Venue;
    }

    public static string ListKey(string? zone, string? category) => $"cache:venues:{Hash($"{zone}\u001f{category}")}";

    public static string DetailKey(string id) => $"cache:venue:{Hash(id)}";

    // Las llaves se derivan con un hash para que un filtro enviado por el cliente no controle la llave de Redis.
    private static string Hash(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private sealed record VenueEntry(Venue? Venue);
}
