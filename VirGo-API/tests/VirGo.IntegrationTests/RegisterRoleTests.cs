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

public sealed class RegisterRoleTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Theory]
    [InlineData("owner")]
    [InlineData("provider")]
    [InlineData("client")]
    public async Task RegisterCreatesTheChosenPublicRole(string role)
    {
        using var client = factory.CreateClient();
        var email = $"{role}-{Guid.NewGuid():N}@virgo.mx";

        using var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Nueva Persona",
            email,
            password = "Strong-password-123",
            phone = "+525500000000",
            role
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(role, json.RootElement.GetProperty("role").GetString());
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("Owner")]
    [InlineData("superuser")]
    public async Task RegisterRejectsRolesThatAreNotPublic(string role)
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Nueva Persona",
            email = $"bad-role-{Guid.NewGuid():N}@virgo.mx",
            password = "Strong-password-123",
            phone = "+525500000000",
            role
        });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.True(json.RootElement.GetProperty("errors").TryGetProperty("role", out _));
    }

    [Fact]
    public async Task RegisteredOwnerCanLogInAndCreateAVenue()
    {
        using var client = factory.CreateClient();
        var email = $"owner-flow-{Guid.NewGuid():N}@virgo.mx";
        using var register = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Dueña Nueva",
            email,
            password = "Strong-password-123",
            phone = "+525500000000",
            role = "owner"
        });
        Assert.Equal(HttpStatusCode.Created, register.StatusCode);
        using var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "Strong-password-123" });
        using var loginJson = JsonDocument.Parse(await login.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginJson.RootElement.GetProperty("accessToken").GetString());

        using var mine = await client.GetAsync("/api/v1/venues/mine");

        Assert.Equal(HttpStatusCode.OK, mine.StatusCode);
    }
}
