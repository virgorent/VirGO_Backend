using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure;

public sealed class InMemoryFavoriteRepository : IFavoriteRepository
{
    private readonly List<Favorite> favorites = [];
    private readonly Lock gate = new();
    public Task<IReadOnlyList<Favorite>> ListAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult<IReadOnlyList<Favorite>>(favorites.Where(favorite => favorite.UserId == userId).OrderByDescending(favorite => favorite.CreatedAt).ToArray());
    }
    public Task<Favorite?> AddAsync(Favorite favorite, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            if (favorites.Any(item => item.UserId == favorite.UserId && item.TargetType == favorite.TargetType && item.TargetId == favorite.TargetId)) return Task.FromResult<Favorite?>(null);
            favorites.Add(favorite);
            return Task.FromResult<Favorite?>(favorite);
        }
    }
    public Task<bool> DeleteAsync(string userId, string targetType, string targetId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(favorites.RemoveAll(favorite => favorite.UserId == userId && favorite.TargetType == targetType && favorite.TargetId == targetId) > 0);
    }
}

public sealed class MongoFavoriteRepository(IMongoDatabase database) : IFavoriteRepository
{
    private readonly IMongoCollection<BsonDocument> favorites = database.GetCollection<BsonDocument>("favorites");
    public async Task<IReadOnlyList<Favorite>> ListAsync(string userId, CancellationToken cancellationToken = default)
    {
        var documents = await favorites.Find(Builders<BsonDocument>.Filter.Eq("userId", userId)).SortByDescending(document => document["createdAt"]).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }
    public async Task<Favorite?> AddAsync(Favorite favorite, CancellationToken cancellationToken = default)
    {
        var duplicate = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("userId", favorite.UserId), Builders<BsonDocument>.Filter.Eq("targetType", favorite.TargetType), Builders<BsonDocument>.Filter.Eq("targetId", favorite.TargetId));
        if (await favorites.Find(duplicate).AnyAsync(cancellationToken)) return null;
        var document = new BsonDocument { ["_id"] = favorite.Id, ["userId"] = favorite.UserId, ["targetType"] = favorite.TargetType, ["targetId"] = favorite.TargetId, ["name"] = favorite.Name, ["photoUrl"] = favorite.PhotoUrl ?? string.Empty, ["rating"] = new BsonDecimal128(favorite.Rating), ["createdAt"] = new BsonDateTime(favorite.CreatedAt) };
        await favorites.InsertOneAsync(document, cancellationToken: cancellationToken);
        return favorite;
    }
    public async Task<bool> DeleteAsync(string userId, string targetType, string targetId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("userId", userId), Builders<BsonDocument>.Filter.Eq("targetType", targetType), Builders<BsonDocument>.Filter.Eq("targetId", targetId));
        return (await favorites.DeleteOneAsync(filter, cancellationToken)).DeletedCount > 0;
    }
    private static Favorite Map(BsonDocument document) => new() { Id = document.GetValue("_id").AsString, UserId = document.GetValue("userId").AsString, TargetType = document.GetValue("targetType").AsString, TargetId = document.GetValue("targetId").AsString, Name = document.GetValue("name").AsString, PhotoUrl = document.GetValue("photoUrl", "").AsString, Rating = document.GetValue("rating", 0m).ToDecimal(), CreatedAt = document.GetValue("createdAt").ToUniversalTime() };
}
