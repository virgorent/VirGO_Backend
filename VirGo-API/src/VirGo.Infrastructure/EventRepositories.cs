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
            Collaborators = [new("99999999-9999-4999-8999-999999999999", "Coordinador Demo", "coord@virgo.mx", "edit")],
            CreatedAt = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 11, 1, 12, 0, 0, DateTimeKind.Utc)
        }
    ];

    public Task<IReadOnlyList<VirGoEvent>> ListForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            return Task.FromResult<IReadOnlyList<VirGoEvent>>(events.Where(item => item.OwnerId == userId || item.Collaborators.Any(collaborator => collaborator.UserId == userId)).OrderBy(item => item.Date).ToArray());
        }
    }

    public Task<VirGoEvent> CreateAsync(VirGoEvent item, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events) events.Add(item);
        return Task.FromResult(item);
    }

    public Task<VirGoEvent?> FindForUserAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            return Task.FromResult(events.FirstOrDefault(item => item.Id == id && (item.OwnerId == userId || item.Collaborators.Any(collaborator => collaborator.UserId == userId))));
        }
    }

    public Task<VirGoEvent?> FindForEditAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            return Task.FromResult(events.FirstOrDefault(item => item.Id == id && (item.OwnerId == userId || item.Collaborators.Any(collaborator => collaborator.UserId == userId && collaborator.Permission == "edit"))));
        }
    }

    public Task<VirGoEvent?> UpdateForUserAsync(VirGoEvent item, string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            var index = events.FindIndex(current => current.Id == item.Id && (current.OwnerId == userId || current.Collaborators.Any(collaborator => collaborator.UserId == userId && collaborator.Permission == "edit")));
            if (index < 0) return Task.FromResult<VirGoEvent?>(null);
            events[index] = item;
            return Task.FromResult<VirGoEvent?>(item);
        }
    }

    public Task<VirGoEvent?> UpdateCollaboratorsAsync(string eventId, string ownerId, EventCollaborator[] collaborators, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (events)
        {
            var index = events.FindIndex(item => item.Id == eventId && item.OwnerId == ownerId);
            if (index < 0) return Task.FromResult<VirGoEvent?>(null);
            var current = events[index];
            var updated = new VirGoEvent { Id = current.Id, OwnerId = current.OwnerId, Name = current.Name, EventType = current.EventType, Date = current.Date, Guests = current.Guests, Budget = current.Budget, Hours = current.Hours, Venue = current.Venue, Collaborators = collaborators, CreatedAt = current.CreatedAt, UpdatedAt = DateTime.UtcNow };
            events[index] = updated;
            return Task.FromResult<VirGoEvent?>(updated);
        }
    }
}

public sealed class MongoEventRepository(IMongoDatabase database) : IEventRepository
{
    private readonly IMongoCollection<BsonDocument> events = database.GetCollection<BsonDocument>("events");

    public async Task<IReadOnlyList<VirGoEvent>> ListForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.Or(
            Builders<BsonDocument>.Filter.Eq("ownerId", userId),
            Builders<BsonDocument>.Filter.Eq("collaborators.userId", userId));
        var documents = await events.Find(filter).SortBy(document => document["date"]).ToListAsync(cancellationToken);
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
        if (item.Venue is not null)
        {
            document["venue"] = new BsonDocument
            {
                ["venueId"] = item.Venue.VenueId,
                ["name"] = item.Venue.Name,
                ["photoUrl"] = item.Venue.PhotoUrl
            };
        }
        await events.InsertOneAsync(document, cancellationToken: cancellationToken);
        return item;
    }

    public async Task<VirGoEvent?> FindForUserAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", id),
            Builders<BsonDocument>.Filter.Or(
                Builders<BsonDocument>.Filter.Eq("ownerId", userId),
                Builders<BsonDocument>.Filter.Eq("collaborators.userId", userId)));
        var document = await events.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<VirGoEvent?> FindForEditAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", id),
            Builders<BsonDocument>.Filter.Or(
                Builders<BsonDocument>.Filter.Eq("ownerId", userId),
                new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument
                {
                    ["collaborators"] = new BsonDocument("$elemMatch", new BsonDocument { ["userId"] = userId, ["permission"] = "edit" })
                })));
        var document = await events.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<VirGoEvent?> UpdateForUserAsync(VirGoEvent item, string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", item.Id),
            Builders<BsonDocument>.Filter.Or(
                Builders<BsonDocument>.Filter.Eq("ownerId", userId),
                new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument
                {
                    ["collaborators"] = new BsonDocument("$elemMatch", new BsonDocument { ["userId"] = userId, ["permission"] = "edit" })
                })));
        var update = Builders<BsonDocument>.Update
            .Set("name", item.Name)
            .Set("eventType", item.EventType)
            .Set("date", item.Date)
            .Set("guests", item.Guests)
            .Set("budget", new BsonDecimal128(item.Budget))
            .Set("hours", item.Hours)
            .Set("updatedAt", new BsonDateTime(item.UpdatedAt));
        update = item.Venue is null
            ? update.Unset("venue")
            : update.Set("venue", new BsonDocument { ["venueId"] = item.Venue.VenueId, ["name"] = item.Venue.Name, ["photoUrl"] = item.Venue.PhotoUrl });
        var result = await events.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.MatchedCount == 0 ? null : item;
    }

    public async Task<VirGoEvent?> UpdateCollaboratorsAsync(string eventId, string ownerId, EventCollaborator[] collaborators, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", eventId), Builders<BsonDocument>.Filter.Eq("ownerId", ownerId));
        var value = new BsonArray(collaborators.Select(collaborator => new BsonDocument { ["userId"] = collaborator.UserId, ["name"] = collaborator.Name, ["email"] = collaborator.Email, ["permission"] = collaborator.Permission }));
        var result = await events.UpdateOneAsync(filter, Builders<BsonDocument>.Update.Set("collaborators", value).Set("updatedAt", new BsonDateTime(DateTime.UtcNow)), cancellationToken: cancellationToken);
        if (result.MatchedCount == 0) return null;
        var document = await events.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

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
            ? new EventVenue(venue["venueId"].AsString, venue["name"].AsString, venue["photoUrl"].AsString)
            : null,
        Collaborators = document.TryGetValue("collaborators", out var collaborators) && collaborators.IsBsonArray
            ? collaborators.AsBsonArray.Select(value =>
            {
                var item = value.AsBsonDocument;
                return new EventCollaborator(item["userId"].AsString, item["name"].AsString, item["email"].AsString, item["permission"].AsString);
            }).ToArray()
            : [],
        CreatedAt = document["createdAt"].ToUniversalTime(),
        UpdatedAt = document["updatedAt"].ToUniversalTime()
    };
}
