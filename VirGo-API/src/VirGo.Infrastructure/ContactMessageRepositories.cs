using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Contact;
using VirGo.Domain.Contact;

namespace VirGo.Infrastructure;

public sealed class InMemoryContactMessageRepository : IContactMessageRepository
{
    private readonly List<ContactMessage> messages = [];

    public Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        messages.Add(message);
        return Task.CompletedTask;
    }
}

public sealed class MongoContactMessageRepository(IMongoDatabase database) : IContactMessageRepository
{
    private readonly IMongoCollection<BsonDocument> messages = database.GetCollection<BsonDocument>("contactMessages");

    public Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
        {
            ["_id"] = message.Id,
            ["name"] = message.Name,
            ["email"] = message.Email,
            ["message"] = message.Message,
            ["createdAt"] = new BsonDateTime(message.CreatedAt)
        };
        if (message.UserId is not null)
        {
            document["userId"] = message.UserId;
        }

        return messages.InsertOneAsync(document, cancellationToken: cancellationToken);
    }
}
