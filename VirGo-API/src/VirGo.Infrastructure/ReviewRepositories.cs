using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure;

public sealed class InMemoryReviewRepository : IReviewRepository
{
    private readonly List<Review> reviews = [];
    private readonly Lock gate = new();

    public Task<IReadOnlyList<Review>> ListAsync(string targetType, string targetId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult<IReadOnlyList<Review>>(reviews.Where(review => review.TargetType == targetType && review.TargetId == targetId).OrderByDescending(review => review.CreatedAt).ToArray());
    }

    public Task<Review?> AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            if (reviews.Any(item => item.AuthorId == review.AuthorId && item.TargetType == review.TargetType && item.TargetId == review.TargetId)) return Task.FromResult<Review?>(null);
            reviews.Add(review);
            return Task.FromResult<Review?>(review);
        }
    }

    public Task<Review?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(reviews.FirstOrDefault(review => review.Id == id));
    }

    public Task<Review?> AddReplyAsync(string id, string targetType, string targetId, ReviewReply reply, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            var index = reviews.FindIndex(review => review.Id == id && review.TargetType == targetType && review.TargetId == targetId && review.Reply is null);
            if (index < 0) return Task.FromResult<Review?>(null);
            var current = reviews[index];
            var updated = new Review { Id = current.Id, TargetType = current.TargetType, TargetId = current.TargetId, AuthorId = current.AuthorId, AuthorName = current.AuthorName, Stars = current.Stars, Text = current.Text, BookingId = current.BookingId, Reply = reply, CreatedAt = current.CreatedAt };
            reviews[index] = updated;
            return Task.FromResult<Review?>(updated);
        }
    }
}

public sealed class MongoReviewRepository(IMongoDatabase database) : IReviewRepository
{
    private readonly IMongoCollection<BsonDocument> reviews = database.GetCollection<BsonDocument>("reviews");

    public async Task<IReadOnlyList<Review>> ListAsync(string targetType, string targetId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("targetType", targetType), Builders<BsonDocument>.Filter.Eq("targetId", targetId));
        var documents = await reviews.Find(filter).SortByDescending(document => document["createdAt"]).Limit(100).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Review?> AddAsync(Review review, CancellationToken cancellationToken = default)
    {
        var duplicate = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("targetType", review.TargetType), Builders<BsonDocument>.Filter.Eq("targetId", review.TargetId), Builders<BsonDocument>.Filter.Eq("authorId", review.AuthorId));
        if (await reviews.Find(duplicate).AnyAsync(cancellationToken)) return null;
        var document = new BsonDocument { ["_id"] = review.Id, ["targetType"] = review.TargetType, ["targetId"] = review.TargetId, ["authorId"] = review.AuthorId, ["authorName"] = review.AuthorName, ["stars"] = review.Stars, ["text"] = review.Text, ["createdAt"] = new BsonDateTime(review.CreatedAt) };
        if (review.BookingId is not null) document["bookingId"] = review.BookingId;
        try { await reviews.InsertOneAsync(document, cancellationToken: cancellationToken); return review; }
        catch (MongoWriteException exception) when (exception.WriteError?.Code == 11000) { return null; }
    }

    public async Task<Review?> AddReplyAsync(string id, string targetType, string targetId, ReviewReply reply, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", id), Builders<BsonDocument>.Filter.Eq("targetType", targetType), Builders<BsonDocument>.Filter.Eq("targetId", targetId), Builders<BsonDocument>.Filter.Exists("reply", false));
        var value = new BsonDocument { ["authorId"] = reply.AuthorId, ["text"] = reply.Text, ["createdAt"] = new BsonDateTime(reply.CreatedAt) };
        var document = await reviews.FindOneAndUpdateAsync(filter, Builders<BsonDocument>.Update.Set("reply", value), new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After }, cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Review?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var document = await reviews.Find(Builders<BsonDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    private static Review Map(BsonDocument document)
    {
        var reply = document.TryGetValue("reply", out var replyValue) && replyValue.IsBsonDocument ? replyValue.AsBsonDocument : null;
        return new Review { Id = document.GetValue("_id").AsString, TargetType = document.GetValue("targetType").AsString, TargetId = document.GetValue("targetId").AsString, AuthorId = document.GetValue("authorId").AsString, AuthorName = document.GetValue("authorName").AsString, Stars = document.GetValue("stars").ToInt32(), Text = document.GetValue("text").AsString, BookingId = document.TryGetValue("bookingId", out var bookingId) ? bookingId.AsString : null, Reply = reply is null ? null : new ReviewReply { AuthorId = reply.GetValue("authorId").AsString, Text = reply.GetValue("text").AsString, CreatedAt = reply.GetValue("createdAt").ToUniversalTime() }, CreatedAt = document.GetValue("createdAt").ToUniversalTime() };
    }
}
