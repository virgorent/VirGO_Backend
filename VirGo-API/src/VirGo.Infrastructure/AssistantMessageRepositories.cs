using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Assistant;
using VirGo.Domain.Assistant;

namespace VirGo.Infrastructure;

public sealed class InMemoryAssistantMessageRepository : IAssistantMessageRepository
{
    private readonly List<AssistantMessage> messages =
    [
        new AssistantMessage
        {
            Id = "99999999-9999-4999-8999-999999999999",
            UserId = "8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1",
            Role = "assistant",
            Text = "Puedo ayudarte a planear tu evento.",
            CreatedAt = DateTime.UtcNow,
            PurgeAt = DateTime.UtcNow.AddDays(30)
        }
    ];

    public Task<AssistantMessage> AddAsync(AssistantMessage message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        messages.Add(message);
        return Task.FromResult(message);
    }

    public Task<IReadOnlyList<AssistantMessage>> ListByUserAsync(
        string userId,
        DateTime now,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<AssistantMessage>>(messages
            .Where(message => message.UserId == userId && message.PurgeAt > now)
            .OrderByDescending(message => message.CreatedAt)
            .Take(50)
            .ToArray());
    }
}

public sealed class MongoAssistantMessageRepository(IMongoDatabase database) : IAssistantMessageRepository
{
    private readonly IMongoCollection<BsonDocument> messages = database.GetCollection<BsonDocument>("assistantMessages");

    public async Task<AssistantMessage> AddAsync(AssistantMessage message, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument { ["_id"] = message.Id, ["userId"] = message.UserId, ["role"] = message.Role, ["text"] = message.Text, ["createdAt"] = new BsonDateTime(message.CreatedAt), ["purgeAt"] = new BsonDateTime(message.PurgeAt) };
        await messages.InsertOneAsync(document, cancellationToken: cancellationToken);
        return message;
    }

    public async Task<IReadOnlyList<AssistantMessage>> ListByUserAsync(
        string userId,
        DateTime now,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("userId", userId),
            Builders<BsonDocument>.Filter.Gt("purgeAt", now));
        var documents = await messages.Find(filter)
            .SortByDescending(document => document["createdAt"])
            .Limit(50)
            .ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    private static AssistantMessage Map(BsonDocument document) => new()
    {
        Id = document["_id"].AsString,
        UserId = document["userId"].AsString,
        Role = document["role"].AsString,
        Text = document["text"].AsString,
        CreatedAt = document["createdAt"].ToUniversalTime(),
        PurgeAt = document["purgeAt"].ToUniversalTime()
    };
}
