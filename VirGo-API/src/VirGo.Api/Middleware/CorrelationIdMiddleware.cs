using System.Text.RegularExpressions;

namespace VirGo.Api.Middleware;

public sealed partial class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string HeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = Sanitize(context.Request.Headers[HeaderName].FirstOrDefault());

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

    /// <summary>
    /// The header comes from the client and ends up in logs and queue messages (log forging, CWE-117).
    /// Only 1–100 letters, digits, '.', '_' or '-' are kept; anything else is replaced by a new GUID.
    /// </summary>
    public static string Sanitize(string? value) =>
        value is not null && SafeCorrelationId().IsMatch(value)
            ? value.Replace("\r", string.Empty, StringComparison.Ordinal).Replace("\n", string.Empty, StringComparison.Ordinal)
            : Guid.NewGuid().ToString("D");

    [GeneratedRegex(@"\A[A-Za-z0-9._-]{1,100}\z", RegexOptions.CultureInvariant)]
    private static partial Regex SafeCorrelationId();
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
