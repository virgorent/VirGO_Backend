using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Api.Middleware;
using VirGo.Application.Jobs;
using VirGo.Infrastructure.Jobs;
using Xunit;

namespace VirGo.IntegrationTests;

/// <summary>Spec 099 CA-01 and spec 902 RF-05.</summary>
public sealed class JobsIntegrationTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task CreatingABookingPublishesBookingCreatedWithTheCorrelationId()
    {
        using var client = factory.CreateClient();
        using var login = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());
        var correlationId = Guid.NewGuid().ToString("D");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new
            {
                venueId = "11111111-1111-4111-8111-111111111111",
                date = "2027-06-20",
                hours = 1
            })
        };
        request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));
        request.Headers.Add("X-Correlation-Id", correlationId);
        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var booking = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var bookingId = booking.RootElement.GetProperty("id").GetString();
        var message = Assert.Single(
            factory.Services.GetRequiredService<InMemoryJobPublisher>().Messages,
            job => job.Type == JobTypes.BookingCreated && job.Get("bookingId") == bookingId);
        Assert.Equal(correlationId, message.CorrelationId);
        Assert.Equal(booking.RootElement.GetProperty("folio").GetString(), message.Get("folio"));
        Assert.Equal("2027-06-20", message.Get("date"));
    }

    [Theory]
    [InlineData("Root=1-5759e988-bd862e3fe1be46a994272793;Parent=53995c3f42cd8ad8;Sampled=1", "1-5759e988-bd862e3fe1be46a994272793")]
    [InlineData("Parent=53995c3f42cd8ad8;Root=1-abc;Sampled=0", "1-abc")]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData("Sampled=1", null)]
    public void TraceIdIsTheXRayRoot(string? header, string? expected) =>
        Assert.Equal(expected, TraceIds.RootFrom(header));
}
