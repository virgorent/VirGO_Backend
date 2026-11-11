using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VirGo.Application.Abstractions;

namespace VirGo.Infrastructure.Caching;

public sealed partial class DistributedCacheService(
    IDistributedCache cache,
    ILogger<DistributedCacheService> logger) : ICacheService
{
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan timeToLive,
        CancellationToken cancellationToken = default)
    {
        var cached = await TryGetAsync<T>(key, cancellationToken);
        if (cached.Found)
        {
            return cached.Value!;
        }

        var value = await factory(cancellationToken);
        await TrySetAsync(key, value, timeToLive, cancellationToken);
        return value;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
        catch (Exception exception) when (IsCacheFailure(exception, cancellationToken))
        {
            LogRemoveFailed(logger, exception);
        }
    }

    private async Task<(bool Found, T? Value)> TryGetAsync<T>(string key, CancellationToken cancellationToken)
    {
        try
        {
            var bytes = await cache.GetAsync(key, cancellationToken);
            return bytes is null ? (false, default) : (true, JsonSerializer.Deserialize<T>(bytes));
        }
        catch (Exception exception) when (IsCacheFailure(exception, cancellationToken))
        {
            LogReadFailed(logger, exception);
            return (false, default);
        }
    }

    private async Task TrySetAsync<T>(string key, T value, TimeSpan timeToLive, CancellationToken cancellationToken)
    {
        try
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeToLive };
            await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), options, cancellationToken);
        }
        catch (Exception exception) when (IsCacheFailure(exception, cancellationToken))
        {
            LogWriteFailed(logger, exception);
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Cache read failed; using the data source.")]
    private static partial void LogReadFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Cache write failed.")]
    private static partial void LogWriteFailed(ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Cache remove failed.")]
    private static partial void LogRemoveFailed(ILogger logger, Exception exception);

    private static bool IsCacheFailure(Exception exception, CancellationToken cancellationToken) =>
        exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested;
}
