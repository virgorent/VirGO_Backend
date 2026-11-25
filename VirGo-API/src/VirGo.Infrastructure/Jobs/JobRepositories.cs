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

    public Task<IReadOnlyList<Notice>> ListByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<Notice> result = notices.Values
            .Where(notice => notice.UserId == userId)
            .OrderByDescending(notice => notice.CreatedAt)
            .Take(50)
            .ToArray();
        return Task.FromResult(result);
    }

    public Task<Notice?> MarkReadAsync(string userId, string noticeId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!notices.TryGetValue(noticeId, out var notice) || notice.UserId != userId)
        {
            return Task.FromResult<Notice?>(null);
        }

        var updated = new Notice
        {
            Id = notice.Id,
            UserId = notice.UserId,
            Kind = notice.Kind,
            Title = notice.Title,
            Body = notice.Body,
            Link = notice.Link,
            Read = true,
            CreatedAt = notice.CreatedAt,
            PurgeAt = notice.PurgeAt
        };
        notices[noticeId] = updated;
        return Task.FromResult<Notice?>(updated);
    }

    public Task<long> MarkAllReadAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        long updatedCount = 0;
        foreach (var pair in notices.Where(pair => pair.Value.UserId == userId && !pair.Value.Read).ToArray())
        {
            var notice = pair.Value;
            notices[pair.Key] = new Notice
            {
                Id = notice.Id,
                UserId = notice.UserId,
                Kind = notice.Kind,
                Title = notice.Title,
                Body = notice.Body,
                Link = notice.Link,
                Read = true,
                CreatedAt = notice.CreatedAt,
                PurgeAt = notice.PurgeAt
            };
            updatedCount++;
        }

        return Task.FromResult(updatedCount);
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

    public async Task<IReadOnlyList<Notice>> ListByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
        var documents = await notices.Find(filter)
            .SortByDescending(document => document["createdAt"])
            .Limit(50)
            .ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Notice?> MarkReadAsync(string userId, string noticeId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", noticeId),
            Builders<BsonDocument>.Filter.Eq("userId", userId));
        var update = Builders<BsonDocument>.Update.Set("read", true);
        var document = await notices.FindOneAndUpdateAsync(
            filter,
            update,
            new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After },
            cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<long> MarkAllReadAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("userId", userId),
            Builders<BsonDocument>.Filter.Eq("read", false));
        var update = Builders<BsonDocument>.Update.Set("read", true);
        var result = await notices.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount;
    }

    private static Notice Map(BsonDocument document) => new()
    {
        Id = document.GetValue("_id").AsString,
        UserId = document.GetValue("userId").AsString,
        Kind = document.GetValue("kind").AsString,
        Title = document.GetValue("title").AsString,
        Body = document.GetValue("body").AsString,
        Link = document.GetValue("link").AsString,
        Read = document.GetValue("read").AsBoolean,
        CreatedAt = document.GetValue("createdAt").ToUniversalTime(),
        PurgeAt = document.GetValue("purgeAt").ToUniversalTime()
    };
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
