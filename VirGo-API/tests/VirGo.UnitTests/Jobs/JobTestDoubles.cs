using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Logging;
using VirGo.Application.Abstractions;
using VirGo.Application.Email;

namespace VirGo.UnitTests.Jobs;

internal sealed class FixedClock(DateTime utcNow) : IClock
{
    public DateTime UtcNow { get; } = utcNow;
}

internal sealed class CapturingEmailSender : IEmailSender
{
    public List<EmailMessage> Sent { get; } = [];

    public bool Fail { get; init; }

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (Fail)
        {
            throw new InvalidOperationException("SES is not available.");
        }

        Sent.Add(message);
        return Task.CompletedTask;
    }
}

/// <summary>Collects every formatted log line, to check what reaches the logs.</summary>
internal sealed class CapturingLoggerProvider : ILoggerProvider
{
    public List<string> Lines { get; } = [];

    public ILogger CreateLogger(string categoryName) => new CapturingLogger(Lines);

    public void Dispose()
    {
    }

    private sealed class CapturingLogger(List<string> lines) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (lines)
            {
                lines.Add(formatter(state, exception));
            }
        }
    }
}

/// <summary>An SQS client whose send always fails, to prove the publisher is fail-open.</summary>
internal sealed class FailingSqsClient() : AmazonSQSClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1)
{
    public int Attempts { get; private set; }

    public override Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        Attempts++;
        throw new AmazonSQSException("Queue unavailable.");
    }
}

/// <summary>An SQS client that records the request instead of sending it.</summary>
internal sealed class RecordingSqsClient() : AmazonSQSClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.USEast1)
{
    public List<SendMessageRequest> Requests { get; } = [];

    public override Task<SendMessageResponse> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        Requests.Add(request);
        return Task.FromResult(new SendMessageResponse { MessageId = Guid.NewGuid().ToString("D") });
    }
}
