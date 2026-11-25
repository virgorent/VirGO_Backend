using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Providers;
using VirGo.Domain.Providers;

namespace VirGo.Infrastructure;

public sealed class InMemoryProviderRepository : IProviderRepository
{
    private readonly List<Provider> providers =
    [
        new Provider
        {
            Id = "aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa",
            UserId = "bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb",
            BusinessName = "Catering VirGo",
            Category = "Catering",
            Description = "Servicio de banquetes para eventos sociales.",
            Zones = ["Centro", "Juriquilla"],
            Packages = [new("basico", "Paquete básico", 450m, ["Menú de tres tiempos"])],
            Gallery = ["https://example.com/catering.jpg"],
            CoverUrl = "https://example.com/catering-cover.jpg",
            RatingAverage = 4.8m,
            RatingCount = 12,
            Pricing = "per_person",
            ResponseTime = "24 horas",
            Status = "published"
        }
    ];

    public Task<IReadOnlyList<Provider>> SearchAsync(ProviderSearch search, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Provider[] snapshot;
        lock (providers)
        {
            snapshot = providers.ToArray();
        }

        var query = snapshot
            .Where(provider => provider.Status == "published")
            .Where(provider => search.Category is null || provider.Category.Equals(search.Category, StringComparison.OrdinalIgnoreCase))
            .Where(provider => search.Zone is null || provider.Zones.Contains(search.Zone, StringComparer.OrdinalIgnoreCase))
            .Where(provider => search.Query is null
                || provider.BusinessName.Contains(search.Query, StringComparison.OrdinalIgnoreCase)
                || provider.Description.Contains(search.Query, StringComparison.OrdinalIgnoreCase))
            .Where(provider => search.Rating is null || provider.RatingAverage >= search.Rating)
            .Where(provider => search.MinPrice is null || provider.Packages.Any(package => package.Price >= search.MinPrice))
            .Where(provider => search.MaxPrice is null || provider.Packages.Any(package => package.Price <= search.MaxPrice))
            .OrderBy(provider => provider.Id);
        if (search.Cursor is not null)
        {
            query = query.Where(provider => string.CompareOrdinal(provider.Id, search.Cursor) > 0).OrderBy(provider => provider.Id);
        }

        return Task.FromResult<IReadOnlyList<Provider>>(query.Take(search.Limit).ToArray());
    }

    public Task<Provider?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (providers)
        {
            return Task.FromResult<Provider?>(providers.FirstOrDefault(provider => provider.Id == id && provider.Status == "published"));
        }
    }

    public Task<Provider?> FindByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (providers)
        {
            return Task.FromResult<Provider?>(providers.FirstOrDefault(provider => provider.UserId == userId));
        }
    }

    public Task<Provider> CreateAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (providers)
        {
            providers.Add(provider);
        }
        return Task.FromResult(provider);
    }

    public Task<Provider?> UpdateAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (providers)
        {
            var index = providers.FindIndex(candidate => candidate.Id == provider.Id && candidate.UserId == provider.UserId);
            if (index < 0) return Task.FromResult<Provider?>(null);
            providers[index] = provider;
            return Task.FromResult<Provider?>(provider);
        }
    }
}

public sealed class MongoProviderRepository(IMongoDatabase database) : IProviderRepository
{
    private readonly IMongoCollection<BsonDocument> providers = database.GetCollection<BsonDocument>("providers");

