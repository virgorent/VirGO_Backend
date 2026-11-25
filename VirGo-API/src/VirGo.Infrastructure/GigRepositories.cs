using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Providers;
using VirGo.Domain.Gigs;

namespace VirGo.Infrastructure;

public sealed class InMemoryGigRepository : IGigRepository
{
    private readonly List<Gig> gigs =
    [
        new Gig
        {
            Id = "cccccccc-cccc-4ccc-8ccc-cccccccccccc",
            ProviderId = "aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa",
            EventName = "Boda Demo",
            ClientName = "Cliente Demo",
            Place = "Centro",
            Date = "2027-05-15",
            Start = "18:00",
            End = "23:00",
            Amount = 12500m,
            QuoteId = "dddddddd-dddd-4ddd-8ddd-dddddddddddd"
        }
    ];

    public Task<IReadOnlyList<Gig>> ListByProviderAsync(string providerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gigs)
        {
            return Task.FromResult<IReadOnlyList<Gig>>(gigs.Where(gig => gig.ProviderId == providerId).OrderBy(gig => gig.Date).ThenBy(gig => gig.Start).ToArray());
        }
    }

    public Task<Gig?> CreateAsync(Gig gig, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gigs)
        {
            if (gigs.Any(existing => existing.ProviderId == gig.ProviderId && existing.Date == gig.Date && string.Compare(existing.Start, gig.End, StringComparison.Ordinal) < 0 && string.Compare(existing.End, gig.Start, StringComparison.Ordinal) > 0))
            {
                return Task.FromResult<Gig?>(null);
            }

            gigs.Add(gig);
            return Task.FromResult<Gig?>(gig);
        }
    }

    public Task<Gig?> UpdateAsync(Gig gig, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gigs)
        {
            if (gigs.Any(existing => existing.Id != gig.Id && existing.ProviderId == gig.ProviderId && existing.Date == gig.Date && string.Compare(existing.Start, gig.End, StringComparison.Ordinal) < 0 && string.Compare(existing.End, gig.Start, StringComparison.Ordinal) > 0)) return Task.FromResult<Gig?>(null);
            var index = gigs.FindIndex(existing => existing.Id == gig.Id && existing.ProviderId == gig.ProviderId);
            if (index < 0) return Task.FromResult<Gig?>(null);
            gigs[index] = gig;
            return Task.FromResult<Gig?>(gig);
        }
    }
}

public sealed class MongoGigRepository(IMongoDatabase database) : IGigRepository
{
    private readonly IMongoCollection<BsonDocument> gigs = database.GetCollection<BsonDocument>("gigs");

    public async Task<IReadOnlyList<Gig>> ListByProviderAsync(string providerId, CancellationToken cancellationToken = default)
    {
        var documents = await gigs.Find(Builders<BsonDocument>.Filter.Eq("providerId", providerId))
            .SortBy(document => document["date"])
            .ThenBy(document => document["start"])
            .ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Gig?> CreateAsync(Gig gig, CancellationToken cancellationToken = default)
    {
        var overlap = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("providerId", gig.ProviderId),
            Builders<BsonDocument>.Filter.Eq("date", gig.Date),
            Builders<BsonDocument>.Filter.Lt("start", gig.End),
            Builders<BsonDocument>.Filter.Gt("end", gig.Start));
        if (await gigs.Find(overlap).AnyAsync(cancellationToken)) return null;

        var document = new BsonDocument
        {
            ["_id"] = gig.Id,
            ["providerId"] = gig.ProviderId,
            ["eventName"] = gig.EventName,
            ["clientName"] = gig.ClientName,
            ["place"] = gig.Place,
            ["date"] = gig.Date,
            ["start"] = gig.Start,
            ["end"] = gig.End
        };
        if (gig.Amount is not null) document["amount"] = new BsonDecimal128(gig.Amount.Value);
        if (gig.QuoteId is not null) document["quoteId"] = gig.QuoteId;
        await gigs.InsertOneAsync(document, cancellationToken: cancellationToken);
        return gig;
    }

    public async Task<Gig?> UpdateAsync(Gig gig, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("providerId", gig.ProviderId),
            Builders<BsonDocument>.Filter.Eq("date", gig.Date),
            Builders<BsonDocument>.Filter.Lt("start", gig.End),
            Builders<BsonDocument>.Filter.Gt("end", gig.Start),
            Builders<BsonDocument>.Filter.Ne("_id", gig.Id));
        if (await gigs.Find(filter).AnyAsync(cancellationToken)) return null;

        var target = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", gig.Id),
            Builders<BsonDocument>.Filter.Eq("providerId", gig.ProviderId));
        var update = Builders<BsonDocument>.Update
            .Set("eventName", gig.EventName)
            .Set("clientName", gig.ClientName)
            .Set("place", gig.Place)
            .Set("date", gig.Date)
            .Set("start", gig.Start)
            .Set("end", gig.End);
        update = gig.Amount is null ? update.Unset("amount") : update.Set("amount", new BsonDecimal128(gig.Amount.Value));
        update = gig.QuoteId is null ? update.Unset("quoteId") : update.Set("quoteId", gig.QuoteId);
        var result = await gigs.UpdateOneAsync(target, update, cancellationToken: cancellationToken);
        return result.MatchedCount == 0 ? null : gig;
    }

    private static Gig Map(BsonDocument document) => new()
    {
        Id = document["_id"].AsString,
        ProviderId = document["providerId"].AsString,
        EventName = document["eventName"].AsString,
        ClientName = document["clientName"].AsString,
        Place = document["place"].AsString,
        Date = document["date"].AsString,
        Start = document["start"].AsString,
        End = document["end"].AsString,
        Amount = document.TryGetValue("amount", out var amount) && amount.IsDecimal128 ? amount.ToDecimal() : null,
        QuoteId = document.TryGetValue("quoteId", out var quoteId) && quoteId.IsString ? quoteId.AsString : null
    };
}
