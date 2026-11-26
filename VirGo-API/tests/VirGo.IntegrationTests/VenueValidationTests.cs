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
/// Issue #49: elementos <c>null</c> en <c>services</c> o <c>photos</c> deben responder 400 por campo, nunca 500.
/// </summary>
public sealed class VenueValidationTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] VenueCategories = ["Salon"];

    [Fact]
    public async Task CreateRejectsNullServiceElement()
    {
        using var client = await CreateOwnerClientAsync();

        using var response = await client.PostAsJsonAsync("/api/v1/venues", VenueBody(services: new object?[] { null }));

        await AssertFieldErrorAsync(response, "services[0]");
        await AssertOwnerHasNoVenuesAsync(client);
    }

    [Fact]
    public async Task CreateRejectsNullPhotoElement()
    {
        using var client = await CreateOwnerClientAsync();

        using var response = await client.PostAsJsonAsync("/api/v1/venues", VenueBody(photos: new object?[] { null }));

        await AssertFieldErrorAsync(response, "photos[0]");
        await AssertOwnerHasNoVenuesAsync(client);
    }

    [Fact]
    public async Task CreateReportsTheIndexOfTheNullElementAmongValidOnes()
    {
        using var client = await CreateOwnerClientAsync();

        using var response = await client.PostAsJsonAsync("/api/v1/venues", VenueBody(
            services: new object?[] { new { name = "Banquete", included = true, price = 0m }, null },
            photos: new object?[] { new { url = "https://cdn.virgo.mx/a.jpg", order = 0 }, null }));

        using var json = await AssertFieldErrorAsync(response, "services[1]");
        var errors = json.RootElement.GetProperty("errors");
        Assert.True(errors.TryGetProperty("photos[1]", out _));
        Assert.False(errors.TryGetProperty("services[0].name", out _));
        Assert.False(errors.TryGetProperty("photos[0].url", out _));
        await AssertOwnerHasNoVenuesAsync(client);
    }

    [Fact]
    public async Task PatchRejectsNullServiceElementAndKeepsTheVenue()
    {
        using var client = await CreateOwnerClientAsync();
        var venueId = await CreateVenueAsync(client);

        using var response = await client.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new
        {
            name = "Nombre que no debe guardarse",
            services = new object?[] { null }
        });

        await AssertFieldErrorAsync(response, "services[0]");
        await AssertVenueNameAsync(client, venueId, "Salon validacion");
    }

    [Fact]
    public async Task PatchRejectsNullPhotoElementAndKeepsTheVenue()
    {
        using var client = await CreateOwnerClientAsync();
        var venueId = await CreateVenueAsync(client);

        using var response = await client.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new
        {
            name = "Nombre que no debe guardarse",
            photos = new object?[] { null }
        });

        await AssertFieldErrorAsync(response, "photos[0]");
        await AssertVenueNameAsync(client, venueId, "Salon validacion");
    }

    private static object VenueBody(object?[]? services = null, object?[]? photos = null) => new
    {
        name = "Salon validacion",
        shortName = "Validacion",
        description = "Un salon para probar la validacion de elementos nulos.",
        address = "Centro, Querétaro",
        zone = "Centro",
        capacity = 80,
        price = new { @base = 1200m, unit = "hour", minHours = 2 },
        services = services ?? Array.Empty<object>(),
        photos,
        categories = VenueCategories,
        amenities = Array.Empty<string>()
    };

    private static async Task<JsonDocument> AssertFieldErrorAsync(HttpResponseMessage response, string field)
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(400, json.RootElement.GetProperty("status").GetInt32());
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty(field, out _), $"Falta el error de {field}.");
        return json;
    }

    private static async Task AssertOwnerHasNoVenuesAsync(HttpClient client)
    {
        using var mine = await client.GetAsync("/api/v1/venues/mine");
        using var json = JsonDocument.Parse(await mine.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.OK, mine.StatusCode);
        Assert.Empty(json.RootElement.GetProperty("data").EnumerateArray());
    }

    private static async Task AssertVenueNameAsync(HttpClient client, string venueId, string expected)
    {
        using var mine = await client.GetAsync("/api/v1/venues/mine");
        using var json = JsonDocument.Parse(await mine.Content.ReadAsStringAsync());
        var venue = json.RootElement.GetProperty("data").EnumerateArray().Single(item => item.GetProperty("id").GetString() == venueId);
        Assert.Equal(expected, venue.GetProperty("name").GetString());
    }

    private static async Task<string> CreateVenueAsync(HttpClient client)
    {
        using var create = await client.PostAsJsonAsync("/api/v1/venues", VenueBody());
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var json = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("id").GetString()!;
    }

    private async Task<HttpClient> CreateOwnerClientAsync()
    {
        var email = $"owner-{Guid.NewGuid():N}@virgo.mx";
        using (var scope = factory.Services.CreateScope())
        {
            var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            var clock = scope.ServiceProvider.GetRequiredService<IClock>();
            var user = new User
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Owner",
                Email = email,
                Phone = "+525500000000",
                PasswordHash = string.Empty,
                Role = UserRole.Owner,
                CreatedAt = clock.UtcNow,
                UpdatedAt = clock.UtcNow
            };
            user.PasswordHash = passwordHasher.Hash(user, "Strong-password-123");
            await users.AddAsync(user);
        }

        var client = factory.CreateClient();
        using var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "Strong-password-123" });
        using var json = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", json.RootElement.GetProperty("accessToken").GetString());
        return client;
    }
}
