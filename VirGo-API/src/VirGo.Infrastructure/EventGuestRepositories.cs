using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Events;
using VirGo.Domain.Events;

namespace VirGo.Infrastructure;

public sealed class InMemoryEventGuestRepository : IEventGuestRepository
{
    private readonly List<EventGuest> guests =
    [
        new EventGuest { Id = "f1111111-1111-4111-8111-111111111111", EventId = "eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee", Name = "Invitado Demo", Phone = "+524420000001", Companions = 1, Status = "confirmed", Table = "Mesa 1" }
    ];

    public Task<IReadOnlyList<EventGuest>> ListAsync(string eventId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (guests) return Task.FromResult<IReadOnlyList<EventGuest>>(guests.Where(guest => guest.EventId == eventId).ToArray());
    }

    public Task<EventGuest> CreateAsync(EventGuest guest, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (guests) guests.Add(guest);
        return Task.FromResult(guest);
    }

    public Task<EventGuest?> UpdateAsync(EventGuest guest, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (guests)
        {
            var index = guests.FindIndex(current => current.Id == guest.Id && current.EventId == guest.EventId);
            if (index < 0) return Task.FromResult<EventGuest?>(null);
            guests[index] = guest;
            return Task.FromResult<EventGuest?>(guest);
        }
    }

    public Task<bool> DeleteAsync(string eventId, string guestId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (guests)
        {
            var removed = guests.RemoveAll(current => current.EventId == eventId && current.Id == guestId);
            return Task.FromResult(removed == 1);
        }
    }
}

public sealed class MongoEventGuestRepository(IMongoDatabase database) : IEventGuestRepository
{
    private readonly IMongoCollection<BsonDocument> guests = database.GetCollection<BsonDocument>("eventGuests");

    public async Task<IReadOnlyList<EventGuest>> ListAsync(string eventId, CancellationToken cancellationToken = default)
    {
        var documents = await guests.Find(Builders<BsonDocument>.Filter.Eq("eventId", eventId)).ToListAsync(cancellationToken);
        return documents.Select(document => new EventGuest
        {
            Id = document["_id"].AsString,
            EventId = document["eventId"].AsString,
            Name = document["name"].AsString,
            Phone = document["phone"].AsString,
            Companions = document["companions"].AsInt32,
            Status = document["status"].AsString,
            Table = document.TryGetValue("table", out var table) && table.IsString ? table.AsString : null
        }).ToArray();
    }

    public async Task<EventGuest> CreateAsync(EventGuest guest, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
        {
            ["_id"] = guest.Id,
            ["eventId"] = guest.EventId,
            ["name"] = guest.Name,
            ["phone"] = guest.Phone,
            ["companions"] = guest.Companions,
            ["status"] = guest.Status
        };
        if (guest.Table is not null) document["table"] = guest.Table;
        await guests.InsertOneAsync(document, cancellationToken: cancellationToken);
        return guest;
    }

    public async Task<EventGuest?> UpdateAsync(EventGuest guest, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", guest.Id), Builders<BsonDocument>.Filter.Eq("eventId", guest.EventId));
        var update = Builders<BsonDocument>.Update.Set("name", guest.Name).Set("phone", guest.Phone).Set("companions", guest.Companions).Set("status", guest.Status);
        update = guest.Table is null ? update.Unset("table") : update.Set("table", guest.Table);
        var result = await guests.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.MatchedCount == 0 ? null : guest;
    }

    public async Task<bool> DeleteAsync(string eventId, string guestId, CancellationToken cancellationToken = default)
    {
        var result = await guests.DeleteOneAsync(Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("eventId", eventId), Builders<BsonDocument>.Filter.Eq("_id", guestId)), cancellationToken);
        return result.DeletedCount == 1;
    }
}
