using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Domain.Auth;
using Xunit;

namespace VirGo.IntegrationTests;

/// <summary>
/// Issue #52: respuestas de error contractuales de venues, disponibilidad y bloqueos
/// (specs 026, 027, 029, 031, 032 y 033).
/// </summary>
public sealed class VenueNegativeTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private const string SeedVenueId = "11111111-1111-4111-8111-111111111111";
    private const string UnknownVenueId = "99999999-9999-4999-8999-999999999999";
    private const string PrivateVenueName = "Salon privado de otra persona";
    private static readonly string[] VenueCategories = ["Salon"];

    // ---------- Venues: 403 por rol ----------

    [Fact]
    public async Task ClientCannotCreateAVenue()
    {
        using var client = await CreateClientAsync(UserRole.Client);

        using var response = await client.PostAsJsonAsync("/api/v1/venues", VenueBody("Salon de cliente"));

        await AssertProblemAsync(response, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ClientCannotPatchOrPublishAVenue()
    {
        using var client = await CreateClientAsync(UserRole.Client);

        using var patch = await client.PatchAsJsonAsync($"/api/v1/venues/{SeedVenueId}", new { name = "Robado" });
        using var publish = await client.PostAsync($"/api/v1/venues/{SeedVenueId}/publish", null);

        await AssertProblemAsync(patch, HttpStatusCode.Forbidden);
        await AssertProblemAsync(publish, HttpStatusCode.Forbidden);
    }

    // ---------- Venues: 404 por recurso ajeno o inexistente ----------

    [Fact]
    public async Task OwnerCannotPatchAnotherOwnersVenueAndItStaysUnchanged()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        using var intruder = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await intruder.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new { name = "Nombre del intruso" });

        await AssertNotFoundWithoutLeakAsync(response);
        Assert.Equal(PrivateVenueName, await FindOwnedVenueNameAsync(owner, venueId));
    }

    [Fact]
    public async Task OwnerCannotPublishAnotherOwnersVenue()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        using var intruder = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await intruder.PostAsync($"/api/v1/venues/{venueId}/publish", null);

        await AssertNotFoundWithoutLeakAsync(response);
        using var detail = await intruder.GetAsync($"/api/v1/venues/{venueId}");
        Assert.Equal(HttpStatusCode.NotFound, detail.StatusCode);
    }

    [Theory]
    [InlineData(UnknownVenueId)]
    [InlineData("no-es-un-guid")]
    public async Task OwnerGetsNotFoundForUnknownVenue(string venueId)
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var patch = await owner.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new { name = "Nuevo nombre" });
        using var publish = await owner.PostAsync($"/api/v1/venues/{venueId}/publish", null);
        using var detail = await owner.GetAsync($"/api/v1/venues/{venueId}");

        await AssertProblemAsync(patch, HttpStatusCode.NotFound);
        await AssertProblemAsync(publish, HttpStatusCode.NotFound);
        await AssertProblemAsync(detail, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DraftVenueIsNotVisibleInThePublicDetail()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        using var anonymous = factory.CreateClient();

        using var response = await anonymous.GetAsync($"/api/v1/venues/{venueId}");

        await AssertNotFoundWithoutLeakAsync(response);
    }

    // ---------- Venues: 400 de validación ----------

    [Fact]
    public async Task CreateWithoutBodyIsAValidationProblem()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var response = await owner.PostAsync(
            "/api/v1/venues",
            new StringContent("", System.Text.Encoding.UTF8, "application/json"));

        await AssertProblemAsync(response, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateReportsEachInvalidField()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var response = await owner.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "",
            shortName = new string('x', 21),
            description = "corta",
            address = "Centro",
            zone = "Marte",
            capacity = 0,
            price = new { @base = -1m, unit = "week", minHours = 0 },
            services = new[] { new { name = "", included = (bool?)null, price = -5m } },
            photos = new[] { new { url = "", order = -1 } },
            categories = new[] { "Castillo" },
            amenities = new[] { "Helipuerto" }
        });

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        var errors = json.RootElement.GetProperty("errors");
        foreach (var field in new[]
        {
            "name", "shortName", "description", "zone", "capacity", "price.base", "price.unit", "price.minHours",
            "services[0].name", "services[0].included", "services[0].price", "photos[0].url", "photos[0].order",
            "categories", "amenities"
        })
        {
            Assert.True(errors.TryGetProperty(field, out _), $"Falta el error de {field}.");
        }
        await AssertOwnerHasNoVenuesAsync(owner);
    }

    [Fact]
    public async Task CreateRejectsA1500CharacterText()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var response = await owner.PostAsJsonAsync("/api/v1/venues", VenueBody(new string('a', 1500)));

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("name", out _));
        await AssertOwnerHasNoVenuesAsync(owner);
    }

    [Fact]
    public async Task PatchWithoutUpdatableFieldsIsAValidationProblem()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await owner.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new { rating = 5 });

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("body", out _));
    }

    [Fact]
    public async Task PatchRejectsInvalidPriceAndKeepsTheVenue()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await owner.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new
        {
            name = "No debe guardarse",
            price = new { @base = 100m, unit = "week", minHours = 1 }
        });

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("price.unit", out _));
        Assert.Equal(PrivateVenueName, await FindOwnedVenueNameAsync(owner, venueId));
    }

    // ---------- Disponibilidad ----------

    [Theory]
    [InlineData(UnknownVenueId)]
    [InlineData("no-es-un-guid")]
    public async Task AvailabilityOfUnknownVenueIsNotFound(string venueId)
    {
        using var anonymous = factory.CreateClient();

        using var response = await anonymous.GetAsync($"/api/v1/venues/{venueId}/availability?from=2027-01-01&to=2027-01-31");

        await AssertProblemAsync(response, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AvailabilityOfDraftVenueIsNotFound()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        using var anonymous = factory.CreateClient();

        using var response = await anonymous.GetAsync($"/api/v1/venues/{venueId}/availability?from=2027-01-01&to=2027-01-31");

        await AssertNotFoundWithoutLeakAsync(response);
    }

    [Theory]
    [InlineData("", "", "from")]
    [InlineData("from=2027-01-01", "", "to")]
    [InlineData("from=2027-13-01", "to=2027-12-31", "from")]
    [InlineData("from=2027-01-01", "to=31-12-2027", "to")]
    [InlineData("from=2027-02-30", "to=2027-03-01", "from")]
    [InlineData("from=2027-03-02", "to=2027-03-01", "range")]
    public async Task InvalidAvailabilityRangesAreValidationProblems(string from, string to, string field)
    {
        using var anonymous = factory.CreateClient();

        using var response = await anonymous.GetAsync($"/api/v1/venues/{SeedVenueId}/availability?{from}&{to}");

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty(field, out _), $"Falta el error de {field}.");
    }

    [Fact]
    public async Task AvailabilityRangeOfTwoYearsIsAValidationProblem()
    {
        using var anonymous = factory.CreateClient();

        using var response = await anonymous.GetAsync($"/api/v1/venues/{SeedVenueId}/availability?from=2027-01-01&to=2029-01-01");

        using var json = await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("range", out _));
    }

    // ---------- Bloqueos: 401, 403/404 ----------

    [Fact]
    public async Task BlocksRequireAToken()
    {
        using var anonymous = factory.CreateClient();

        using var create = await anonymous.PostAsJsonAsync($"/api/v1/venues/{SeedVenueId}/blocks", BlockBody("2027-08-01"));
        using var delete = await anonymous.DeleteAsync($"/api/v1/venues/{SeedVenueId}/blocks/sample-block");

        await AssertProblemAsync(create, HttpStatusCode.Unauthorized);
        await AssertProblemAsync(delete, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task OwnerCannotBlockAnotherOwnersVenue()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        using var intruder = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await intruder.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-08-01"));

        await AssertNotFoundWithoutLeakAsync(response);
        // El día sigue libre para el dueño real.
        using var ownBlock = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-08-01"));
        Assert.Equal(HttpStatusCode.Created, ownBlock.StatusCode);
    }

    [Fact]
    public async Task ClientCannotBlockAVenue()
    {
        using var client = await CreateClientAsync(UserRole.Client);

        using var response = await client.PostAsJsonAsync($"/api/v1/venues/{SeedVenueId}/blocks", BlockBody("2027-08-02"));

        Assert.True(
            response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.NotFound,
            $"Se esperaba 403 o 404 y llegó {(int)response.StatusCode}.");
        await AssertProblemAsync(response, response.StatusCode);
    }

    [Theory]
    [InlineData(UnknownVenueId)]
    [InlineData("no-es-un-guid")]
    public async Task BlockingAnUnknownVenueIsNotFound(string venueId)
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var response = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-08-03"));

        await AssertProblemAsync(response, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task BlockingANonGuidVenueWithAnInvalidBodyIsStillNotFound()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);

        using var response = await owner.PostAsJsonAsync("/api/v1/venues/no-es-un-guid/blocks", new { date = "mal" });

        await AssertProblemAsync(response, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task OwnerCannotDeleteAnotherOwnersBlock()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        using var intruder = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        var blockId = await CreateBlockAsync(owner, venueId, "2027-09-01");

        using var response = await intruder.DeleteAsync($"/api/v1/venues/{venueId}/blocks/{blockId}");

        await AssertNotFoundWithoutLeakAsync(response);
        // El bloqueo sigue en pie: repetirlo choca.
        using var duplicate = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-09-01"));
        Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);
    }

    [Fact]
    public async Task OwnerCannotDeleteABlockThroughTheirOwnVenueRoute()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        using var intruder = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        var blockId = await CreateBlockAsync(owner, venueId, "2027-09-02");
        var intruderVenueId = await CreateVenueAsync(intruder, "Salon del intruso");

        using var response = await intruder.DeleteAsync($"/api/v1/venues/{intruderVenueId}/blocks/{blockId}");

        await AssertProblemAsync(response, HttpStatusCode.NotFound);
        using var stillThere = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-09-02"));
        Assert.Equal(HttpStatusCode.Conflict, stillThere.StatusCode);
    }

    [Fact]
    public async Task DeletingAnUnknownOrAlreadyDeletedBlockIsNotFound()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        var blockId = await CreateBlockAsync(owner, venueId, "2027-09-03");

        using var unknown = await owner.DeleteAsync($"/api/v1/venues/{venueId}/blocks/{Guid.NewGuid():D}");
        using var first = await owner.DeleteAsync($"/api/v1/venues/{venueId}/blocks/{blockId}");
        using var second = await owner.DeleteAsync($"/api/v1/venues/{venueId}/blocks/{blockId}");

        await AssertProblemAsync(unknown, HttpStatusCode.NotFound);
        Assert.Equal(HttpStatusCode.NoContent, first.StatusCode);
        await AssertProblemAsync(second, HttpStatusCode.NotFound);
    }

    // ---------- Bloqueos: 409 ----------

    [Fact]
    public async Task DuplicateBlockOnTheSameDayIsAConflict()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        await CreateBlockAsync(owner, venueId, "2027-10-01");

        using var response = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", new
        {
            date = "2027-10-01",
            reason = "other",
            note = "Segundo intento"
        });

        await AssertProblemAsync(response, HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task BlockingADayAlreadyBlockedBySeedDataIsAConflict()
    {
        using var demoOwner = await CreateClientForExistingUserAsync("22222222-2222-4222-8222-222222222222");

        using var response = await demoOwner.PostAsJsonAsync($"/api/v1/venues/{SeedVenueId}/blocks", BlockBody("2027-05-20"));

        await AssertProblemAsync(response, HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ADayCanBeBlockedAgainAfterItsBlockIsDeleted()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);
        var blockId = await CreateBlockAsync(owner, venueId, "2027-10-02");
        using var delete = await owner.DeleteAsync($"/api/v1/venues/{venueId}/blocks/{blockId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        using var again = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-10-02"));

        Assert.Equal(HttpStatusCode.Created, again.StatusCode);
    }

    // ---------- Bloqueos: 400 ----------

    public static TheoryData<string> InvalidBlockBodies => new()
    {
        "{}",
        """{"date":"2027-10-03"}""",
        """{"date":"2027-10-03","reason":"vacaciones"}""",
        """{"date":"03/10/2027","reason":"maintenance"}""",
        """{"date":"2027-02-30","reason":"maintenance"}""",
        """{"date":null,"reason":"maintenance"}""",
        $$"""{"date":"2027-10-03","reason":"other","note":"{{new string('n', 201)}}"}""",
        $$"""{"date":"2027-10-03","reason":"other","note":"{{new string('n', 1500)}}"}"""
    };

    [Theory]
    [MemberData(nameof(InvalidBlockBodies))]
    public async Task InvalidBlocksAreValidationProblemsAndDoNotBlockTheDay(string body)
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await owner.PostAsync(
            $"/api/v1/venues/{venueId}/blocks",
            new StringContent(body, System.Text.Encoding.UTF8, "application/json"));

        await AssertProblemAsync(response, HttpStatusCode.BadRequest);
        using var valid = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody("2027-10-03"));
        Assert.Equal(HttpStatusCode.Created, valid.StatusCode);
    }

    [Fact]
    public async Task BlockNoteOf200CharactersIsAccepted()
    {
        using var owner = await CreateClientAsync(UserRole.Owner);
        var venueId = await CreateVenueAsync(owner, PrivateVenueName);

        using var response = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", new
        {
            date = "2027-10-04",
            reason = "other",
            note = new string('n', 200)
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    // ---------- Ayudantes ----------

    private static object VenueBody(string name) => new
    {
        name,
        shortName = "Negativas",
        description = "Un salon para probar las respuestas de error del contrato.",
        address = "Centro, Querétaro",
        zone = "Centro",
        capacity = 80,
        price = new { @base = 1200m, unit = "hour", minHours = 2 },
        services = Array.Empty<object>(),
        categories = VenueCategories,
        amenities = Array.Empty<string>()
    };

    private static object BlockBody(string date) => new { date, reason = "maintenance" };

    private static async Task<JsonDocument> AssertProblemAsync(HttpResponseMessage response, HttpStatusCode expected)
    {
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal((int)expected, json.RootElement.GetProperty("status").GetInt32());
        return json;
    }

    private static async Task AssertNotFoundWithoutLeakAsync(HttpResponseMessage response)
    {
        using var json = await AssertProblemAsync(response, HttpStatusCode.NotFound);
        Assert.DoesNotContain(PrivateVenueName, json.RootElement.GetRawText(), StringComparison.Ordinal);
    }

    private static async Task AssertOwnerHasNoVenuesAsync(HttpClient owner)
    {
        using var mine = await owner.GetAsync("/api/v1/venues/mine");
        using var json = JsonDocument.Parse(await mine.Content.ReadAsStringAsync());
        Assert.Empty(json.RootElement.GetProperty("data").EnumerateArray());
    }

    private static async Task<string?> FindOwnedVenueNameAsync(HttpClient owner, string venueId)
    {
        using var mine = await owner.GetAsync("/api/v1/venues/mine");
        using var json = JsonDocument.Parse(await mine.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("data").EnumerateArray()
            .Single(item => item.GetProperty("id").GetString() == venueId)
            .GetProperty("name").GetString();
    }

    private static async Task<string> CreateVenueAsync(HttpClient owner, string name)
    {
        using var create = await owner.PostAsJsonAsync("/api/v1/venues", VenueBody(name));
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var json = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task<string> CreateBlockAsync(HttpClient owner, string venueId, string date)
    {
        using var create = await owner.PostAsJsonAsync($"/api/v1/venues/{venueId}/blocks", BlockBody(date));
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var json = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("id").GetString()!;
    }

    private Task<HttpClient> CreateClientAsync(UserRole role) => CreateClientForUserAsync(Guid.NewGuid().ToString("D"), role);

    /// <summary>Da de alta en memoria un usuario con el id del dueño semilla, si aún no existe.</summary>
    private Task<HttpClient> CreateClientForExistingUserAsync(string userId) => CreateClientForUserAsync(userId, UserRole.Owner);

    private async Task<HttpClient> CreateClientForUserAsync(string userId, UserRole role)
    {
        var email = $"{role.ToString().ToLowerInvariant()}-{Guid.NewGuid():N}@virgo.mx";
        using (var scope = factory.Services.CreateScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var clock = scope.ServiceProvider.GetRequiredService<IClock>();
            var user = new User
            {
                Id = userId,
                Name = role.ToString(),
                Email = email,
                Phone = "+525500000000",
                PasswordHash = string.Empty,
                Role = role,
                CreatedAt = clock.UtcNow,
                UpdatedAt = clock.UtcNow
            };
            user.PasswordHash = passwordHasher.Hash(user, "Strong-password-123");
            await users.AddAsync(user);
        }

        var client = factory.CreateClient();
        using var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "Strong-password-123" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var json = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", json.RootElement.GetProperty("accessToken").GetString());
        return client;
    }
}
