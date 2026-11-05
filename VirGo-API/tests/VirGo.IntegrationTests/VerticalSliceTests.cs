using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace VirGo.IntegrationTests;

public sealed class VerticalSliceTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] RequestedServices = ["Banquete"];

    [Fact]
    public async Task RegisterCreatesAClientAccount()
    {
        using var client = factory.CreateClient();
        var email = $"new-{Guid.NewGuid():N}@virgo.mx";

        using var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Nueva Persona",
            email,
            password = "Strong-password-123",
            phone = "+525500000000"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(email, json.RootElement.GetProperty("email").GetString());
        Assert.Equal("client", json.RootElement.GetProperty("role").GetString());
    }

    [Fact]
    public async Task RegisterRejectsDuplicateEmail()
    {
        using var client = factory.CreateClient();
        var email = $"duplicate-{Guid.NewGuid():N}@virgo.mx";
        var request = new
        {
            name = "Nueva Persona",
            email,
            password = "Strong-password-123",
            phone = "+525500000000"
        };

        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/api/v1/auth/register", request)).StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, (await client.PostAsJsonAsync("/api/v1/auth/register", request)).StatusCode);
    }

    [Fact]
    public async Task AuthenticatedUserCanReadOwnProfile()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/users/me");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("demo@virgo.mx", json.RootElement.GetProperty("email").GetString());
    }

    [Fact]
    public async Task PublicVenueCatalogReturnsPublishedVenues()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/venues?zone=Centro");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.NotEmpty(json.RootElement.GetProperty("data").EnumerateArray());
    }

    [Fact]
    public async Task AuthenticatedClientCanCreateQuoteAndBooking()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var quoteResponse = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventId = "22222222-2222-4222-8222-222222222222",
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices,
            message = "Solicito una cotización."
        });
        Assert.Equal(HttpStatusCode.Created, quoteResponse.StatusCode);

        using var bookingRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                quoteId = Guid.NewGuid().ToString("D"),
                eventId = "22222222-2222-4222-8222-222222222222",
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-05-15",
                hours = 1,
                deposit = 1000
            })
        };
        bookingRequest.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));
        using var bookingResponse = await client.SendAsync(bookingRequest);
        Assert.Equal(HttpStatusCode.Created, bookingResponse.StatusCode);
        using var bookingJson = JsonDocument.Parse(await bookingResponse.Content.ReadAsStringAsync());
        Assert.Equal("pending_payment", bookingJson.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task BookingRequiresIdempotencyKey()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/bookings", new
        {
            venueId = "11111111-1111-4111-8111-111111111111",
            date = "2027-05-15",
            hours = 1
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private static async Task<string> LoginAsync(HttpClient client)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }
}
