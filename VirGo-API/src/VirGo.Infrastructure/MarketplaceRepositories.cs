using System.Globalization;
using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Jobs;
using VirGo.Application.Marketplace;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure;

public sealed class InMemoryVenueRepository : IVenueRepository
{
    private readonly List<Venue> venues =
    [
        new Venue
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
            MinimumHours = 1,
            Categories = ["Salon"],
            Amenities = ["Estacionamiento"],
            Photos = [],
            Status = "published",
            RatingAverage = 5m,
            RatingCount = 1
        }
    ];

    public Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = venues.Where(venue =>
            (string.IsNullOrWhiteSpace(zone) || string.Equals(zone, venue.Zone, StringComparison.OrdinalIgnoreCase))
            && (string.IsNullOrWhiteSpace(category) || venue.Categories.Contains(category, StringComparer.OrdinalIgnoreCase))
            && venue.Status == "published").ToArray();
        return Task.FromResult<IReadOnlyList<Venue>>(result);
    }

    public Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<Venue?>(venues.FirstOrDefault(venue => venue.Id == id && venue.Status == "published"));
    }

    public Task<IReadOnlyList<Venue>> ListOwnedAsync(string ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<Venue>>(venues.Where(venue => venue.OwnerId == ownerId).ToArray());
    }

    public Task<Venue> CreateAsync(CreateVenueCommand command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var venue = ToVenue(command, Guid.NewGuid().ToString("D"));
        venues.Add(venue);
        return Task.FromResult(venue);
    }

    public Task<Venue?> UpdateAsync(PatchVenueCommand command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var index = venues.FindIndex(venue => venue.Id == command.VenueId && venue.OwnerId == command.OwnerId);
        if (index < 0) return Task.FromResult<Venue?>(null);

        var current = venues[index];
        var updated = new Venue
        {
            Id = current.Id,
            OwnerId = current.OwnerId,
            Name = command.Name ?? current.Name,
            ShortName = command.ShortName ?? current.ShortName,
            Description = command.Description ?? current.Description,
            Address = command.Address ?? current.Address,
            Zone = command.Zone ?? current.Zone,
            Capacity = command.Capacity ?? current.Capacity,
            BasePrice = command.BasePrice ?? current.BasePrice,
            PriceUnit = command.PriceUnit ?? current.PriceUnit,
            MinimumHours = command.MinimumHours ?? current.MinimumHours,
            Categories = command.Categories ?? current.Categories,
            Amenities = command.Amenities ?? current.Amenities,
            Photos = command.Photos?.Select(photo => photo.Url).ToArray() ?? current.Photos,
            Status = current.Status,
            RatingAverage = current.RatingAverage,
            RatingCount = current.RatingCount
        };
        venues[index] = updated;
        return Task.FromResult<Venue?>(updated);
    }

    public Task<Venue?> PublishAsync(string venueId, string ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var index = venues.FindIndex(venue => venue.Id == venueId && venue.OwnerId == ownerId);
        if (index < 0) return Task.FromResult<Venue?>(null);

        var current = venues[index];
        var published = new Venue
        {
            Id = current.Id,
            OwnerId = current.OwnerId,
            Name = current.Name,
            ShortName = current.ShortName,
            Description = current.Description,
            Address = current.Address,
            Zone = current.Zone,
            Capacity = current.Capacity,
            BasePrice = current.BasePrice,
            PriceUnit = current.PriceUnit,
            MinimumHours = current.MinimumHours,
            Categories = current.Categories,
            Amenities = current.Amenities,
            Photos = current.Photos,
            Status = "published",
            RatingAverage = current.RatingAverage,
            RatingCount = current.RatingCount
        };
        venues[index] = published;
        return Task.FromResult<Venue?>(published);
    }

    private static Venue ToVenue(CreateVenueCommand command, string id) => new()
    {
        Id = id,
        OwnerId = command.OwnerId,
        Name = command.Name,
        ShortName = command.ShortName,
        Description = command.Description,
        Address = command.Address,
        Zone = command.Zone,
        Capacity = command.Capacity,
        BasePrice = command.BasePrice,
        PriceUnit = command.PriceUnit,
        MinimumHours = command.MinimumHours,
        Categories = command.Categories,
        Amenities = command.Amenities,
        Photos = command.Photos.Select(photo => photo.Url).ToArray(),
        Status = "draft",
        RatingAverage = 0m,
        RatingCount = 0
    };
}

