using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace VirGo.ContractTests;

// Fija la forma del JSON que consume la app móvil: si cambia, Móvil se rompe.
public sealed class MobileContractTests(ContractApiFactory factory) : IClassFixture<ContractApiFactory>
{
    private readonly HttpClient client = factory.CreateClient();

    [Theory]
    [InlineData("/api/v1/auth/register", "post")]
    [InlineData("/api/v1/auth/login", "post")]
    [InlineData("/api/v1/auth/logout", "post")]
    [InlineData("/api/v1/users/me", "get")]
    [InlineData("/api/v1/users/me", "delete")]
    [InlineData("/api/v1/users/me", "patch")]
    [InlineData("/api/v1/users/me/password", "patch")]
    [InlineData("/api/v1/health/ready", "get")]
    [InlineData("/api/v1/notices", "get")]
    [InlineData("/api/v1/notices/{id}/read", "patch")]
    [InlineData("/api/v1/notices/read-all", "post")]
    [InlineData("/api/v1/assistant/messages", "get")]
    [InlineData("/api/v1/contact", "post")]
    [InlineData("/api/v1/providers", "get")]
    [InlineData("/api/v1/providers", "post")]
    [InlineData("/api/v1/providers/me", "patch")]
    [InlineData("/api/v1/providers/me/gigs", "get")]
    [InlineData("/api/v1/providers/me/gigs", "post")]
    [InlineData("/api/v1/providers/me/gigs/{id}", "patch")]
    [InlineData("/api/v1/events", "get")]
    [InlineData("/api/v1/events", "post")]
    [InlineData("/api/v1/events/{id}", "get")]
    [InlineData("/api/v1/events/{id}", "patch")]
    [InlineData("/api/v1/events/{id}/guests", "get")]
    [InlineData("/api/v1/events/{id}/guests", "post")]
    [InlineData("/api/v1/events/{id}/guests/{guestId}", "patch")]
    [InlineData("/api/v1/events/{id}/guests/{guestId}", "delete")]
    [InlineData("/api/v1/events/{id}/tasks", "get")]
    [InlineData("/api/v1/events/{id}/tasks", "post")]
    [InlineData("/api/v1/events/{id}/tasks/{taskId}", "patch")]
    [InlineData("/api/v1/events/{id}/tasks/{taskId}", "delete")]
    [InlineData("/api/v1/events/{id}/collaborators", "get")]
    [InlineData("/api/v1/events/{id}/collaborators", "post")]
    [InlineData("/api/v1/events/{id}/collaborators/{userId}", "patch")]
    [InlineData("/api/v1/events/{id}/collaborators/{userId}", "delete")]
    [InlineData("/api/v1/quotes", "get")]
    [InlineData("/api/v1/quotes/{id}", "get")]
    [InlineData("/api/v1/quotes/{id}/response", "post")]
    [InlineData("/api/v1/quotes/{id}/accept", "post")]
    [InlineData("/api/v1/quotes/{id}/reject", "post")]
    [InlineData("/api/v1/quotes/{id}/expire", "post")]
    [InlineData("/api/v1/bookings", "get")]
    [InlineData("/api/v1/bookings/{id}", "get")]
    [InlineData("/api/v1/bookings/by-folio/{folio}", "get")]
    [InlineData("/api/v1/bookings/{id}/cancel", "post")]
    [InlineData("/api/v1/bookings/{id}/payments", "post")]
    [InlineData("/api/v1/bookings/{id}/payments", "get")]
    [InlineData("/api/v1/venues/{id}/reviews", "get")]
    [InlineData("/api/v1/providers/{id}/reviews", "get")]
    [InlineData("/api/v1/reviews", "post")]
    [InlineData("/api/v1/reviews/{id}/reply", "post")]
    [InlineData("/api/v1/venues/{id}/stats", "get")]
    [InlineData("/api/v1/providers/{id}/stats", "get")]
    [InlineData("/api/v1/stats/overview", "get")]
    [InlineData("/api/v1/favorites", "get")]
    [InlineData("/api/v1/favorites", "post")]
    [InlineData("/api/v1/favorites/{targetType}/{targetId}", "delete")]
    [InlineData("/api/v1/conversations", "get")]
    [InlineData("/api/v1/conversations", "post")]
    [InlineData("/api/v1/conversations/{id}", "get")]
    [InlineData("/api/v1/conversations/{id}/messages", "get")]
    [InlineData("/api/v1/conversations/{id}/messages", "post")]
    [InlineData("/api/v1/conversations/{id}/read", "patch")]
    [InlineData("/api/v1/payment-cards", "get")]
    [InlineData("/api/v1/payment-cards", "post")]
    [InlineData("/api/v1/payment-cards/{id}", "patch")]
    [InlineData("/api/v1/payment-cards/{id}", "delete")]
    [InlineData("/api/v1/payment-cards/{id}/default", "post")]
    [InlineData("/api/v1/venues/{id}/blocks", "post")]
    [InlineData("/api/v1/venues/{id}/blocks/{blockId}", "delete")]
    [InlineData("/api/v1/assistant/messages", "post")]
    [InlineData("/api/v1/providers/{id}", "get")]
    [InlineData("/api/v1/providers/me", "get")]
    [InlineData("/api/v1/venues/{id}", "get")]
    [InlineData("/api/v1/venues/mine", "get")]
    [InlineData("/api/v1/venues", "post")]
    [InlineData("/api/v1/venues/{id}", "patch")]
    [InlineData("/api/v1/venues/{id}/availability", "get")]
    [InlineData("/api/v1/users/{id}/public", "get")]
    [InlineData("/api/v1/venues", "get")]
    [InlineData("/api/v1/quotes", "post")]
    [InlineData("/api/v1/bookings", "post")]
    public async Task OpenApiPublishesEveryEndpoint(string path, string method)
    {
        using var document = JsonDocument.Parse(await client.GetStringAsync("/openapi/v1.json"));

        Assert.True(document.RootElement.GetProperty("paths").TryGetProperty(path, out var item), path);
        Assert.True(item.TryGetProperty(method, out _), $"{method} {path}");
    }

    [Fact]
    public async Task RegisterAndLoginReturnTheAgreedShape()
    {
        var email = $"{Guid.NewGuid():N}@virgo.test";
        var registered = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Ana López",
            email,
            password = "Password-Segura-1",
            phone = "+524421234567"
        });
        Assert.Equal(HttpStatusCode.Created, registered.StatusCode);

        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "Password-Segura-1" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        using var body = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        var root = body.RootElement;

        foreach (var field in new[] { "accessToken", "tokenType", "expiresIn", "refreshToken", "refreshTokenExpiresAt", "user" })
        {
            Assert.True(root.TryGetProperty(field, out _), field);
        }
        foreach (var field in new[] { "id", "name", "email", "role" })
        {
            Assert.True(root.GetProperty("user").TryGetProperty(field, out _), field);
        }
    }

    [Fact]
    public async Task VenuesReturnTheAgreedShape()
    {
        using var body = JsonDocument.Parse(await client.GetStringAsync("/api/v1/venues"));
        var venue = body.RootElement.GetProperty("data").EnumerateArray().First();

        foreach (var field in new[] { "id", "name", "shortName", "zone", "capacity", "price", "categories", "amenities", "photos", "rating" })
        {
            Assert.True(venue.TryGetProperty(field, out _), field);
        }
    }

    [Fact]
    public async Task ErrorsUseProblemDetails()
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "nadie@virgo.test", password = "incorrecta" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(body.RootElement.TryGetProperty("traceId", out _));
    }
}
