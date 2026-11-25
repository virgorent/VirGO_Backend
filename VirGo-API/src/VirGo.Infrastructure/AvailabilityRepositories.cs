using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Marketplace;

namespace VirGo.Infrastructure;

public sealed class InMemoryVenueAvailabilityRepository : IVenueAvailabilityRepository
{
    private readonly List<(string Id, VenueDateRecord Record)> dates =
    [
        ("sample-block", new(
            "11111111-1111-4111-8111-111111111111",
            new DateOnly(2027, 5, 20),
            "blocked",
            "maintenance",
            "Nota interna no pública",
            null))
    ];

    public Task<IReadOnlyList<VenueDateRecord>> ListAsync(
        string venueId,
        DateOnly from,
        DateOnly toDate,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<VenueDateRecord>>(
            dates.Where(date => date.Record.VenueId == venueId && date.Record.Date >= from && date.Record.Date <= toDate).Select(date => date.Record with { Id = date.Id }).ToArray());
    }

    public Task<VenueDateRecord?> AddBlockAsync(string venueId, VenueBlockRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (dates)
        {
            if (dates.Any(date => date.Record.VenueId == venueId && date.Record.Date == request.Date)) return Task.FromResult<VenueDateRecord?>(null);
            var id = Guid.NewGuid().ToString("D");
            var record = new VenueDateRecord(venueId, request.Date, "blocked", request.Reason, request.Note, null) { Id = id };
            dates.Add((id, record));
            return Task.FromResult<VenueDateRecord?>(record);
        }
    }

    public Task<bool> DeleteBlockAsync(string venueId, string blockId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (dates) return Task.FromResult(dates.RemoveAll(date => date.Id == blockId && date.Record.VenueId == venueId && date.Record.Kind == "blocked") > 0);
    }
}

public sealed class MongoVenueAvailabilityRepository(IMongoDatabase database) : IVenueAvailabilityRepository
{
    private readonly IMongoCollection<BsonDocument> dates = database.GetCollection<BsonDocument>("venueDates");

    public async Task<IReadOnlyList<VenueDateRecord>> ListAsync(
        string venueId,
        DateOnly from,
        DateOnly toDate,
        CancellationToken cancellationToken = default)
    {
        var fromValue = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toValue = toDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("venueId", venueId),
            Builders<BsonDocument>.Filter.Gte("date", fromValue),
            Builders<BsonDocument>.Filter.Lte("date", toValue));
        var documents = await dates.Find(filter).SortBy(document => document["date"]).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<VenueDateRecord?> AddBlockAsync(string venueId, VenueBlockRequest request, CancellationToken cancellationToken = default)
    {
        var date = request.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var duplicate = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("venueId", venueId), Builders<BsonDocument>.Filter.Eq("date", date));
        if (await dates.Find(duplicate).AnyAsync(cancellationToken)) return null;
        var document = new BsonDocument { ["_id"] = Guid.NewGuid().ToString("D"), ["venueId"] = venueId, ["date"] = date, ["kind"] = "blocked" };
        if (request.Reason is not null) document["reason"] = request.Reason;
        if (request.Note is not null) document["note"] = request.Note;
        await dates.InsertOneAsync(document, cancellationToken: cancellationToken);
        return Map(document);
    }

    public async Task<bool> DeleteBlockAsync(string venueId, string blockId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", blockId), Builders<BsonDocument>.Filter.Eq("venueId", venueId), Builders<BsonDocument>.Filter.Eq("kind", "blocked"));
        return (await dates.DeleteOneAsync(filter, cancellationToken)).DeletedCount > 0;
    }

    private static VenueDateRecord Map(BsonDocument document) => new(
        document["venueId"].AsString,
        DateOnly.ParseExact(document["date"].AsString, "yyyy-MM-dd", CultureInfo.InvariantCulture),
        document["kind"].AsString,
        document.TryGetValue("reason", out var reason) ? reason.AsString : null,
        document.TryGetValue("note", out var note) ? note.AsString : null,
        document.TryGetValue("bookingId", out var bookingId) ? bookingId.AsString : null)
    { Id = document.GetValue("_id").AsString };
}
