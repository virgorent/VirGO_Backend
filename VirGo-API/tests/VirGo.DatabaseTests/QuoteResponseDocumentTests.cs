using System.Text.Json;
using MongoDB.Bson;
using VirGo.Domain.Marketplace;
using VirGo.Infrastructure;

namespace VirGo.DatabaseTests;

// Compara lo que escribe la API en quotes.response con el validador estricto de db/schema/quotes.json.
// SchemaTests revisa el esquema; esta prueba revisa que el código lo cumpla.
public sealed class QuoteResponseDocumentTests
{
    private static readonly DateTime AnsweredAt = new(2026, 11, 26, 12, 0, 0, DateTimeKind.Utc);

    public static TheoryData<decimal?, string?> Responses() => new()
    {
        { 14400m, "Incluye mobiliario." },
        { null, null },
        { 0m, new string('a', QuoteResponseDocument.MaxNoteLength) }
    };

    [Theory]
    [MemberData(nameof(Responses))]
    public void ResponseDocumentHasEveryRequiredFieldWithItsType(decimal? deposit, string? message)
    {
        var document = QuoteResponseDocument.From(new QuoteResponse { Total = 48000m, Deposit = deposit, Message = message }, AnsweredAt);
        using var schema = LoadQuotesSchema();
        var response = schema.RootElement.GetProperty("validator").GetProperty("$jsonSchema").GetProperty("properties").GetProperty("response");

        foreach (var required in response.GetProperty("required").EnumerateArray().Select(item => item.GetString()!))
        {
            Assert.True(document.Contains(required), $"Falta response.{required}");
        }
        foreach (var property in response.GetProperty("properties").EnumerateObject())
        {
            if (!document.TryGetValue(property.Name, out var value)) continue;
            Assert.Equal(property.Value.GetProperty("bsonType").GetString(), BsonTypeName(value));
            if (property.Value.TryGetProperty("maxLength", out var maxLength))
            {
                Assert.True(value.AsString.Length <= maxLength.GetInt32(), $"response.{property.Name} supera {maxLength}");
            }
        }
        Assert.Empty(document["items"].AsBsonArray);
        Assert.All(document.Values.Where(value => value.IsBsonNull), _ => Assert.Fail("La respuesta no debe guardar null"));
    }

    [Fact]
    public void NoteLimitMatchesTheSchema()
    {
        using var schema = LoadQuotesSchema();
        var note = schema.RootElement.GetProperty("validator").GetProperty("$jsonSchema").GetProperty("properties")
            .GetProperty("response").GetProperty("properties").GetProperty("note");
        Assert.Equal(note.GetProperty("maxLength").GetInt32(), QuoteResponseDocument.MaxNoteLength);
    }

    [Fact]
    public void ResponseRoundTripsAndReadsTheOldShape()
    {
        var written = QuoteResponseDocument.From(new QuoteResponse { Total = 30000m, Deposit = 9000m, Message = "Hola" }, AnsweredAt);
        var read = QuoteResponseDocument.To(written);
        Assert.Equal(30000m, read.Total);
        Assert.Equal(9000m, read.Deposit);
        Assert.Equal("Hola", read.Message);
        Assert.Equal(AnsweredAt.Add(QuoteResponseDocument.Validity), written["validUntil"].ToUniversalTime());

        var withoutNote = QuoteResponseDocument.To(QuoteResponseDocument.From(new QuoteResponse { Total = 1m }, AnsweredAt));
        Assert.Null(withoutNote.Message);
        Assert.Null(withoutNote.Deposit);

        var legacy = QuoteResponseDocument.To(new BsonDocument { ["total"] = new BsonDecimal128(5m), ["deposit"] = BsonNull.Value, ["message"] = "Antes" });
        Assert.Equal("Antes", legacy.Message);
        Assert.Null(legacy.Deposit);
    }

    private static string BsonTypeName(BsonValue value) => value.BsonType switch
    {
        BsonType.Array => "array",
        BsonType.String => "string",
        BsonType.Decimal128 => "decimal",
        BsonType.DateTime => "date",
        BsonType.Int32 => "int",
        BsonType.Document => "object",
        _ => value.BsonType.ToString()
    };

    private static JsonDocument LoadQuotesSchema()
    {
        for (var folder = new DirectoryInfo(AppContext.BaseDirectory); folder is not null; folder = folder.Parent)
        {
            var candidate = Path.Combine(folder.FullName, "db", "schema", "quotes.json");
            if (File.Exists(candidate)) return JsonDocument.Parse(File.ReadAllText(candidate));
        }
        throw new FileNotFoundException("db/schema/quotes.json");
    }
}
