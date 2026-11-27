using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Events;
using VirGo.Domain.Events;

namespace VirGo.Infrastructure;

public sealed class InMemoryEventRepository : IEventRepository
{
    private readonly List<VirGoEvent> events =
    [
        new VirGoEvent
        {
            Id = "eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee",
            OwnerId = "8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1",
            Name = "Boda Demo",
            EventType = "Boda",
            Date = "2027-08-21",
            Guests = 120,
            Budget = 85000m,
            Hours = 8,
            Venue = new("11111111-1111-4111-8111-111111111111", "VirGo Demo Venue", ""),
            CreatedAt = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc)
        }
    ];

    public Task<IReadOnlyList<VirGoEvent>> ListForOwnerAsync(string ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            return Task.FromResult<IReadOnlyList<VirGoEvent>>(events.Where(item => item.OwnerId == ownerId).OrderBy(item => item.Date, StringComparer.Ordinal).ToArray());
        }
    }

    public Task<VirGoEvent> CreateAsync(VirGoEvent item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events) events.Add(item);
        return Task.FromResult(item);
    }

    public Task<VirGoEvent?> FindForOwnerAsync(string id, string ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            return Task.FromResult(events.FirstOrDefault(item => item.Id == id && item.OwnerId == ownerId));
        }
    }

    public Task<VirGoEvent?> UpdateForOwnerAsync(VirGoEvent item, string ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            var index = events.FindIndex(current => current.Id == item.Id && current.OwnerId == ownerId);
            if (index < 0) return Task.FromResult<VirGoEvent?>(null);
            events[index] = item;
            return Task.FromResult<VirGoEvent?>(item);
        }
    }
}

public sealed class MongoEventRepository(IMongoDatabase database) : IEventRepository
{
    private readonly IMongoCollection<BsonDocument> events = database.GetCollection<BsonDocument>("events");

    public async Task<IReadOnlyList<VirGoEvent>> ListForOwnerAsync(string ownerId, CancellationToken cancellationToken = default)
    {
        var documents = await events.Find(Builders<BsonDocument>.Filter.Eq("ownerId", ownerId)).SortBy(document => document["date"]).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<VirGoEvent> CreateAsync(VirGoEvent item, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
        {
            ["_id"] = item.Id,
            ["ownerId"] = item.OwnerId,
            ["name"] = item.Name,
            ["eventType"] = item.EventType,
            ["date"] = item.Date,
            ["guests"] = item.Guests,
            ["budget"] = new BsonDecimal128(item.Budget),
            ["hours"] = item.Hours,
            ["createdAt"] = new BsonDateTime(item.CreatedAt),
            ["updatedAt"] = new BsonDateTime(item.UpdatedAt)
        };
        if (item.Venue is not null) document["venue"] = ToDocument(item.Venue);
        await events.InsertOneAsync(document, cancellationToken: cancellationToken);
        return item;
    }

    public async Task<VirGoEvent?> FindForOwnerAsync(string id, string ownerId, CancellationToken cancellationToken = default)
    {
        var document = await events.Find(OwnerFilter(id, ownerId)).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<VirGoEvent?> UpdateForOwnerAsync(VirGoEvent item, string ownerId, CancellationToken cancellationToken = default)
    {
        var update = Builders<BsonDocument>.Update
            .Set("name", item.Name)
            .Set("eventType", item.EventType)
            .Set("date", item.Date)
            .Set("guests", item.Guests)
            .Set("budget", new BsonDecimal128(item.Budget))
            .Set("hours", item.Hours)
            .Set("updatedAt", new BsonDateTime(item.UpdatedAt));
        update = item.Venue is null ? update.Unset("venue") : update.Set("venue", ToDocument(item.Venue));
        var result = await events.UpdateOneAsync(OwnerFilter(item.Id, ownerId), update, cancellationToken: cancellationToken);
        return result.MatchedCount == 0 ? null : item;
    }

    private static FilterDefinition<BsonDocument> OwnerFilter(string id, string ownerId) => Builders<BsonDocument>.Filter.And(
        Builders<BsonDocument>.Filter.Eq("_id", id),
        Builders<BsonDocument>.Filter.Eq("ownerId", ownerId));

    private static BsonDocument ToDocument(EventVenue venue) => new()
    {
        ["venueId"] = venue.VenueId,
        ["name"] = venue.Name,
        ["photoUrl"] = venue.PhotoUrl
    };

    private static VirGoEvent Map(BsonDocument document) => new()
    {
        Id = document["_id"].AsString,
        OwnerId = document["ownerId"].AsString,
        Name = document["name"].AsString,
        EventType = document["eventType"].AsString,
        Date = document["date"].AsString,
        Guests = document["guests"].AsInt32,
        Budget = document["budget"].ToDecimal(),
        Hours = document["hours"].AsInt32,
        Venue = document.TryGetValue("venue", out var venue) && venue.IsBsonDocument
            ? new EventVenue(venue["venueId"].AsString, venue["name"].AsString, venue.AsBsonDocument.TryGetValue("photoUrl", out var photo) && photo.IsString ? photo.AsString : string.Empty)
            : null,
        CreatedAt = document["createdAt"].ToUniversalTime(),
        UpdatedAt = document["updatedAt"].ToUniversalTime()
    };
}