public sealed class InMemoryQuoteRepository : IQuoteRepository, IQuoteExpiry
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

    public Task<IReadOnlyList<Quote>> ListVisibleAsync(string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            var visible = quotes.Where(quote => quote.ClientId == clientId || targetIds.Contains(quote.TargetId, StringComparer.Ordinal)).OrderByDescending(quote => quote.CreatedAt).Take(100).ToArray();
            return Task.FromResult<IReadOnlyList<Quote>>(visible);
        }
    }

    public Task<Quote?> FindVisibleAsync(string id, string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(quotes.FirstOrDefault(quote => quote.Id == id && (quote.ClientId == clientId || targetIds.Contains(quote.TargetId, StringComparer.Ordinal))));
    }

    public Task<Quote?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(quotes.FirstOrDefault(quote => quote.Id == id));
    }

    public Task<Quote?> UpdateAsync(Quote quote, string expectedStatus, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            var index = quotes.FindIndex(item => item.Id == quote.Id && item.Status == expectedStatus);
            if (index < 0) return Task.FromResult<Quote?>(null);
            quotes[index] = quote;
            return Task.FromResult<Quote?>(quote);
        }
    }

    public IReadOnlyList<Quote> Snapshot()
    {
        lock (gate)
        {
            return quotes.ToArray();
        }
    }

    public Task<long> ExpireBeforeAsync(DateOnly today, DateTime now, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cutoff = today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        long expired = 0;
        lock (gate)
        {
            for (var index = 0; index < quotes.Count; index++)
            {
                var quote = quotes[index];
                if (quote.Status is "sent" or "answered" && string.CompareOrdinal(quote.EventDate, cutoff) < 0)
                {
                    quotes[index] = new Quote
                    {
                        Id = quote.Id,
                        ClientId = quote.ClientId,
                        ClientName = quote.ClientName,
                        TargetType = quote.TargetType,
                        TargetId = quote.TargetId,
                        TargetName = quote.TargetName,
                        EventId = quote.EventId,
                        EventType = quote.EventType,
                        EventDate = quote.EventDate,
                        Guests = quote.Guests,
                        RequestedServices = quote.RequestedServices,
                        Message = quote.Message,
                        Response = quote.Response,
                        Status = "expired",
                        CreatedAt = quote.CreatedAt,
                        UpdatedAt = now
                    };
                    expired++;
                }
            }
        }

        return Task.FromResult(expired);
    }
}

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<Booking> bookings = [];
    private readonly Dictionary<int, int> folios = [];
    private readonly Dictionary<string, Booking> idempotentBookings = new(StringComparer.Ordinal);
    private readonly Lock gate = new();

    public Task<IReadOnlyList<Booking>> ListVisibleAsync(string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult<IReadOnlyList<Booking>>(bookings.Where(booking => booking.ClientId == userId || venueIds.Contains(booking.VenueId, StringComparer.Ordinal)).OrderByDescending(booking => booking.CreatedAt).Take(100).ToArray());
    }

    public Task<Booking?> FindVisibleAsync(string id, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(bookings.FirstOrDefault(booking => booking.Id == id && (booking.ClientId == userId || venueIds.Contains(booking.VenueId, StringComparer.Ordinal))));
    }

    public Task<Booking?> FindByFolioVisibleAsync(string folio, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate) return Task.FromResult(bookings.FirstOrDefault(booking => booking.Folio == folio && (booking.ClientId == userId || venueIds.Contains(booking.VenueId, StringComparer.Ordinal))));
    }

    public Task<Booking?> FindByIdempotencyKeyAsync(string clientId, string idempotencyKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (gate)
        {
            idempotentBookings.TryGetValue(Key(clientId, idempotencyKey), out var booking);
            return Task.FromResult(booking);
        }
    }

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
            if (idempotentBookings.ContainsKey(Key(booking.ClientId, booking.IdempotencyKey)))
            {
                throw new DuplicateBookingIdempotencyException();
            }

            bookings.Add(booking);
            idempotentBookings[Key(booking.ClientId, booking.IdempotencyKey)] = booking;
        }

        return Task.CompletedTask;
    }

    private static string Key(string clientId, string idempotencyKey) => $"{clientId}\u001f{idempotencyKey}";
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

    public async Task<IReadOnlyList<Venue>> ListOwnedAsync(string ownerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("ownerId", ownerId);
        var documents = await venues.Find(filter).Limit(100).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Venue> CreateAsync(CreateVenueCommand command, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString("D");
        var now = DateTime.UtcNow;
        var document = new BsonDocument
        {
            ["_id"] = id,
            ["ownerId"] = command.OwnerId,
            ["name"] = command.Name,
            ["shortName"] = command.ShortName,
            ["description"] = command.Description,
            ["address"] = command.Address,
            ["zone"] = command.Zone,
            ["capacity"] = command.Capacity,
            ["price"] = new BsonDocument
            {
                ["base"] = new BsonDecimal128(command.BasePrice),
                ["unit"] = command.PriceUnit,
                ["minHours"] = command.MinimumHours
            },
            ["services"] = new BsonArray(command.Services.Select(service => new BsonDocument
            {
                ["name"] = service.Name,
                ["included"] = service.Included,
                ["price"] = new BsonDecimal128(service.Price)
            })),
            ["photos"] = new BsonArray(command.Photos.Select(photo => new BsonDocument
            {
                ["url"] = photo.Url,
                ["order"] = photo.Order
            })),
            ["categories"] = new BsonArray(command.Categories),
            ["amenities"] = new BsonArray(command.Amenities),
            ["rating"] = new BsonDocument { ["average"] = 0m, ["count"] = 0 },
            ["verified"] = false,
            ["status"] = "draft",
            ["createdAt"] = new BsonDateTime(now),
            ["updatedAt"] = new BsonDateTime(now)
        };

        if (command.Coordinates is not null)
        {
            document["location"] = new BsonDocument
            {
                ["type"] = "Point",
                ["coordinates"] = new BsonArray(command.Coordinates.Select(value => new BsonDouble(value)))
            };
        }

        await venues.InsertOneAsync(document, cancellationToken: cancellationToken);
        return Map(document);
    }

    public async Task<Venue?> UpdateAsync(PatchVenueCommand command, CancellationToken cancellationToken = default)
    {
        var updates = new List<UpdateDefinition<BsonDocument>>();
        var updateBuilder = Builders<BsonDocument>.Update;
        if (command.Name is not null) updates.Add(updateBuilder.Set("name", command.Name));
        if (command.ShortName is not null) updates.Add(updateBuilder.Set("shortName", command.ShortName));
        if (command.Description is not null) updates.Add(updateBuilder.Set("description", command.Description));
        if (command.Address is not null) updates.Add(updateBuilder.Set("address", command.Address));
        if (command.Zone is not null) updates.Add(updateBuilder.Set("zone", command.Zone));
        if (command.Coordinates is not null)
        {
            updates.Add(updateBuilder.Set("location", new BsonDocument
            {
                ["type"] = "Point",
                ["coordinates"] = new BsonArray(command.Coordinates.Select(value => new BsonDouble(value)))
            }));
        }
        if (command.Capacity is not null) updates.Add(updateBuilder.Set("capacity", command.Capacity.Value));
        if (command.BasePrice is not null) updates.Add(updateBuilder.Set("price.base", new BsonDecimal128(command.BasePrice.Value)));
        if (command.PriceUnit is not null) updates.Add(updateBuilder.Set("price.unit", command.PriceUnit));
        if (command.MinimumHours is not null) updates.Add(updateBuilder.Set("price.minHours", command.MinimumHours.Value));
        if (command.Services is not null) updates.Add(updateBuilder.Set("services", new BsonArray(command.Services.Select(service => new BsonDocument
        {
            ["name"] = service.Name,
            ["included"] = service.Included,
            ["price"] = new BsonDecimal128(service.Price)
        }))));
        if (command.Photos is not null) updates.Add(updateBuilder.Set("photos", new BsonArray(command.Photos.Select(photo => new BsonDocument
        {
            ["url"] = photo.Url,
            ["order"] = photo.Order
        }))));
        if (command.Categories is not null) updates.Add(updateBuilder.Set("categories", new BsonArray(command.Categories)));
        if (command.Amenities is not null) updates.Add(updateBuilder.Set("amenities", new BsonArray(command.Amenities)));
        updates.Add(updateBuilder.Set("updatedAt", new BsonDateTime(DateTime.UtcNow)));

        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", command.VenueId),
            Builders<BsonDocument>.Filter.Eq("ownerId", command.OwnerId));
        var document = await venues.FindOneAndUpdateAsync(filter, updateBuilder.Combine(updates),
            new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After }, cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Venue?> PublishAsync(string venueId, string ownerId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", venueId),
            Builders<BsonDocument>.Filter.Eq("ownerId", ownerId));
        var update = Builders<BsonDocument>.Update.Set("status", "published").Set("updatedAt", new BsonDateTime(DateTime.UtcNow));
        var document = await venues.FindOneAndUpdateAsync(filter, update,
            new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After }, cancellationToken);
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
        MinimumHours = document["price"].AsBsonDocument["minHours"].ToInt32(),
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

    public Task AddAsync(Quote quote, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
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
            ["eventType"] = quote.EventType,
            ["eventDate"] = quote.EventDate,
            ["guests"] = quote.Guests,
            ["requestedServices"] = new BsonArray(quote.RequestedServices),
            ["status"] = quote.Status,
            ["createdAt"] = new BsonDateTime(quote.CreatedAt),
            ["updatedAt"] = new BsonDateTime(quote.UpdatedAt)
        };
        if (quote.EventId is not null)
        {
            document["eventId"] = quote.EventId;
        }
        if (quote.Message is not null)
        {
            document["message"] = quote.Message;
        }
        if (quote.Response is not null)
        {
            document["response"] = QuoteResponseDocument.From(quote.Response, quote.UpdatedAt);
        }

        return quotes.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<Quote>> ListVisibleAsync(string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<BsonDocument>> { Builders<BsonDocument>.Filter.Eq("clientId", clientId) };
        if (targetIds.Count > 0) filters.Add(Builders<BsonDocument>.Filter.In("target.id", targetIds));
        var documents = await quotes.Find(Builders<BsonDocument>.Filter.Or(filters)).SortByDescending(document => document["createdAt"]).Limit(100).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Quote?> FindVisibleAsync(string id, string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default)
    {
        var visibility = Builders<BsonDocument>.Filter.Eq("clientId", clientId);
        if (targetIds.Count > 0) visibility = Builders<BsonDocument>.Filter.Or(visibility, Builders<BsonDocument>.Filter.In("target.id", targetIds));
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", id), visibility);
        var document = await quotes.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Quote?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var document = await quotes.Find(Builders<BsonDocument>.Filter.Eq("_id", id)).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Quote?> UpdateAsync(Quote quote, string expectedStatus, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", quote.Id), Builders<BsonDocument>.Filter.Eq("status", expectedStatus));
        var update = Builders<BsonDocument>.Update.Set("status", quote.Status).Set("updatedAt", new BsonDateTime(quote.UpdatedAt));
        // Solo al responder se escribe la respuesta; aceptar, rechazar o vencer conserva la guardada
        // (y su answeredAt).
        if (quote.Response is not null && quote.Status == "answered")
        {
            update = update.Set("response", QuoteResponseDocument.From(quote.Response, quote.UpdatedAt));
        }
        var result = await quotes.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount == 0 ? null : quote;
    }

    private static Quote Map(BsonDocument document)
    {
        var target = document.GetValue("target", new BsonDocument()).AsBsonDocument;
        return new Quote
        {
            Id = document.GetValue("_id").AsString,
            ClientId = document.GetValue("clientId").AsString,
            ClientName = document.GetValue("clientName", "").AsString,
            TargetType = target.GetValue("type", "venue").AsString,
            TargetId = target.GetValue("id", "").AsString,
            TargetName = target.GetValue("name", "").AsString,
            EventId = document.TryGetValue("eventId", out var eventId) ? eventId.AsString : null,
            EventType = document.GetValue("eventType", "Otro").AsString,
            EventDate = document.GetValue("eventDate", "").AsString,
            Guests = document.GetValue("guests", 0).ToInt32(),
            RequestedServices = document.GetValue("requestedServices", new BsonArray()).AsBsonArray.Select(value => value.AsString).ToArray(),
            Message = document.TryGetValue("message", out var message) ? message.AsString : null,
            Response = document.TryGetValue("response", out var response) && response.IsBsonDocument ? QuoteResponseDocument.To(response.AsBsonDocument) : null,
            Status = document.GetValue("status", "sent").AsString,
            CreatedAt = document.GetValue("createdAt").ToUniversalTime(),
            UpdatedAt = document.GetValue("updatedAt").ToUniversalTime()
        };
    }

}

public sealed class MongoBookingRepository(IMongoDatabase database) : IBookingRepository
{
    private readonly IMongoCollection<BsonDocument> bookings = database.GetCollection<BsonDocument>("bookings");
    private readonly IMongoCollection<BsonDocument> counters = database.GetCollection<BsonDocument>("counters");

    public async Task<IReadOnlyList<Booking>> ListVisibleAsync(string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<BsonDocument>> { Builders<BsonDocument>.Filter.Eq("clientId", userId) };
        if (venueIds.Count > 0) filters.Add(Builders<BsonDocument>.Filter.In("venue.venueId", venueIds));
        var documents = await bookings.Find(Builders<BsonDocument>.Filter.Or(filters)).SortByDescending(document => document["createdAt"]).Limit(100).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Booking?> FindVisibleAsync(string id, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        var visibility = Builders<BsonDocument>.Filter.Eq("clientId", userId);
        if (venueIds.Count > 0) visibility = Builders<BsonDocument>.Filter.Or(visibility, Builders<BsonDocument>.Filter.In("venue.venueId", venueIds));
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", id), visibility);
        var document = await bookings.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Booking?> FindByFolioVisibleAsync(string folio, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default)
    {
        var visibility = Builders<BsonDocument>.Filter.Eq("clientId", userId);
        if (venueIds.Count > 0) visibility = Builders<BsonDocument>.Filter.Or(visibility, Builders<BsonDocument>.Filter.In("venue.venueId", venueIds));
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("folio", folio), visibility);
        var document = await bookings.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Booking?> FindByIdempotencyKeyAsync(string clientId, string idempotencyKey, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("clientId", clientId),
            Builders<BsonDocument>.Filter.Eq("idempotencyKey", idempotencyKey));
        var document = await bookings.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<string> NextFolioAsync(int year, CancellationToken cancellationToken = default)
    {
        var counterId = $"folio-{year:D4}";
        var filter = Builders<BsonDocument>.Filter.Eq("_id", counterId);
        var update = Builders<BsonDocument>.Update.Inc("next", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };
        var counter = await counters.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
        return $"VG-{year:D4}-{counter["next"].ToInt32():D4}";
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
        {
            ["_id"] = booking.Id,
            ["folio"] = booking.Folio,
            ["clientId"] = booking.ClientId,
            ["idempotencyKey"] = booking.IdempotencyKey,
            ["clientName"] = booking.ClientName,
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
        };
        if (booking.QuoteId is not null)
        {
            document["quoteId"] = booking.QuoteId;
        }
        if (booking.EventId is not null)
        {
            document["eventId"] = booking.EventId;
        }

        try
        {
            await bookings.InsertOneAsync(document, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException exception) when (exception.WriteError?.Code == 11000)
        {
            throw new DuplicateBookingIdempotencyException();
        }
    }

    private static Booking Map(BsonDocument document) => new()
    {
        Id = document.GetValue("_id").AsString,
        ClientId = document.GetValue("clientId").AsString,
        IdempotencyKey = document.GetValue("idempotencyKey").AsString,
        ClientName = document.GetValue("clientName").AsString,
        QuoteId = document.TryGetValue("quoteId", out var quoteId) && quoteId.IsString ? quoteId.AsString : null,
        EventId = document.TryGetValue("eventId", out var eventId) && eventId.IsString ? eventId.AsString : null,
        VenueId = document["venue"].AsBsonDocument["venueId"].AsString,
        VenueName = document["venue"].AsBsonDocument["name"].AsString,
        Date = document.GetValue("date").AsString,
        Hours = document.GetValue("hours").ToInt32(),
        Total = document.GetValue("total").ToDecimal(),
        Deposit = document.GetValue("deposit").ToDecimal(),
        Folio = document.GetValue("folio").AsString,
        Status = document.GetValue("status").AsString,
        CreatedAt = document.GetValue("createdAt").AsBsonDateTime.ToUniversalTime(),
        UpdatedAt = document.GetValue("updatedAt").AsBsonDateTime.ToUniversalTime()
    };
}
