using System.Reflection;
using MongoDB.Bson;
using VirGo.Domain.Marketplace;
using VirGo.Infrastructure;

namespace VirGo.UnitTests;

public sealed class MongoVenueMappingTests
{
    private static readonly MethodInfo MapMethod = typeof(MongoVenueRepository)
        .GetMethod("Map", BindingFlags.NonPublic | BindingFlags.Static)
        ?? throw new InvalidOperationException("No se encontró MongoVenueRepository.Map.");

    [Fact]
    public void MapWithoutAmenitiesReturnsEmptyArray()
    {
        var document = CreateVenueDocument(includeAmenities: false);

        var venue = Map(document);

        Assert.Empty(venue.Amenities);
    }

    [Fact]
    public void MapPreservesAmenitiesArray()
    {
        var document = CreateVenueDocument(new BsonArray(["Estacionamiento", "Wi-Fi"]));

        var venue = Map(document);

        Assert.Equal(["Estacionamiento", "Wi-Fi"], venue.Amenities);
    }

    [Fact]
    public void MapWithUnexpectedAmenitiesTypeReturnsEmptyArray()
    {
        var document = CreateVenueDocument(new BsonInt32(42));

        var venue = Map(document);

        Assert.Empty(venue.Amenities);
    }

    private static Venue Map(BsonDocument document) =>
        (Venue)MapMethod.Invoke(null, [document])!;

    private static BsonDocument CreateVenueDocument(BsonValue? amenities = null, bool includeAmenities = true)
    {
        var document = new BsonDocument
        {
            ["_id"] = "11111111-1111-4111-8111-111111111111",
            ["ownerId"] = "22222222-2222-4222-8222-222222222222",
            ["name"] = "Casa VirGo",
            ["shortName"] = "Casa",
            ["description"] = "Descripción válida para el salón de demostración.",
            ["address"] = "Centro, Querétaro",
            ["zone"] = "Centro",
            ["capacity"] = 150,
            ["price"] = new BsonDocument
            {
                ["base"] = new BsonDecimal128(45000m),
                ["unit"] = "day",
                ["minHours"] = 1
            },
            ["categories"] = new BsonArray(["Salón"]),
            ["photos"] = new BsonArray([new BsonDocument { ["url"] = "https://example.test/venue.jpg", ["order"] = 1 }]),
            ["status"] = "published",
            ["rating"] = new BsonDocument
            {
                ["average"] = new BsonDecimal128(5m),
                ["count"] = 1
            }
        };

        if (includeAmenities)
        {
            document["amenities"] = amenities ?? new BsonArray();
        }

        return document;
    }
}
