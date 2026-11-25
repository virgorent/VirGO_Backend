using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Jobs;
using VirGo.Domain.Notifications;
using Xunit;

namespace VirGo.IntegrationTests;

public sealed class AuthTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task LoginThenLogoutInvalidatesOnlyThatSession()
    {
        using var client = factory.CreateClient();
        using var loginResponse = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        var loginBody = await loginResponse.Content.ReadAsStringAsync();
        using var loginJson = JsonDocument.Parse(loginBody);
        var accessToken = loginJson.RootElement.GetProperty("accessToken").GetString();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(accessToken));

        using var authorizedRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/_diagnostics/secure");
        authorizedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var authorizedResponse = await client.SendAsync(authorizedRequest);
        Assert.Equal(HttpStatusCode.OK, authorizedResponse.StatusCode);

        using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/logout");
        logoutRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var logoutResponse = await client.SendAsync(logoutRequest);
        Assert.Equal(HttpStatusCode.NoContent, logoutResponse.StatusCode);

        using var rejectedRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/_diagnostics/secure");
        rejectedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        using var rejectedResponse = await client.SendAsync(rejectedRequest);
        Assert.Equal(HttpStatusCode.Unauthorized, rejectedResponse.StatusCode);
    }

    [Fact]
    public async Task UnknownEmailReturnsGenericUnauthorizedResponse()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "unknown@virgo.mx", password = "wrong-password" });
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("Correo o contraseña incorrectos", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LoginNormalizesEmailCaseAndWhitespace()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "  DEMO@VIRGO.MX  ", password = "VirGo-local-only-password" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task LoginRejectsInvalidInputWithFieldErrors()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "not-an-email", password = "" });
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("email", body, StringComparison.Ordinal);
        Assert.Contains("password", body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task DeleteAccountAnonymizesUserAndRevokesAllSessions()
    {
        using var client = factory.CreateClient();
        var email = $"delete-{Guid.NewGuid():N}@virgo.test";
        const string password = "Password-Segura-1";

        using var register = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Cuenta por eliminar",
            email,
            password,
            phone = "+524421234567"
        });
        Assert.Equal(HttpStatusCode.Created, register.StatusCode);

        var firstToken = await LoginAsync(client, email, password);
        var secondToken = await LoginAsync(client, email, password);
        using var delete = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/me");
        delete.Headers.Authorization = new AuthenticationHeaderValue("Bearer", firstToken);

        using var deleteResponse = await client.SendAsync(delete);

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, await StatusAsync(client, firstToken));
        Assert.Equal(HttpStatusCode.Unauthorized, await StatusAsync(client, secondToken));

        using var login = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email, password });
        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
    }

    [Fact]
    public async Task PublicProfileReturnsOnlyPublicFieldsWithoutAuthentication()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/v1/users/8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1/public");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = body.RootElement;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1", root.GetProperty("id").GetString());
        Assert.Equal("VirGo Demo", root.GetProperty("name").GetString());
        Assert.Equal("client", root.GetProperty("role").GetString());
        Assert.False(root.GetProperty("isVerified").GetBoolean());
        Assert.DoesNotContain("email", root.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("phone", root.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("passwordHash", root.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PublicProfileReturnsNotFoundForUnknownUser()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/v1/users/11111111-1111-4111-8111-111111111111/public");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PublicProfileTreatsInjectionPayloadAsAnUnknownId()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/v1/users/%24ne%3Anull/public");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchProfileUpdatesAllowedFieldsAndPreservesProtectedFields()
    {
        using var client = factory.CreateClient();
        var email = $"profile-{Guid.NewGuid():N}@virgo.test";
        const string password = "Password-Segura-1";

        using var register = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Perfil original",
            email,
            password,
            phone = "+524421234567"
        });
        Assert.Equal(HttpStatusCode.Created, register.StatusCode);

        var token = await LoginAsync(client, email, password);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            name = "Perfil actualizado",
            phone = "+524421234568",
            city = "Queretaro",
            defaultGuests = 4,
            role = "provider",
            isVerified = true
        });

        using var response = await client.SendAsync(request);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = body.RootElement;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Perfil actualizado", root.GetProperty("name").GetString());
        Assert.Equal("+524421234568", root.GetProperty("phone").GetString());
        Assert.Equal("Queretaro", root.GetProperty("city").GetString());
        Assert.Equal(4, root.GetProperty("defaultGuests").GetInt32());
        Assert.Equal(email, root.GetProperty("email").GetString());
        Assert.Equal("client", root.GetProperty("role").GetString());
        Assert.False(root.GetProperty("isVerified").GetBoolean());
    }

    [Fact]
    public async Task PatchProfileRejectsInvalidFields()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { name = new string('a', 101), defaultGuests = -1 });

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PatchProfileRejects1500CharacterName()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/users/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { name = new string('a', 1500) });

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ChangePasswordReplacesThePreviousCredential()
    {
        using var client = factory.CreateClient();
        var email = $"password-{Guid.NewGuid():N}@virgo.test";
        const string oldPassword = "Password-Segura-1";
        const string newPassword = "Password-Nueva-2";

        using var register = await client.PostAsJsonAsync("/api/v1/auth/register", new
        {
            name = "Cambio de contraseña",
            email,
            password = oldPassword,
            phone = "+524421234567"
        });
        Assert.Equal(HttpStatusCode.Created, register.StatusCode);

        var token = await LoginAsync(client, email, oldPassword);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/users/me/password");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { currentPassword = oldPassword, newPassword });

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using var oldLogin = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = oldPassword });
        using var newLogin = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = newPassword });
        Assert.Equal(HttpStatusCode.Unauthorized, oldLogin.StatusCode);
        Assert.Equal(HttpStatusCode.OK, newLogin.StatusCode);
    }

    [Fact]
    public async Task ChangePasswordRejectsAnIncorrectCurrentCredential()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/users/me/password");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = JsonContent.Create(new { currentPassword = "incorrecta", newPassword = "Password-Nueva-2" });

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task NoticesReturnsOnlyTheAuthenticatedUsersCollection()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/notices");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.SendAsync(request);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(body.RootElement.TryGetProperty("data", out var data));
        Assert.Equal(JsonValueKind.Array, data.ValueKind);
    }

    [Fact]
    public async Task AuthenticatedUserCanMarkTheirNoticeAsRead()
    {
        const string userId = "8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1";
        var noticeId = $"notice-{Guid.NewGuid():N}";
        var now = DateTime.UtcNow;
        using var scope = factory.Services.CreateScope();
        var notices = scope.ServiceProvider.GetRequiredService<INoticeRepository>();
        await notices.AddIfAbsentAsync(new Notice
        {
            Id = noticeId,
            UserId = userId,
            Kind = "reminder",
            Title = "Aviso de prueba",
            Body = "Contenido de prueba",
            Link = "/app",
            Read = false,
            CreatedAt = now,
            PurgeAt = now.AddDays(30)
        });

        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Patch, $"/api/v1/notices/{noticeId}/read");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.SendAsync(request);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(noticeId, body.RootElement.GetProperty("id").GetString());
        Assert.True(body.RootElement.GetProperty("read").GetBoolean());
    }

    [Fact]
    public async Task UserCannotMarkAnotherUsersNoticeAsRead()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Patch, "/api/v1/notices/not-owned-by-demo/read");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticatedUserCanMarkAllTheirUnreadNoticesAsRead()
    {
        const string userId = "8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1";
        var now = DateTime.UtcNow;
        using var scope = factory.Services.CreateScope();
        var notices = scope.ServiceProvider.GetRequiredService<INoticeRepository>();
        for (var index = 0; index < 2; index++)
        {
            await notices.AddIfAbsentAsync(new Notice
            {
                Id = $"notice-all-{Guid.NewGuid():N}",
                UserId = userId,
                Kind = "reminder",
                Title = "Aviso de prueba",
                Body = "Contenido de prueba",
                Link = "/app",
                Read = false,
                CreatedAt = now.AddMinutes(index),
                PurgeAt = now.AddDays(30)
            });
        }

        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsync("/api/v1/notices/read-all", content: null);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        using var second = await client.PostAsync("/api/v1/notices/read-all", content: null);
        using var secondBody = JsonDocument.Parse(await second.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, body.RootElement.GetProperty("updatedCount").GetInt64());
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);
        Assert.Equal(0, secondBody.RootElement.GetProperty("updatedCount").GetInt64());
    }

    [Fact]
    public async Task AuthenticatedUserCanReadTheirAssistantHistory()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/assistant/messages");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var message = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("assistant", message.GetProperty("role").GetString());
        Assert.False(string.IsNullOrWhiteSpace(message.GetProperty("text").GetString()));
        Assert.DoesNotContain("userId", message.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AnyoneCanSubmitAContactMessage()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/contact", new
        {
            name = "Persona visitante",
            email = "visitante@example.com",
            message = "Necesito ayuda para planear un evento."
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.True(Guid.TryParse(body.RootElement.GetProperty("id").GetString(), out _));
    }

    [Fact]
    public async Task ContactRejectsOversizedInputAsProblemDetails()
    {
        using var client = factory.CreateClient();

        using var response = await client.PostAsJsonAsync("/api/v1/contact", new
        {
            name = new string('a', 1500),
            email = "visitante@example.com",
            message = "Mensaje válido."
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PublicProviderCatalogSupportsFiltersAndHidesInternalFields()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/providers?category=Catering&zone=Centro&rating=4");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var provider = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Catering VirGo", provider.GetProperty("businessName").GetString());
        Assert.DoesNotContain("userId", provider.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("status", provider.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ProviderCatalogRejectsAnInvalidPriceRange()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/providers?minPrice=1000&maxPrice=10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PublicProviderDetailReturnsPublishedProviderWithoutInternalFields()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/providers/aaaaaaaa-aaaa-4aaa-8aaa-aaaaaaaaaaaa");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var provider = body.RootElement;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Catering VirGo", provider.GetProperty("businessName").GetString());
        Assert.DoesNotContain("userId", provider.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("status", provider.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PublicProviderDetailReturnsNotFoundForUnknownProvider()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/api/v1/providers/11111111-1111-4111-8111-111111111111");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task LogoutKeepsASecondSessionActive()
    {
        using var client = factory.CreateClient();
        var firstToken = await LoginAsync(client);
        var secondToken = await LoginAsync(client);

        using var logout = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/logout");
        logout.Headers.Authorization = new AuthenticationHeaderValue("Bearer", firstToken);
        Assert.Equal(HttpStatusCode.NoContent, (await client.SendAsync(logout)).StatusCode);

        using var secondRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/_diagnostics/secure");
        secondRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", secondToken);
        Assert.Equal(HttpStatusCode.OK, (await client.SendAsync(secondRequest)).StatusCode);
    }

    private static async Task<string> LoginAsync(HttpClient client)
    {
        return await LoginAsync(client, "demo@virgo.mx", "VirGo-local-only-password");
    }

    private static async Task<string> LoginAsync(HttpClient client, string email, string password)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email, password });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }

    private static async Task<HttpStatusCode> StatusAsync(HttpClient client, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/_diagnostics/secure");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request);
        return response.StatusCode;
    }
}
