using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure;

public sealed class InMemoryVenueRepository : IVenueRepository
{
    private readonly Venue venue = new()
    {
        Id = "11111111-1111-4111-8111-111111111111",
        OwnerId = "22222222-2222-4222-8222-222222222222",
        Name = "VirGo Demo Venue",
        ShortName = "Demo Venue",
        Description = "Venue de demostración para el flujo inicial de VirGo.",
        Address = "Centro, Querétaro",
        Zone = "Centro",
        Capacity = 150,
        BasePrice = 45000m,
        PriceUnit = "day",
        Categories = ["Elegante"],
        Amenities = ["Parking"],
        Photos = [],
        Status = "published",
        RatingAverage = 5m,
        RatingCount = 1
    };

    public Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var matches = string.IsNullOrWhiteSpace(zone) || string.Equals(zone, venue.Zone, StringComparison.OrdinalIgnoreCase);
        matches &= string.IsNullOrWhiteSpace(category) || venue.Categories.Contains(category, StringComparer.OrdinalIgnoreCase);
        return Task.FromResult<IReadOnlyList<Venue>>(matches ? [venue] : []);
    }

    public Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<Venue?>(venue.Id == id ? venue : null);
    }
}

public sealed class InMemoryQuoteRepository : IQuoteRepository
{
    private readonly List<Quote> quotes = [];
    private readonly Lock gate = new();

    public Task AddAsync(Quote quote, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            quotes.Add(quote);
        }

        return Task.CompletedTask;
    }
}

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<Booking> bookings = [];
    private readonly Dictionary<int, int> folios = [];
    private readonly Lock gate = new();

    public Task<string> NextFolioAsync(int year, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            folios[year] = folios.GetValueOrDefault(year) + 1;
            return Task.FromResult($"VG-{year:D4}-{folios[year]:D4}");
        }
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            bookings.Add(booking);
        }

        return Task.CompletedTask;
    }
}

public sealed class MongoVenueRepository(IMongoDatabase database) : IVenueRepository
{
    private readonly IMongoCollection<BsonDocument> venues = database.GetCollection<BsonDocument>("venues");

    public async Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<BsonDocument>>
        {
            Builders<BsonDocument>.Filter.Eq("status", "published")
        };
        if (!string.IsNullOrWhiteSpace(zone))
        {
            filters.Add(Builders<BsonDocument>.Filter.Eq("zone", zone));
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            filters.Add(Builders<BsonDocument>.Filter.AnyEq("categories", category));
        }

        var documents = await venues.Find(Builders<BsonDocument>.Filter.And(filters)).Limit(100).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", id),
            Builders<BsonDocument>.Filter.Eq("status", "published"));
        var document = await venues.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    private static Venue Map(BsonDocument document) => new()
    {
        Id = document.GetValue("_id").AsString,
        OwnerId = document.GetValue("ownerId").AsString,
        Name = document.GetValue("name").AsString,
        ShortName = document.GetValue("shortName").AsString,
        Description = document.GetValue("description").AsString,
        Address = document.GetValue("address").AsString,
        Zone = document.GetValue("zone").AsString,
        Capacity = document.GetValue("capacity").ToInt32(),
        BasePrice = document["price"].AsBsonDocument["base"].ToDecimal(),
        PriceUnit = document["price"].AsBsonDocument["unit"].AsString,
        Categories = document["categories"].AsBsonArray.Select(value => value.AsString).ToArray(),
        Amenities = document["amenities"].AsBsonArray.Select(value => value.AsString).ToArray(),
        Photos = document["photos"].AsBsonArray.Select(value => value.AsBsonDocument["url"].AsString).ToArray(),
        Status = document.GetValue("status").AsString,
        RatingAverage = document["rating"].AsBsonDocument["average"].ToDecimal(),
        RatingCount = document["rating"].AsBsonDocument["count"].ToInt32()
    };
}

public sealed class MongoQuoteRepository(IMongoDatabase database) : IQuoteRepository
{
    private readonly IMongoCollection<BsonDocument> quotes = database.GetCollection<BsonDocument>("quotes");

    public Task AddAsync(Quote quote, CancellationToken cancellationToken = default) =>
        quotes.InsertOneAsync(new BsonDocument
        {
            ["_id"] = quote.Id,
            ["clientId"] = quote.ClientId,
            ["clientName"] = quote.ClientName,
            ["target"] = new BsonDocument
            {
                ["type"] = quote.TargetType,
                ["id"] = quote.TargetId,
                ["name"] = quote.TargetName
            },
            ["eventId"] = quote.EventId is null ? BsonNull.Value : quote.EventId,
            ["eventType"] = quote.EventType,
            ["eventDate"] = quote.EventDate,
            ["guests"] = quote.Guests,
            ["requestedServices"] = new BsonArray(quote.RequestedServices),
            ["message"] = quote.Message is null ? BsonNull.Value : quote.Message,
            ["status"] = quote.Status,
            ["createdAt"] = new BsonDateTime(quote.CreatedAt),
            ["updatedAt"] = new BsonDateTime(quote.UpdatedAt)
        }, cancellationToken: cancellationToken);
}

public sealed class MongoBookingRepository(IMongoDatabase database) : IBookingRepository
{
    private readonly IMongoCollection<BsonDocument> bookings = database.GetCollection<BsonDocument>("bookings");
    private readonly IMongoCollection<BsonDocument> counters = database.GetCollection<BsonDocument>("counters");

    public async Task<string> NextFolioAsync(int year, CancellationToken cancellationToken = default)
    {
        var counterId = $"folio-{year:D4}";
        var filter = Builders<BsonDocument>.Filter.Eq("_id", counterId);
        var update = Builders<BsonDocument>.Update
            .SetOnInsert("next", 0)
            .Inc("next", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };
        var counter = await counters.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
        return $"VG-{year:D4}-{counter["next"].ToInt32():D4}";
    }

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default) =>
        bookings.InsertOneAsync(new BsonDocument
        {
            ["_id"] = booking.Id,
            ["folio"] = booking.Folio,
            ["clientId"] = booking.ClientId,
            ["clientName"] = booking.ClientName,
            ["quoteId"] = booking.QuoteId is null ? BsonNull.Value : booking.QuoteId,
            ["eventId"] = booking.EventId is null ? BsonNull.Value : booking.EventId,
            ["venue"] = new BsonDocument
            {
                ["venueId"] = booking.VenueId,
                ["name"] = booking.VenueName,
                ["photoUrl"] = string.Empty
            },
            ["date"] = booking.Date,
            ["hours"] = booking.Hours,
            ["total"] = new Decimal128(booking.Total),
            ["deposit"] = new Decimal128(booking.Deposit),
            ["status"] = booking.Status,
            ["payments"] = new BsonArray(),
            ["createdAt"] = new BsonDateTime(booking.CreatedAt),
            ["updatedAt"] = new BsonDateTime(booking.UpdatedAt)
        }, cancellationToken: cancellationToken);
}
