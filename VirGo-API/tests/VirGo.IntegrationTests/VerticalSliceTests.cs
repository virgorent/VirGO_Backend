using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Domain.Auth;
using VirGo.Domain.Marketplace;
using Xunit;

namespace VirGo.IntegrationTests;

public sealed class VerticalSliceTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] RequestedServices = ["Banquete"];
    private static readonly string[] VenueCategories = ["Salon"];

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
    public async Task RegisterRejectsInvalidInputAsProblemDetails()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "",
            email = "not-an-email",
            password = "short",
            phone = ""
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
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
    public async Task ProfileRejectsAnInvalidToken()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid-token");

        using var response = await client.GetAsync("/api/v1/users/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
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
    public async Task PublicVenueCatalogFiltersBySchemaCategory()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/venues?category=Salon");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.NotEmpty(json.RootElement.GetProperty("data").EnumerateArray());
        Assert.DoesNotContain("ownerId", json.RootElement.GetProperty("data")[0].EnumerateObject().Select(property => property.Name));
    }

    [Fact]
    public async Task PublicVenueCatalogReturnsNoResultsForUnknownCategory()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/venues?category=Rooftop");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Empty(json.RootElement.GetProperty("data").EnumerateArray());
    }

    [Fact]
    public async Task OwnerCanCreateAndPublishVenueWithoutLocationOrPhotos()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var create = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "Salon MVP",
            shortName = "Salon MVP",
            description = "Un salon creado sin mapa ni fotos para el MVP.",
            address = "Centro, Querétaro",
            zone = "Centro",
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            categories = VenueCategories,
            amenities = Array.Empty<string>()
        });
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var venueId = created.RootElement.GetProperty("id").GetString();

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        Assert.Empty(created.RootElement.GetProperty("photos").EnumerateArray());
        Assert.Equal("draft", created.RootElement.GetProperty("status").GetString());

        using var publish = await client.PostAsync($"/api/v1/venues/{venueId}/publish", null);
        using var published = JsonDocument.Parse(await publish.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        Assert.Equal(venueId, published.RootElement.GetProperty("id").GetString());
        Assert.Equal("published", published.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task QuoteRejectsNullRequestedService()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = new string?[] { null }
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task QuoteRequiresAuthentication()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCannotCreateQuote()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices
        });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCannotCreateBooking()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-05-15",
                hours = 1
            })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task QuoteRejectsPastEventDate()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2020-01-01",
            guests = 150,
            requestedServices = RequestedServices
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task QuoteReturnsNotFoundForUnknownVenue()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "99999999-9999-4999-8999-999999999999" },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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
        var bookingId = bookingJson.RootElement.GetProperty("id").GetString();
        Assert.Equal("pending_payment", bookingJson.RootElement.GetProperty("status").GetString());

        using var replayRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                eventId = "22222222-2222-4222-8222-222222222222",
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-05-15",
                hours = 1,
                deposit = 1000
            })
        };
        replayRequest.Headers.Add("Idempotency-Key", bookingRequest.Headers.GetValues("Idempotency-Key").Single());
        using var replayResponse = await client.SendAsync(replayRequest);
        Assert.Equal(HttpStatusCode.Created, replayResponse.StatusCode);
        using var replayJson = JsonDocument.Parse(await replayResponse.Content.ReadAsStringAsync());
        Assert.Equal(bookingId, replayJson.RootElement.GetProperty("id").GetString());
    }

    [Fact]
    public async Task BookingRejectsDepositAboveCalculatedTotal()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-05-15",
                hours = 1,
                deposit = 45001
            })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task BookingCalculatesDayPriceAndDefaultDeposit()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-05-15",
                hours = 25
            })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(90000, json.RootElement.GetProperty("total").GetDecimal());
        Assert.Equal(27000, json.RootElement.GetProperty("deposit").GetDecimal());
    }

    [Fact]
    public async Task ConcurrentBookingRequestsWithTheSameKeyReturnOneBooking()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var idempotencyKey = Guid.NewGuid().ToString("D");

        var responses = await Task.WhenAll(Enumerable.Range(0, 8).Select(async _ =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
            {
                Content = JsonContent.Create(new
                {
                    venueId = "11111111-1111-4111-8111-111111111111",
                    date = "2027-05-15",
                    hours = 1,
                    deposit = 1000
                })
            };
            request.Headers.Add("Idempotency-Key", idempotencyKey);
            return await client.SendAsync(request);
        }));

        Assert.All(responses, response => Assert.Equal(HttpStatusCode.Created, response.StatusCode));
        var payloads = await Task.WhenAll(responses.Select(async response =>
        {
            using (response)
            {
                using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                return (
                    Id: payload.RootElement.GetProperty("id").GetString(),
                    Folio: payload.RootElement.GetProperty("folio").GetString());
            }
        }));
        var bookingIds = payloads.Select(payload => payload.Id).Distinct().ToArray();
        var folios = payloads.Select(payload => payload.Folio).Distinct().ToArray();
        Assert.Single(bookingIds);
        Assert.Single(folios);
    }

    [Fact]
    public async Task LoginLocksAnAccountAfterFiveFailedAttempts()
    {
        using var client = factory.CreateClient();
        var email = $"lockout-{Guid.NewGuid():N}@virgo.mx";
        var password = "Strong-password-123";

        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Cuenta bloqueable",
            email,
            password,
            phone = "+525500000000"
        })).StatusCode);

        for (var attempt = 0; attempt < 5; attempt++)
        {
            using var failed = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "wrong-password" });
            Assert.Equal(HttpStatusCode.Unauthorized, failed.StatusCode);
        }

        using var locked = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        Assert.Equal(HttpStatusCode.TooManyRequests, locked.StatusCode);
        Assert.True(locked.Headers.Contains("Retry-After"));
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

    [Fact]
    public async Task ClientCanReadBookingQueries()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var list = await client.GetAsync("/api/v1/bookings");
        using var listBody = JsonDocument.Parse(await list.Content.ReadAsStringAsync());
        using var missingFolio = await client.GetAsync("/api/v1/bookings/by-folio/VG-2099-9999");

        Assert.Equal(HttpStatusCode.OK, list.StatusCode);
        Assert.True(listBody.RootElement.TryGetProperty("data", out _));
        Assert.Equal(HttpStatusCode.NotFound, missingFolio.StatusCode);
    }

    [Fact]
    public async Task BookingWithAnAnsweredQuoteUsesTheQuotedTotalAndAcceptsIt()
    {
        using var client = await ClientWithQuoteAsync("2027-06-20");
        var quoteId = await AnswerLatestQuoteAsync(48000m, 10000m);

        using var booking = await BookAsync(client, quoteId, "2027-06-20");
        Assert.Equal(HttpStatusCode.Created, booking.StatusCode);
        using var json = JsonDocument.Parse(await booking.Content.ReadAsStringAsync());
        Assert.Equal(48000m, json.RootElement.GetProperty("total").GetDecimal());
        Assert.Equal(10000m, json.RootElement.GetProperty("deposit").GetDecimal());
        Assert.Equal(quoteId, json.RootElement.GetProperty("quoteId").GetString());

        using var quote = await client.GetAsync($"/api/v1/quotes/{quoteId}");
        using var quoteJson = JsonDocument.Parse(await quote.Content.ReadAsStringAsync());
        Assert.Equal("accepted", quoteJson.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task BookingRejectsAQuoteThatIsNotAnsweredOrForAnotherDate()
    {
        using var client = await ClientWithQuoteAsync("2027-07-10");
        using var unanswered = await BookAsync(client, await LatestQuoteIdAsync(), "2027-07-10");
        var quoteId = await AnswerLatestQuoteAsync(30000m, null);
        using var otherDate = await BookAsync(client, quoteId, "2027-07-11");
        using var unknown = await BookAsync(client, Guid.NewGuid().ToString("D"), "2027-07-10");

        Assert.Equal(HttpStatusCode.BadRequest, unanswered.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, otherDate.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, unknown.StatusCode);
    }

    private const string SeedVenueId = "11111111-1111-4111-8111-111111111111";

    private async Task<HttpClient> ClientWithQuoteAsync(string date)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Client));
        using var created = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = SeedVenueId },
            eventType = "Boda",
            eventDate = date,
            guests = 120,
            requestedServices = RequestedServices
        });
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        using var json = JsonDocument.Parse(await created.Content.ReadAsStringAsync());
        lastQuoteId = json.RootElement.GetProperty("id").GetString()!;
        return client;
    }

    private string lastQuoteId = string.Empty;

    private Task<string> LatestQuoteIdAsync() => Task.FromResult(lastQuoteId);

    /** Simula la respuesta del propietario directamente en el repositorio. */
    private async Task<string> AnswerLatestQuoteAsync(decimal total, decimal? deposit)
    {
        using var scope = factory.Services.CreateScope();
        var quotes = scope.ServiceProvider.GetRequiredService<IQuoteRepository>();
        var quote = (await quotes.FindByIdAsync(lastQuoteId))!;
        var answered = new Quote
        {
            Id = quote.Id,
            ClientId = quote.ClientId,
            ClientName = quote.ClientName,
            TargetType = quote.TargetType,
            TargetId = quote.TargetId,
            TargetName = quote.TargetName,
            EventId = quote.EventId,
            EventType = quote.EventType,
            EventDate = quote.EventDate,
            Guests = quote.Guests,
            RequestedServices = quote.RequestedServices,
            Message = quote.Message,
            Response = new QuoteResponse { Total = total, Deposit = deposit },
            Status = "answered",
            CreatedAt = quote.CreatedAt,
            UpdatedAt = quote.UpdatedAt
        };
        Assert.NotNull(await quotes.UpdateAsync(answered, "sent"));
        return quote.Id;
    }

    private static async Task<HttpResponseMessage> BookAsync(HttpClient client, string quoteId, string date)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new { quoteId, venueId = SeedVenueId, date, hours = 4 })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));
        return await client.SendAsync(request);
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
