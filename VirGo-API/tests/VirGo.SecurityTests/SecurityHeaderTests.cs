using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace VirGo.SecurityTests;

public sealed class SecurityHeaderTests(SecurityApiFactory factory) : IClassFixture<SecurityApiFactory>
{
    [Fact]
    public async Task HealthHasSecurityHeaders()
    {
        using var client = factory.CreateClient();
        using var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("nosniff", response.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("DENY", response.Headers.GetValues("X-Frame-Options").Single());
        Assert.Equal("no-referrer", response.Headers.GetValues("Referrer-Policy").Single());
    }

    [Fact]
    public async Task DisallowedOriginDoesNotReceiveCorsPermission()
    {
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", "https://not-allowed.example");

        using var response = await client.SendAsync(request);

        Assert.DoesNotContain("Access-Control-Allow-Origin", response.Headers.Select(header => header.Key));
    }
}

public sealed class ProductionCorsTests(ProductionCorsApiFactory factory) : IClassFixture<ProductionCorsApiFactory>
{
    [Fact]
    public async Task ProductionDoesNotAllowLocalhostOrigin()
    {
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", "http://localhost:3000");

        using var response = await client.SendAsync(request);

        Assert.DoesNotContain("Access-Control-Allow-Origin", response.Headers.Select(header => header.Key));
    }
}

public sealed class ProductionCorsApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        builder.UseSetting("Persistence:Provider", "MongoDb");
        builder.UseSetting("Persistence:ConnectionString", "mongodb://127.0.0.1:27017/virgo");
        builder.UseSetting("Jwt:SigningKey", "TestingOnlySigningKeyMustBeAtLeast32Characters");
    }
}
