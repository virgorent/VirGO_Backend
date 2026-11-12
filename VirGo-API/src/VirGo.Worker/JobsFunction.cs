using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.SQSEvents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using VirGo.Application.Jobs;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace VirGo.Worker;

/// <summary>Consumes the virgo-jobs queue. Reports failures per message so only those are retried (099, RNF-03).</summary>
public sealed partial class JobsFunction
{
    private static readonly Lazy<ServiceProvider> DefaultServices = new(WorkerStartup.BuildServices);
    private readonly IServiceProvider services;

    public JobsFunction() : this(DefaultServices.Value)
    {
    }

    public JobsFunction(IServiceProvider services)
    {
        this.services = services;
    }

    public async Task<SQSBatchResponse> FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
    {
        ArgumentNullException.ThrowIfNull(sqsEvent);
        var logger = services.GetRequiredService<ILogger<JobsFunction>>();
        var failures = new List<SQSBatchResponse.BatchItemFailure>();

        foreach (var record in sqsEvent.Records)
        {
            await using var scope = services.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<JobDispatcher>();
            try
            {
                await dispatcher.DispatchAsync(record.Body);
            }
            catch (Exception exception) when (exception is not OutOfMemoryException)
            {
                LogFailed(logger, exception, record.MessageId);
                failures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = record.MessageId });
            }
        }

        return new SQSBatchResponse(failures);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Job message {MessageId} failed; it will be retried.")]
    private static partial void LogFailed(ILogger logger, Exception exception, string messageId);
}
