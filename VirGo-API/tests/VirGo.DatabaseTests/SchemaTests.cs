using System.Text.Json;

namespace VirGo.DatabaseTests;

// Revisa el esquema sin base de datos. La comparación contra MongoDB la hace db/tools/check_schema.py --live.
public sealed class SchemaTests
{
    private static readonly string SchemaFolder = FindSchemaFolder();

    public static TheoryData<string> Collections()
    {
        var data = new TheoryData<string>();
        foreach (var file in Directory.GetFiles(SchemaFolder, "*.json"))
        {
            data.Add(Path.GetFileNameWithoutExtension(file));
        }
        return data;
    }

    [Fact]
    public void ThereAreTwentyTwoCollections() => Assert.Equal(22, Directory.GetFiles(SchemaFolder, "*.json").Length);

    [Theory]
    [MemberData(nameof(Collections))]
    public void ValidationIsStrict(string name)
    {
        using var document = Load(name);

        Assert.Equal("strict", document.RootElement.GetProperty("validationLevel").GetString());
        Assert.Equal("error", document.RootElement.GetProperty("validationAction").GetString());
    }

    [Theory]
    [MemberData(nameof(Collections))]
    public void RequiredFieldsAreDeclared(string name)
    {
        using var document = Load(name);
        var schema = JsonSchema(document.RootElement.GetProperty("validator"));
        if (!schema.TryGetProperty("required", out var required))
        {
            return;
        }

        var properties = schema.GetProperty("properties");
        foreach (var field in required.EnumerateArray())
        {
            Assert.True(properties.TryGetProperty(field.GetString()!, out _), field.GetString());
        }
    }

    [Theory]
    [MemberData(nameof(Collections))]
    public void FreeTextHasMaximumLength(string name)
    {
        using var document = Load(name);

        Assert.Empty(Unbounded(JsonSchema(document.RootElement.GetProperty("validator")), string.Empty));
    }

    [Theory]
    [MemberData(nameof(Collections))]
    public void IndexNamesAreUnique(string name)
    {
        using var document = Load(name);
        var names = document.RootElement.GetProperty("indexes").EnumerateArray().Select(index => index.GetProperty("name").GetString()).ToArray();

        Assert.Equal(names.Length, names.Distinct().Count());
    }

    [Theory]
    [InlineData("sessions", "purgeAt")]
    [InlineData("passwordResets", "expiresAt")]
    [InlineData("notices", "purgeAt")]
    [InlineData("assistantMessages", "purgeAt")]
    public void TemporaryDataExpires(string name, string field)
    {
        using var document = Load(name);

        Assert.Contains(document.RootElement.GetProperty("indexes").EnumerateArray(), index =>
            index.GetProperty("key").TryGetProperty(field, out _)
            && index.GetProperty("options").TryGetProperty("expireAfterSeconds", out _));
    }

    [Theory]
    [InlineData("users", "email")]
    [InlineData("sessions", "tokenHash")]
    [InlineData("bookings", "folio")]
    public void BusinessKeysAreUnique(string name, string field)
    {
        using var document = Load(name);

        Assert.Contains(document.RootElement.GetProperty("indexes").EnumerateArray(), index =>
            index.GetProperty("key").TryGetProperty(field, out _)
            && index.GetProperty("options").TryGetProperty("unique", out var unique)
            && unique.GetBoolean());
    }

    private static IEnumerable<string> Unbounded(JsonElement node, string path)
    {
        if (node.ValueKind != JsonValueKind.Object)
        {
            yield break;
        }
        if (node.TryGetProperty("bsonType", out var type) && type.ValueKind == JsonValueKind.String && type.GetString() == "string"
            && !node.TryGetProperty("enum", out _) && !node.TryGetProperty("pattern", out _) && !node.TryGetProperty("maxLength", out _))
        {
            yield return path;
        }
        if (node.TryGetProperty("properties", out var properties))
        {
            foreach (var property in properties.EnumerateObject())
            {
                foreach (var found in Unbounded(property.Value, $"{path}.{property.Name}"))
                {
                    yield return found;
                }
            }
        }
        if (node.TryGetProperty("items", out var items))
        {
            foreach (var found in Unbounded(items, $"{path}[]"))
            {
                yield return found;
            }
        }
    }

    private static JsonElement JsonSchema(JsonElement validator) =>
        validator.TryGetProperty("$jsonSchema", out var schema)
            ? schema
            : validator.GetProperty("$and").EnumerateArray().First(item => item.TryGetProperty("$jsonSchema", out _)).GetProperty("$jsonSchema");

    private static JsonDocument Load(string name) => JsonDocument.Parse(File.ReadAllText(Path.Combine(SchemaFolder, $"{name}.json")));

    private static string FindSchemaFolder()
    {
        for (var folder = new DirectoryInfo(AppContext.BaseDirectory); folder is not null; folder = folder.Parent)
        {
            var candidate = Path.Combine(folder.FullName, "db", "schema");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
        }
        throw new DirectoryNotFoundException("db/schema");
    }
}
