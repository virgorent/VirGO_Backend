using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace VirGo.Infrastructure;

public sealed class MongoReadinessCheck(IMongoDatabase database) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await database.RunCommandAsync(
                new BsonDocumentCommand<BsonDocument>(new BsonDocument("ping", 1)),
                cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("MongoDB no está disponible.", exception);
        }
    }
}
