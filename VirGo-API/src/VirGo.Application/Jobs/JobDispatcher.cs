using Microsoft.Extensions.Logging;

namespace VirGo.Application.Jobs;

public sealed partial class JobDispatcher(IEnumerable<IJobHandler> handlers, ILogger<JobDispatcher> logger)
{
    private readonly Dictionary<string, IJobHandler> handlersByType =
        handlers.ToDictionary(handler => handler.Type, StringComparer.Ordinal);

    /// <summary>Runs the handler for the message. Unknown or malformed messages are logged and discarded.</summary>
    public async Task DispatchAsync(string body, CancellationToken cancellationToken = default)
    {
        JobMessage message;
        try
        {
            message = JobMessage.Parse(body);
        }
        catch (InvalidJobMessageException exception)
        {
            LogDiscarded(logger, exception.Message);
            return;
        }

        if (!handlersByType.TryGetValue(message.Type, out var handler))
        {
            LogUnknownType(logger, message.Type, message.CorrelationId);
            return;
        }

        try
        {
            await handler.HandleAsync(message, cancellationToken);
        }
        catch (InvalidJobMessageException exception)
        {
            LogDiscarded(logger, exception.Message);
            return;
        }

        LogHandled(logger, message.Type, message.CorrelationId);
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Job message discarded: {Reason}")]
    private static partial void LogDiscarded(ILogger logger, string reason);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Unknown job type {Type}; message discarded (correlation {CorrelationId}).")]
    private static partial void LogUnknownType(ILogger logger, string type, string? correlationId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Job {Type} handled (correlation {CorrelationId}).")]
    private static partial void LogHandled(ILogger logger, string type, string? correlationId);
}
