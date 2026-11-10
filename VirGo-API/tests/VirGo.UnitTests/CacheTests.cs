using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;
using VirGo.Infrastructure.Caching;

namespace VirGo.UnitTests;

public sealed class CacheTests
{
    [Fact]
    public async Task SecondReadUsesTheCache()
    {
        var calls = 0;
        var service = Service(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())));

        for (var i = 0; i < 2; i++)
        {
            var value = await service.GetOrCreateAsync("key", _ => Task.FromResult(++calls), TimeSpan.FromMinutes(1));
            Assert.Equal(1, value);
        }

        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task RemovedKeyReadsTheSourceAgain()
    {
        var calls = 0;
        var service = Service(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())));

        await service.GetOrCreateAsync("key", _ => Task.FromResult(++calls), TimeSpan.FromMinutes(1));
        await service.RemoveAsync("key");
        await service.GetOrCreateAsync("key", _ => Task.FromResult(++calls), TimeSpan.FromMinutes(1));

        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task CacheFailureFallsBackToTheSource()
    {
        var service = Service(new BrokenCache());

        var value = await service.GetOrCreateAsync("key", _ => Task.FromResult(42), TimeSpan.FromMinutes(1));
        await service.RemoveAsync("key");

        Assert.Equal(42, value);
    }

    [Fact]
    public async Task CancellationIsNotTreatedAsACacheFailure()
    {
        using var source = new CancellationTokenSource();
        await source.CancelAsync();
        var service = Service(new BrokenCache(new OperationCanceledException()));

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            service.GetOrCreateAsync("key", _ => Task.FromResult(1), TimeSpan.FromMinutes(1), source.Token));
    }

    [Fact]
    public async Task VenueListIsCachedPerFilter()
    {
        var inner = new CountingVenues();
        var repository = new CachedVenueRepository(inner, Service(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()))));

        await repository.ListPublishedAsync("Centro", null);
        await repository.ListPublishedAsync("Centro", null);
        await repository.ListPublishedAsync("Norte", null);

        Assert.Equal(2, inner.ListCalls);
    }

    [Fact]
    public async Task MissingVenueIsCachedAsMissing()
    {
        var inner = new CountingVenues();
        var repository = new CachedVenueRepository(inner, Service(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()))));

        Assert.Null(await repository.FindPublishedAsync("missing"));
        Assert.Null(await repository.FindPublishedAsync("missing"));

        Assert.Equal(1, inner.FindCalls);
    }

    [Fact]
    public async Task CachedVenueKeepsItsData()
    {
        var inner = new CountingVenues { Venue = Sample() };
        var repository = new CachedVenueRepository(inner, Service(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()))));

        await repository.FindPublishedAsync("v1");
        var cached = await repository.FindPublishedAsync("v1");

        Assert.Equal(1, inner.FindCalls);
        Assert.Equal("Casa Oliva", cached!.Name);
        Assert.Equal(45000.50m, cached.BasePrice);
        Assert.Equal(["Jardin"], cached.Categories);
    }

    [Fact]
    public void KeysDoNotExposeClientInput()
    {
        var key = CachedVenueRepository.ListKey("evil\r\nFLUSHALL", null);

        Assert.DoesNotContain("evil", key);
        Assert.DoesNotContain("\n", key);
        Assert.NotEqual(CachedVenueRepository.ListKey("a", "b"), CachedVenueRepository.ListKey("ab", null));
    }

    private static DistributedCacheService Service(IDistributedCache cache) =>
        new(cache, NullLogger<DistributedCacheService>.Instance);

    private static Venue Sample() => new()
    {
        Id = "v1", OwnerId = "o1", Name = "Casa Oliva", ShortName = "C. Oliva", Description = "Salón de eventos", Address = "Centro",
        Zone = "Centro", Capacity = 100, BasePrice = 45000.50m, PriceUnit = "day", MinimumHours = 4, Categories = ["Jardin"],
        Amenities = [], Photos = [], Status = "published", RatingAverage = 4.5m, RatingCount = 2
    };

    private sealed class CountingVenues : IVenueRepository
    {
        public int ListCalls { get; private set; }
        public int FindCalls { get; private set; }
        public Venue? Venue { get; init; }

        public Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default)
        {
            ListCalls++;
            return Task.FromResult<IReadOnlyList<Venue>>([]);
        }

        public Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
        {
            FindCalls++;
            return Task.FromResult(Venue);
        }
    }

    private sealed class BrokenCache(Exception? failure = null) : IDistributedCache
    {
        private readonly Exception failure = failure ?? new InvalidOperationException("Redis is down.");

        public byte[]? Get(string key) => throw failure;
        public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => Task.FromException<byte[]?>(failure);
        public void Refresh(string key) => throw failure;
        public Task RefreshAsync(string key, CancellationToken token = default) => Task.FromException(failure);
        public void Remove(string key) => throw failure;
        public Task RemoveAsync(string key, CancellationToken token = default) => Task.FromException(failure);
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => throw failure;
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) => Task.FromException(failure);
    }
}
