using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Domain.Auth;
using VirGo.Domain.Marketplace;
using Xunit;

namespace VirGo.IntegrationTests;

public sealed class BookingWithQuoteTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] RequestedServices = ["Banquete"];

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
