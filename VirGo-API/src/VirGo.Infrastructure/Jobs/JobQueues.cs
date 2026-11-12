using System.Collections.Concurrent;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using VirGo.Application.Jobs;

namespace VirGo.Infrastructure.Jobs;

/// <summary>Local and test queue: keeps the published messages in memory.</summary>
public sealed class InMemoryJobPublisher : IJobPublisher
{
    private readonly ConcurrentQueue<JobMessage> messages = new();

    public IReadOnlyCollection<JobMessage> Messages => messages.ToArray();

    public Task PublishAsync(JobMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        cancellationToken.ThrowIfCancellationRequested();
        messages.Enqueue(message);
        return Task.CompletedTask;
    }
}

/// <summary>Publishes to Amazon SQS. A failed publish is logged and swallowed (fail-open).</summary>
public sealed partial class SqsJobPublisher(IAmazonSQS sqs, string queueUrl, ILogger<SqsJobPublisher> logger) : IJobPublisher
{
    public async Task PublishAsync(JobMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        var request = new SendMessageRequest
        {
            QueueUrl = queueUrl,
            MessageBody = message.ToJson(),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                ["type"] = new() { DataType = "String", StringValue = message.Type }
            }
        };
        if (!string.IsNullOrWhiteSpace(message.CorrelationId))
        {
            request.MessageAttributes["correlationId"] = new() { DataType = "String", StringValue = message.CorrelationId };
        }

        try
        {
            await sqs.SendMessageAsync(request, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
        {
            LogPublishFailed(logger, exception, message.Type, message.CorrelationId);
        }
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Could not publish job {Type} (correlation {CorrelationId}); continuing without it.")]
    private static partial void LogPublishFailed(ILogger logger, Exception exception, string type, string? correlationId);
}
