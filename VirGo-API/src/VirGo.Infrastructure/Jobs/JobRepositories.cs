using System.Collections.Concurrent;
using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Jobs;
using VirGo.Domain.Notifications;

namespace VirGo.Infrastructure.Jobs;

public sealed class InMemoryNoticeRepository : INoticeRepository
{
    private readonly ConcurrentDictionary<string, Notice> notices = new(StringComparer.Ordinal);

    public IReadOnlyCollection<Notice> Notices => notices.Values.ToArray();

    public Task<bool> AddIfAbsentAsync(Notice notice, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notice);
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(notices.TryAdd(notice.Id, notice));
    }
}

public sealed class MongoNoticeRepository(IMongoDatabase database) : INoticeRepository
{
    private readonly IMongoCollection<BsonDocument> notices = database.GetCollection<BsonDocument>("notices");

    public async Task<bool> AddIfAbsentAsync(Notice notice, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notice);
        var document = new BsonDocument
        {
            ["_id"] = notice.Id,
            ["userId"] = notice.UserId,
            ["kind"] = notice.Kind,
            ["title"] = notice.Title,
            ["body"] = notice.Body,
            ["link"] = notice.Link,
            ["read"] = notice.Read,
            ["createdAt"] = notice.CreatedAt,
            ["purgeAt"] = notice.PurgeAt
        };

        try
        {
            await notices.InsertOneAsync(document, cancellationToken: cancellationToken);
            return true;
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Category == ServerErrorCategory.DuplicateKey)
        {
            return false;
        }
    }
}

public sealed class MongoQuoteExpiry(IMongoDatabase database) : IQuoteExpiry
{
    private static readonly string[] ExpirableStatuses = ["sent", "answered"];
    private readonly IMongoCollection<BsonDocument> quotes = database.GetCollection<BsonDocument>("quotes");

    public async Task<long> ExpireBeforeAsync(DateOnly today, DateTime now, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.In("status", ExpirableStatuses),
            Builders<BsonDocument>.Filter.Lt("eventDate", today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
        var update = Builders<BsonDocument>.Update
            .Set("status", "expired")
            .Set("updatedAt", now);
        var result = await quotes.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }
}
