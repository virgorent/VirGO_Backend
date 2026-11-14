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
