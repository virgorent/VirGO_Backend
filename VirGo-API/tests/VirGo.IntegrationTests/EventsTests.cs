using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using VirGo.Application.Abstractions;
using VirGo.Application.Auth;
using VirGo.Domain.Auth;
using Xunit;

namespace VirGo.IntegrationTests;

// Eventos del cliente, invitados y pendientes (specs 043-051, portado del PR #37).
public sealed class EventsTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private const string DemoEventId = "eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee";
    private const string PublishedVenueId = "11111111-1111-4111-8111-111111111111";

    [Fact]
    public async Task DemoClientCanReadTheirEventsWithoutInternalFields()
    {
        using var client = await DemoClientAsync();

        using var response = await client.GetAsync("/api/v1/events");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var eventItem = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray(), item => item.GetProperty("id").GetString() == DemoEventId);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Boda Demo", eventItem.GetProperty("name").GetString());
        Assert.Equal("Boda", eventItem.GetProperty("eventType").GetString());
        Assert.False(eventItem.TryGetProperty("ownerId", out _));
    }

    [Fact]
    public async Task ClientCanCreateAnEventWithPublishedVenueSnapshot()
    {
        using var client = await NewClientAsync(UserRole.Client);

        using var response = await client.PostAsJsonAsync("/api/v1/events", ValidEvent("Evento nuevo", PublishedVenueId));
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("Evento nuevo", body.RootElement.GetProperty("name").GetString());
        Assert.Equal(PublishedVenueId, body.RootElement.GetProperty("venue").GetProperty("venueId").GetString());
        Assert.Equal("VirGo Demo Venue", body.RootElement.GetProperty("venue").GetProperty("name").GetString());
        Assert.False(body.RootElement.TryGetProperty("ownerId", out _));
        Assert.EndsWith($"/api/v1/events/{body.RootElement.GetProperty("id").GetString()}", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task EventCreationRejectsUnknownVenueByField()
    {
        using var client = await NewClientAsync(UserRole.Client);

        using var response = await client.PostAsJsonAsync("/api/v1/events", ValidEvent("Sin salón", Guid.NewGuid().ToString("D")));

        await AssertFieldErrorsAsync(response, "venueId");
    }

    [Fact]
    public async Task EventCreationReportsEveryInvalidField()
    {
        using var client = await NewClientAsync(UserRole.Client);

        using var response = await client.PostAsJsonAsync("/api/v1/events", new
        {
            name = new string('x', 101),
            eventType = "No existe",
            date = "10/09/2027",
            guests = 0,
            budget = -1,
            hours = 0,
            venueId = "no-es-uuid"
        });

        await AssertFieldErrorsAsync(response, "name", "eventType", "date", "guests", "budget", "hours", "venueId");
    }

    [Fact]
    public async Task OwnerCanReadAndUpdateTheirEvent()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);

        using var detail = await client.GetAsync($"/api/v1/events/{id}");
        Assert.Equal(HttpStatusCode.OK, detail.StatusCode);

        using var response = await client.PatchAsJsonAsync($"/api/v1/events/{id}", new
        {
            name = "Evento actualizado",
            eventType = "Cumpleaños",
            date = "2027-10-11",
            guests = 60,
            budget = 35000.5,
            hours = 6
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Evento actualizado", body.RootElement.GetProperty("name").GetString());
        Assert.Equal("Cumpleaños", body.RootElement.GetProperty("eventType").GetString());
        Assert.Equal(60, body.RootElement.GetProperty("guests").GetInt32());
        Assert.Equal(35000.5m, body.RootElement.GetProperty("budget").GetDecimal());
        Assert.Equal(JsonValueKind.Null, body.RootElement.GetProperty("venue").ValueKind);
    }

    [Fact]
    public async Task EventUpdateRejectsIncompleteBodyByField()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);

        using var response = await client.PatchAsJsonAsync($"/api/v1/events/{id}", new { name = new string('x', 1500) });

        await AssertFieldErrorsAsync(response, "name", "eventType", "date", "guests", "budget", "hours");
    }

    [Fact]
    public async Task AnotherClientGetsNotFoundForEveryEventRoute()
    {
        using var owner = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(owner);
        var guestId = await CreateGuestAsync(owner, id);
        var taskId = await CreateTaskAsync(owner, id);
        using var intruder = await NewClientAsync(UserRole.Client);

        var responses = new List<HttpResponseMessage>
        {
            await intruder.GetAsync($"/api/v1/events/{id}"),
            await intruder.PatchAsJsonAsync($"/api/v1/events/{id}", ValidEvent("Robado")),
            await intruder.GetAsync($"/api/v1/events/{id}/guests"),
            await intruder.PostAsJsonAsync($"/api/v1/events/{id}/guests", ValidGuest()),
            await intruder.PatchAsJsonAsync($"/api/v1/events/{id}/guests/{guestId}", ValidGuest()),
            await intruder.DeleteAsync($"/api/v1/events/{id}/guests/{guestId}"),
            await intruder.GetAsync($"/api/v1/events/{id}/tasks"),
            await intruder.PostAsJsonAsync($"/api/v1/events/{id}/tasks", ValidTask()),
            await intruder.PatchAsJsonAsync($"/api/v1/events/{id}/tasks/{taskId}", ValidTask()),
            await intruder.DeleteAsync($"/api/v1/events/{id}/tasks/{taskId}")
        };

        Assert.All(responses, response => Assert.Equal(HttpStatusCode.NotFound, response.StatusCode));
        using var list = await intruder.GetAsync("/api/v1/events");
        using var body = JsonDocument.Parse(await list.Content.ReadAsStringAsync());
        Assert.Empty(body.RootElement.GetProperty("data").EnumerateArray());

        using var guests = await owner.GetAsync($"/api/v1/events/{id}/guests");
        using var guestsBody = JsonDocument.Parse(await guests.Content.ReadAsStringAsync());
        Assert.Equal("Invitado nuevo", Assert.Single(guestsBody.RootElement.GetProperty("data").EnumerateArray()).GetProperty("name").GetString());
        foreach (var response in responses) response.Dispose();
    }

    [Theory]
    [InlineData("11111111-1111-4111-8111-111111111111")]
    [InlineData("no-es-uuid")]
    public async Task UnknownOrInvalidEventIdReturnsNotFound(string id)
    {
        using var client = await NewClientAsync(UserRole.Client);

        using var response = await client.GetAsync($"/api/v1/events/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(UserRole.Owner)]
    [InlineData(UserRole.Provider)]
    public async Task NonClientRolesAreForbidden(UserRole role)
    {
        using var client = await NewClientAsync(role);

        using var list = await client.GetAsync("/api/v1/events");
        using var create = await client.PostAsJsonAsync("/api/v1/events", ValidEvent("No permitido"));

        Assert.Equal(HttpStatusCode.Forbidden, list.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, create.StatusCode);
    }

    [Fact]
    public async Task DemoClientCanListTheSeededGuest()
    {
        using var client = await DemoClientAsync();

        using var response = await client.GetAsync($"/api/v1/events/{DemoEventId}/guests");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(body.RootElement.GetProperty("data").EnumerateArray(), guest => guest.GetProperty("name").GetString() == "Invitado Demo");
    }

    [Fact]
    public async Task OwnerCanCreateUpdateAndDeleteAGuest()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);
        var guestId = await CreateGuestAsync(client, id);

        using var update = await client.PatchAsJsonAsync($"/api/v1/events/{id}/guests/{guestId}", new
        {
            name = "Invitada editada",
            phone = "+524420000003",
            companions = 0,
            status = "confirmed"
        });
        using var updated = JsonDocument.Parse(await update.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        Assert.Equal("Invitada editada", updated.RootElement.GetProperty("name").GetString());
        Assert.Equal("confirmed", updated.RootElement.GetProperty("status").GetString());
        Assert.Equal(JsonValueKind.Null, updated.RootElement.GetProperty("table").ValueKind);

        using var delete = await client.DeleteAsync($"/api/v1/events/{id}/guests/{guestId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
        using var again = await client.DeleteAsync($"/api/v1/events/{id}/guests/{guestId}");
        Assert.Equal(HttpStatusCode.NotFound, again.StatusCode);
    }

    [Fact]
    public async Task GuestCreationReportsInvalidFields()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);

        using var response = await client.PostAsJsonAsync($"/api/v1/events/{id}/guests", new
        {
            name = "",
            phone = new string('1', 17),
            companions = -1,
            status = "unknown",
            table = new string('m', 21)
        });

        await AssertFieldErrorsAsync(response, "name", "phone", "companions", "status", "table");
    }

    [Fact]
    public async Task GuestFromAnotherEventCannotBeEdited()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var first = await CreateEventAsync(client);
        var second = await CreateEventAsync(client);
        var guestId = await CreateGuestAsync(client, first);

        using var response = await client.PatchAsJsonAsync($"/api/v1/events/{second}/guests/{guestId}", ValidGuest());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCanCreateUpdateAndDeleteATask()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);
        var taskId = await CreateTaskAsync(client, id);

        using var update = await client.PatchAsJsonAsync($"/api/v1/events/{id}/tasks/{taskId}", new { title = "Pagar anticipo", dueDate = "2027-03-01", done = true });
        using var updated = JsonDocument.Parse(await update.Content.ReadAsStringAsync());
        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        Assert.True(updated.RootElement.GetProperty("done").GetBoolean());
        Assert.False(updated.RootElement.GetProperty("suggested").GetBoolean());

        using var list = await client.GetAsync($"/api/v1/events/{id}/tasks");
        using var listBody = JsonDocument.Parse(await list.Content.ReadAsStringAsync());
        Assert.Equal("Pagar anticipo", Assert.Single(listBody.RootElement.GetProperty("data").EnumerateArray()).GetProperty("title").GetString());

        using var delete = await client.DeleteAsync($"/api/v1/events/{id}/tasks/{taskId}");
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task EditingASuggestedTaskKeepsItSuggested()
    {
        using var client = await DemoClientAsync();
        const string suggestedTaskId = "f2222222-2222-4222-8222-222222222222";

        using var response = await client.PatchAsJsonAsync($"/api/v1/events/{DemoEventId}/tasks/{suggestedTaskId}", new { title = "Confirmar proveedores", dueDate = "2027-02-01", done = false });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(body.RootElement.GetProperty("suggested").GetBoolean());
    }

    [Fact]
    public async Task TaskCreationReportsInvalidFields()
    {
        using var client = await NewClientAsync(UserRole.Client);
        var id = await CreateEventAsync(client);

        using var response = await client.PostAsJsonAsync($"/api/v1/events/{id}/tasks", new { title = new string('t', 151), dueDate = "mañana" });

        await AssertFieldErrorsAsync(response, "title", "dueDate", "done");
    }

    private static object ValidEvent(string name, string? venueId = null) => new
    {
        name,
        eventType = "Boda",
        date = "2027-09-10",
        guests = 80,
        budget = 60000,
        hours = 6,
        venueId
    };

    private static object ValidGuest() => new { name = "Invitado nuevo", phone = "+524420000002", companions = 2, status = "pending", table = "Mesa 2" };

    private static object ValidTask() => new { title = "Reservar banquete", dueDate = "2027-02-15", done = false };

    private static async Task<string> CreateEventAsync(HttpClient client)
    {
        using var response = await client.PostAsJsonAsync("/api/v1/events", ValidEvent("Evento original"));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task<string> CreateGuestAsync(HttpClient client, string eventId)
    {
        using var response = await client.PostAsJsonAsync($"/api/v1/events/{eventId}/guests", ValidGuest());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(eventId, body.RootElement.GetProperty("eventId").GetString());
        return body.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task<string> CreateTaskAsync(HttpClient client, string eventId)
    {
        using var response = await client.PostAsJsonAsync($"/api/v1/events/{eventId}/tasks", ValidTask());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.False(body.RootElement.GetProperty("suggested").GetBoolean());
        return body.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task AssertFieldErrorsAsync(HttpResponseMessage response, params string[] fields)
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var errors = body.RootElement.GetProperty("errors");
        foreach (var field in fields)
        {
            Assert.True(errors.TryGetProperty(field, out _), field);
        }
    }

    private async Task<HttpClient> DemoClientAsync()
    {
        var client = factory.CreateClient();
        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json.RootElement.GetProperty("accessToken").GetString());
        return client;
    }

    private async Task<HttpClient> NewClientAsync(UserRole role)
    {
        var email = $"events-{role.ToString().ToLowerInvariant()}-{Guid.NewGuid():N}@virgo.mx";
        using (var scope = factory.Services.CreateScope())
        {
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
        }

        var client = factory.CreateClient();
        using var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password = "Strong-password-123" });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json.RootElement.GetProperty("accessToken").GetString());
        return client;
    }
}