    public async Task<IReadOnlyList<Provider>> SearchAsync(ProviderSearch search, CancellationToken cancellationToken = default)
    {
        var filters = new List<FilterDefinition<BsonDocument>>
        {
            Builders<BsonDocument>.Filter.Eq("status", "published")
        };
        if (search.Category is not null) filters.Add(Builders<BsonDocument>.Filter.Eq("category", search.Category));
        if (search.Zone is not null) filters.Add(Builders<BsonDocument>.Filter.AnyEq("zones", search.Zone));
        if (search.Rating is not null) filters.Add(Builders<BsonDocument>.Filter.Gte("rating.average", search.Rating.Value));
        if (search.MinPrice is not null) filters.Add(Builders<BsonDocument>.Filter.ElemMatch("packages", Builders<BsonDocument>.Filter.Gte("price", search.MinPrice.Value)));
        if (search.MaxPrice is not null) filters.Add(Builders<BsonDocument>.Filter.ElemMatch("packages", Builders<BsonDocument>.Filter.Lte("price", search.MaxPrice.Value)));
        if (search.Query is not null)
        {
            var regex = new BsonRegularExpression(Regex.Escape(search.Query), "i");
            filters.Add(Builders<BsonDocument>.Filter.Or(
                Builders<BsonDocument>.Filter.Regex("businessName", regex),
                Builders<BsonDocument>.Filter.Regex("description", regex)));
        }
        if (search.Cursor is not null) filters.Add(Builders<BsonDocument>.Filter.Gt("_id", search.Cursor));

        var documents = await providers.Find(Builders<BsonDocument>.Filter.And(filters))
            .SortBy(document => document["_id"])
            .Limit(search.Limit)
            .ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<Provider?> FindPublishedAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", id),
            Builders<BsonDocument>.Filter.Eq("status", "published"));
        var document = await providers.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Provider?> FindByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
        var document = await providers.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<Provider> CreateAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        var document = new BsonDocument
        {
            ["_id"] = provider.Id,
            ["userId"] = provider.UserId,
            ["businessName"] = provider.BusinessName,
            ["category"] = provider.Category,
            ["description"] = provider.Description,
            ["zones"] = new BsonArray(provider.Zones),
            ["packages"] = new BsonArray(provider.Packages.Select(package => new BsonDocument
            {
                ["id"] = package.Id,
                ["name"] = package.Name,
                ["price"] = package.Price,
                ["includes"] = new BsonArray(package.Includes)
            })),
            ["gallery"] = new BsonArray(provider.Gallery.Select(url => new BsonDocument("url", url))),
            ["pricing"] = provider.Pricing,
            ["responseTime"] = provider.ResponseTime,
            ["rating"] = new BsonDocument { ["average"] = provider.RatingAverage, ["count"] = provider.RatingCount },
            ["status"] = provider.Status
        };
        if (provider.CoverUrl is not null)
        {
            document["coverUrl"] = provider.CoverUrl;
        }

        await providers.InsertOneAsync(document, cancellationToken: cancellationToken);
        return provider;
    }

    public async Task<Provider?> UpdateAsync(Provider provider, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("_id", provider.Id),
            Builders<BsonDocument>.Filter.Eq("userId", provider.UserId));
        var update = Builders<BsonDocument>.Update
            .Set("businessName", provider.BusinessName)
            .Set("category", provider.Category)
            .Set("description", provider.Description)
            .Set("zones", new BsonArray(provider.Zones))
            .Set("packages", new BsonArray(provider.Packages.Select(package => new BsonDocument
            {
                ["id"] = package.Id,
                ["name"] = package.Name,
                ["price"] = package.Price,
                ["includes"] = new BsonArray(package.Includes)
            })))
            .Set("gallery", new BsonArray(provider.Gallery.Select(url => new BsonDocument("url", url))))
            .Set("pricing", provider.Pricing)
            .Set("responseTime", provider.ResponseTime);
        update = provider.CoverUrl is null ? update.Unset("coverUrl") : update.Set("coverUrl", provider.CoverUrl);
        var result = await providers.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.MatchedCount == 0 ? null : provider;
    }

    private static Provider Map(BsonDocument document) => new()
    {
        Id = document["_id"].AsString,
        UserId = document["userId"].AsString,
        BusinessName = document["businessName"].AsString,
        Category = document["category"].AsString,
        Description = document["description"].AsString,
        Zones = document["zones"].AsBsonArray.Select(value => value.AsString).ToArray(),
        Packages = document["packages"].AsBsonArray.Select(package =>
        {
            var value = package.AsBsonDocument;
            var includes = value["includes"] is BsonArray array ? array.Select(item => item.AsString).ToArray() : [value["includes"].AsString];
            return new ProviderPackage(value["id"].AsString, value["name"].AsString, value["price"].ToDecimal(), includes);
        }).ToArray(),
        Gallery = document["gallery"].AsBsonArray.Select(value => value.AsBsonDocument["url"].AsString).ToArray(),
        CoverUrl = document.TryGetValue("coverUrl", out var cover) ? cover.AsString : null,
        RatingAverage = document["rating"].AsBsonDocument["average"].ToDecimal(),
        RatingCount = document["rating"].AsBsonDocument["count"].ToInt32(),
        Pricing = document["pricing"].AsString,
        ResponseTime = document["responseTime"].AsString,
        Status = document["status"].AsString
    };
}
