using Microsoft.Extensions.Logging;
using VirGo.Application.Abstractions;

namespace VirGo.Application.Jobs;

/// <summary>Expires quotes whose event date already passed in Mexico City (UTC−6, no daylight saving time since 2022).</summary>
public sealed partial class QuotesExpireHandler(IQuoteExpiry quotes, IClock clock, ILogger<QuotesExpireHandler> logger) : IJobHandler
{
    private static readonly TimeSpan MexicoCityOffset = TimeSpan.FromHours(-6);

    public string Type => JobTypes.QuotesExpire;

    public static DateOnly MexicoCityToday(DateTime utcNow) => DateOnly.FromDateTime(utcNow.Add(MexicoCityOffset));

    public async Task HandleAsync(JobMessage message, CancellationToken cancellationToken = default)
    {
        var now = clock.UtcNow;
        var expired = await quotes.ExpireBeforeAsync(MexicoCityToday(now), now, cancellationToken);
        LogExpired(logger, expired);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Expired {Count} quotes.")]
    private static partial void LogExpired(ILogger logger, long count);
}
