namespace VirGo.Api.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string HeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId) || correlationId.Length > 100)
        {
            correlationId = Guid.NewGuid().ToString("D");
        }

        context.Items[HeaderName] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        var scope = new Dictionary<string, object>(StringComparer.Ordinal) { ["CorrelationId"] = correlationId };
        var traceId = TraceIds.RootFrom(Environment.GetEnvironmentVariable("_X_AMZN_TRACE_ID"));
        if (traceId is not null)
        {
            scope["TraceId"] = traceId;
        }

        using (logger.BeginScope(scope))
        {
            await next(context);
        }
    }
}

/// <summary>Reads the X-Ray root id from the Lambda trace header: "Root=1-…;Parent=…;Sampled=1" → "1-…".</summary>
public static class TraceIds
{
    public static string? RootFrom(string? header)
    {
        if (string.IsNullOrWhiteSpace(header))
        {
            return null;
        }

        foreach (var part in header.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (part.StartsWith("Root=", StringComparison.Ordinal) && part.Length > 5)
            {
                return part[5..];
            }
        }

        return null;
    }
}
