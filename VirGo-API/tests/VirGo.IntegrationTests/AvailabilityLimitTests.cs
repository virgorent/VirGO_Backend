using System.Net;
using System.Text.Json;
using Xunit;

namespace VirGo.IntegrationTests;

/// <summary>
/// Issue #50: spec 031 RF-03 define <c>from</c> y <c>to</c> inclusivos con un máximo de 366 días.
/// </summary>
public sealed class AvailabilityLimitTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private const string PublishedVenueId = "11111111-1111-4111-8111-111111111111";

    [Theory]
    [InlineData("2027-01-01", "2027-01-01")] // 1 día: from == to
    [InlineData("2027-01-01", "2027-12-31")] // 365 días
    [InlineData("2027-01-01", "2028-01-01")] // 366 días: el máximo
    [InlineData("2028-01-01", "2028-12-31")] // 366 días dentro de un año bisiesto
    public async Task RangesUpTo366InclusiveDaysAreAccepted(string from, string to)
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/api/v1/venues/{PublishedVenueId}/availability?from={from}&to={to}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData("2027-01-01", "2028-01-02")] // 367 días
    [InlineData("2028-01-01", "2029-01-01")] // 367 días que cruzan un año bisiesto
    [InlineData("2027-01-01", "2029-01-01")] // muy por encima del máximo
    public async Task RangesLongerThan366InclusiveDaysAreRejected(string from, string to)
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/api/v1/venues/{PublishedVenueId}/availability?from={from}&to={to}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("range", out _));
    }

    [Fact]
    public async Task MaximumRangeStillReturnsTheDatesInside()
    {
        using var client = factory.CreateClient();

        // El bloqueo semilla del salón está el 2027-05-20.
        using var response = await client.GetAsync($"/api/v1/venues/{PublishedVenueId}/availability?from=2027-01-01&to=2028-01-01");
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(
            json.RootElement.GetProperty("data").EnumerateArray(),
            item => item.GetProperty("date").GetString() == "2027-05-20");
    }
}
