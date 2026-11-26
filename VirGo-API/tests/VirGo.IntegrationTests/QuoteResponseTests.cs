using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Domain.Auth;
using Xunit;

namespace VirGo.IntegrationTests;

public sealed class QuoteResponseTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] RequestedServices = ["Banquete"];
    private static readonly string[] VenueCategories = ["Salon"];

    [Fact]
    public async Task VenueOwnerCanAnswerAQuoteForTheirVenue()
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Cotizable");
        var quoteId = await CreateClientQuoteAsync(venueId);

        using var response = await ownerClient.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response", new { total = 18000m, deposit = 5000m, message = "Incluye banquete." });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("answered", body.RootElement.GetProperty("status").GetString());
    }

    [Theory]
    [InlineData(18000, 20000, null)]
    [InlineData(18000, -1, null)]
    [InlineData(18000, null, 1001)]
    public async Task AnswerOutsideTheStoredLimitsIsRejected(int total, int? deposit, int? messageLength)
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Limites");
        var quoteId = await CreateClientQuoteAsync(venueId);

        using var response = await ownerClient.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response", new
        {
            total = (decimal)total,
            deposit = (decimal?)deposit,
            message = messageLength is null ? null : new string('a', messageLength.Value)
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AnswerAtTheNoteLimitIsAccepted()
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Nota Larga");
        var quoteId = await CreateClientQuoteAsync(venueId);

        using var response = await ownerClient.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response",
            new { total = 18000m, deposit = 18000m, message = new string('a', 1000) });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCannotAnswerAQuoteForAnotherOwnersVenue()
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Ajeno");
        var quoteId = await CreateClientQuoteAsync(venueId);
        using var otherOwner = factory.CreateClient();
        otherOwner.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));

        using var response = await otherOwner.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response", new { total = 18000m });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProviderCannotAnswerAVenueQuote()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Provider));

        using var response = await client.PostAsJsonAsync($"/api/v1/quotes/{Guid.NewGuid():D}/response", new { total = 18000m });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task QuoteValidationReturnsFieldErrors()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await LoginAsync(client));

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2020-01-01",
            guests = 0,
            requestedServices = RequestedServices
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var errors = body.RootElement.GetProperty("errors");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.True(errors.TryGetProperty("eventDate", out _));
        Assert.True(errors.TryGetProperty("guests", out _));
        Assert.False(errors.TryGetProperty("eventType", out _));
    }

    private static async Task<string> CreatePublishedVenueAsync(HttpClient ownerClient, string name)
    {
        using var create = await ownerClient.PostAsJsonAsync("/api/v1/venues", new
        {
            name,
            shortName = name,
            description = "Un salon creado para probar cotizaciones.",
            address = "Centro, Querétaro",
            zone = "Centro",
            capacity = 200,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            categories = VenueCategories,
            amenities = Array.Empty<string>()
        });
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var venueId = created.RootElement.GetProperty("id").GetString()!;
        using var publish = await ownerClient.PostAsync($"/api/v1/venues/{venueId}/publish", null);
        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        return venueId;
    }

    private async Task<string> CreateClientQuoteAsync(string venueId)
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Client));
        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = venueId },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task<string> LoginAsync(HttpClient client)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }

    private async Task<string> CreateUserTokenAsync(UserRole role)
    {
        var email = $"{role.ToString().ToLowerInvariant()}-{Guid.NewGuid():N}@virgo.mx";
        using var scope = factory.Services.CreateScope();
        var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var clock = scope.ServiceProvider.GetRequiredService<IClock>();
        var user = new User
        {
            Id = Guid.NewGuid().ToString("D"),
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
        using var client = factory.CreateClient();
        using var response = await client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email,
            password = "Strong-password-123"
        });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }
}
