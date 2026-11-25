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

public sealed class VerticalSliceTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private static readonly string[] RequestedServices = ["Banquete"];
    private static readonly string[] ProviderTestZones = ["Centro"];
    private static readonly string[] ProviderTestIncludes = ["Menú"];
    private static readonly string[] NewVenueCategories = ["Salon"];
    private static readonly double[] NewVenueCoordinates = [20.7, -100.45];
    private static readonly double[] InvalidVenueCoordinates = [20.6, -100.4];

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
    public async Task ProviderCanReadOwnProfile()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider, "bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/providers/me");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Catering VirGo", body.RootElement.GetProperty("businessName").GetString());
        Assert.Equal("published", body.RootElement.GetProperty("status").GetString());
        Assert.DoesNotContain("userId", body.RootElement.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ClientCannotReadProviderOwnProfile()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/providers/me");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ProviderCanCreateOnlyOneDraftProfile()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var request = new
        {
            businessName = "Proveedor nuevo",
            category = "Catering",
            description = "Servicio para eventos sociales.",
            zones = new[] { "Centro" },
            packages = new[] { new { id = "basico", name = "Paquete básico", price = 500, includes = new[] { "Menú" } } },
            gallery = new[] { "https://example.com/provider.jpg" },
            pricing = "per_event",
            responseTime = "24 horas"
        };

        using var first = await client.PostAsJsonAsync("/api/v1/providers", request);
        using var firstBody = JsonDocument.Parse(await first.Content.ReadAsStringAsync());
        using var second = await client.PostAsJsonAsync("/api/v1/providers", request);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal("draft", firstBody.RootElement.GetProperty("status").GetString());
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task ProviderCreationRejectsOversizedDescription()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var zones = new[] { "Centro" };
        var packages = new[] { new { id = "basico", name = "Paquete", price = 500, includes = new[] { "Menú" } } };

        using var response = await client.PostAsJsonAsync("/api/v1/providers", new
        {
            businessName = "Proveedor nuevo",
            category = "Catering",
            description = new string('x', 1500),
            zones,
            packages,
            gallery = Array.Empty<string>(),
            pricing = "per_event",
            responseTime = "24 horas"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ProviderCanUpdateOwnProfileWithoutChangingStatusOrRating()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var request = new
        {
            businessName = "Proveedor actualizado",
            category = "Catering",
            description = "Descripción actualizada.",
            zones = new[] { "Centro" },
            packages = new[] { new { id = "premium", name = "Premium", price = 900, includes = new[] { "Menú completo" } } },
            gallery = new[] { "https://example.com/updated.jpg" },
            pricing = "per_event",
            responseTime = "12 horas"
        };
        using var create = await client.PostAsJsonAsync("/api/v1/providers", request);
        using var response = await client.PatchAsJsonAsync("/api/v1/providers/me", request);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Proveedor actualizado", body.RootElement.GetProperty("businessName").GetString());
        Assert.Equal("draft", body.RootElement.GetProperty("status").GetString());
        Assert.Equal(0, body.RootElement.GetProperty("rating").GetProperty("count").GetInt32());
    }

    [Fact]
    public async Task ProviderCanReadOnlyTheirOwnGigs()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider, "bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/providers/me/gigs");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var gig = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Boda Demo", gig.GetProperty("eventName").GetString());
        Assert.DoesNotContain("providerId", gig.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ProviderCanCreateAGigAndOverlapsAreRejected()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider, "bbbbbbbb-bbbb-4bbb-8bbb-bbbbbbbbbbbb");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var request = new
        {
            eventName = "Evento nuevo",
            clientName = "Cliente nuevo",
            place = "Juriquilla",
            date = "2027-05-16",
            start = "10:00",
            end = "12:00",
            amount = 3000,
            quoteId = "dddddddd-dddd-4ddd-8ddd-dddddddddddd"
        };

        using var first = await client.PostAsJsonAsync("/api/v1/providers/me/gigs", request);
        using var second = await client.PostAsJsonAsync("/api/v1/providers/me/gigs", request);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task GigCreationRejectsInvalidTimeRange()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/providers/me/gigs", new
        {
            eventName = "Evento",
            clientName = "Cliente",
            place = "Centro",
            date = "2027-05-17",
            start = "18:00",
            end = "17:00"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ProviderCanUpdateTheirOwnGig()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Provider);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var profile = await client.PostAsJsonAsync("/api/v1/providers", new
        {
            businessName = "Proveedor de pruebas",
            category = "Catering",
            description = "Servicio para eventos sociales.",
            zones = ProviderTestZones,
            packages = new[] { new { id = "basico", name = "Paquete", price = 500, includes = ProviderTestIncludes } },
            gallery = Array.Empty<string>(),
            pricing = "per_event",
            responseTime = "24 horas"
        });
        Assert.Equal(HttpStatusCode.Created, profile.StatusCode);
        var createRequest = new
        {
            eventName = "Evento original",
            clientName = "Cliente",
            place = "Centro",
            date = "2027-06-01",
            start = "10:00",
            end = "12:00"
        };
        using var create = await client.PostAsJsonAsync("/api/v1/providers/me/gigs", createRequest);
        using var createdBody = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var id = createdBody.RootElement.GetProperty("id").GetString();

        using var response = await client.PatchAsJsonAsync($"/api/v1/providers/me/gigs/{id}", new
        {
            eventName = "Evento actualizado",
            clientName = "Cliente",
            place = "Juriquilla",
            date = "2027-06-01",
            start = "11:00",
            end = "13:00"
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Evento actualizado", body.RootElement.GetProperty("eventName").GetString());
    }

    [Fact]
    public async Task AuthenticatedUserCanReadTheirEvents()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/events");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var eventItem = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray(), item => item.GetProperty("name").GetString() == "Boda Demo");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Boda Demo", eventItem.GetProperty("name").GetString());
        Assert.Equal("Boda", eventItem.GetProperty("eventType").GetString());
    }

    [Fact]
    public async Task AuthenticatedUserCanCreateAnEventWithPublishedVenueSnapshot()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/events", new
        {
            name = "Evento nuevo",
            eventType = "Boda",
            date = "2027-09-10",
            guests = 80,
            budget = 60000,
            hours = 6,
            venueId = "11111111-1111-4111-8111-111111111111"
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("Evento nuevo", body.RootElement.GetProperty("name").GetString());
        Assert.Equal("VirGo Demo Venue", body.RootElement.GetProperty("venue").GetProperty("name").GetString());
    }

    [Fact]
    public async Task AuthenticatedUserCanReadTheirEventDetail()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/events/eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Boda Demo", body.RootElement.GetProperty("name").GetString());
    }

    [Fact]
    public async Task EventDetailReturnsNotFoundForUnknownOrUnauthorizedResource()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/events/11111111-1111-4111-8111-111111111111");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task EventCreationRejectsInvalidEventType()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/events", new
        {
            name = "Evento inválido",
            eventType = "No existe",
            date = "2027-09-10",
            guests = 80,
            budget = 60000,
            hours = 6
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task EventOwnerCanUpdateTheirEvent()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var create = await client.PostAsJsonAsync("/api/v1/events", new
        {
            name = "Evento original",
            eventType = "Boda",
            date = "2027-10-10",
            guests = 40,
            budget = 30000,
            hours = 5
        });
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var id = created.RootElement.GetProperty("id").GetString();

        using var response = await client.PatchAsJsonAsync($"/api/v1/events/{id}", new
        {
            name = "Evento actualizado",
            eventType = "Boda",
            date = "2027-10-11",
            guests = 60,
            budget = 35000,
            hours = 6
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Evento actualizado", body.RootElement.GetProperty("name").GetString());
        Assert.Equal(60, body.RootElement.GetProperty("guests").GetInt32());
    }

    [Fact]
    public async Task EventUpdateRejectsInvalidData()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PatchAsJsonAsync("/api/v1/events/eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee", new { name = new string('x', 1500) });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task EventOwnerCanListTheirGuests()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/events/eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee/guests");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var guest = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Invitado Demo", guest.GetProperty("name").GetString());
    }

    [Fact]
    public async Task EventOwnerCanCreateAGuest()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/events/eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee/guests", new
        {
            name = "Invitado nuevo",
            phone = "+524420000002",
            companions = 2,
            status = "pending",
            table = "Mesa 2"
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("Invitado nuevo", body.RootElement.GetProperty("name").GetString());
    }

    [Fact]
    public async Task GuestCreationRejectsInvalidStatus()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/events/eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee/guests", new { name = "Invitado", phone = "123", companions = 0, status = "unknown" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCanReadOnlyTheirVenueCollection()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner, "22222222-2222-4222-8222-222222222222");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/venues/mine");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var venues = body.RootElement.GetProperty("data").EnumerateArray().ToArray();
        Assert.Contains(venues, venue => venue.GetProperty("id").GetString() == "11111111-1111-4111-8111-111111111111");
        Assert.Equal("published", venues.Single(venue => venue.GetProperty("id").GetString() == "11111111-1111-4111-8111-111111111111").GetProperty("status").GetString());
    }

    [Fact]
    public async Task ClientCannotReadOwnerVenueCollection()
    {
        using var client = factory.CreateClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.GetAsync("/api/v1/venues/mine");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task OwnerCanCreateADraftVenueWithoutControllingServerFields()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner, "33333333-3333-4333-8333-333333333333");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "Salon Nuevo",
            shortName = "Nuevo",
            description = "Un salon nuevo para pruebas de creacion.",
            address = "Juriquilla, Querétaro",
            zone = "Juriquilla",
            location = new { type = "Point", coordinates = NewVenueCoordinates },
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            photos = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>(),
            rating = new { average = 5, count = 99 },
            verified = true,
            status = "published"
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("draft", body.RootElement.GetProperty("status").GetString());
        Assert.Equal(0, body.RootElement.GetProperty("rating").GetProperty("count").GetInt32());
    }

    [Fact]
    public async Task OwnerCanCreateAVenueWithoutLocationOrPhotos()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "Salon Sin Fotos",
            shortName = "Sin Fotos",
            description = "Un salon creado sin mapa ni fotos para el MVP.",
            address = "Centro, Querétaro",
            zone = "Centro",
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>()
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Empty(body.RootElement.GetProperty("photos").EnumerateArray());
        Assert.Equal("draft", body.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task OwnerCanPublishTheirDraftVenue()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var create = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "Salon Publicable",
            shortName = "Publicable",
            description = "Un salon creado para probar publicación directa.",
            address = "Centro, Querétaro",
            zone = "Centro",
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            photos = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>()
        });
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var venueId = created.RootElement.GetProperty("id").GetString();

        using var publish = await client.PostAsync($"/api/v1/venues/{venueId}/publish", null);
        using var body = JsonDocument.Parse(await publish.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        Assert.Equal(venueId, body.RootElement.GetProperty("id").GetString());
        Assert.Equal("published", body.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task VenueCreationRejectsOversizedInputAsProblemDetails()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner, "44444444-4444-4444-8444-444444444444");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = new string('a', 1500),
            shortName = "Nuevo",
            description = "Un salon nuevo para pruebas de validacion.",
            address = "Centro, Querétaro",
            zone = "Centro",
            location = new { type = "Point", coordinates = InvalidVenueCoordinates },
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            photos = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>()
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task OwnerCanPatchTheirVenueAndServerFieldsRemainProtected()
    {
        using var client = factory.CreateClient();
        var ownerId = "55555555-5555-4555-8555-555555555555";
        var token = await CreateUserTokenAsync(UserRole.Owner, ownerId);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var create = await client.PostAsJsonAsync("/api/v1/venues", new
        {
            name = "Salon Editable",
            shortName = "Editable",
            description = "Un salon creado para probar actualizaciones parciales.",
            address = "Centro, Querétaro",
            zone = "Centro",
            location = new { type = "Point", coordinates = InvalidVenueCoordinates },
            capacity = 80,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            photos = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>()
        });
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var venueId = created.RootElement.GetProperty("id").GetString();

        using var patch = await client.PatchAsJsonAsync($"/api/v1/venues/{venueId}", new
        {
            name = "Salon Editado",
            rating = new { average = 5, count = 100 },
            status = "published"
        });
        using var body = JsonDocument.Parse(await patch.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, patch.StatusCode);
        Assert.Equal("Salon Editado", body.RootElement.GetProperty("name").GetString());
        Assert.Equal("draft", body.RootElement.GetProperty("status").GetString());
        Assert.Equal(0, body.RootElement.GetProperty("rating").GetProperty("count").GetInt32());
    }

    [Fact]
    public async Task OwnerCannotPatchAnotherOwnersVenue()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner, "66666666-6666-4666-8666-666666666666");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PatchAsJsonAsync(
            "/api/v1/venues/11111111-1111-4111-8111-111111111111",
            new { name = "No autorizado" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task VenuePatchRejectsOversizedInputAsProblemDetails()
    {
        using var client = factory.CreateClient();
        var token = await CreateUserTokenAsync(UserRole.Owner, "77777777-7777-4777-8777-777777777777");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var response = await client.PatchAsJsonAsync(
            "/api/v1/venues/11111111-1111-4111-8111-111111111111",
            new { name = new string('a', 1500) });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task PublicVenueDetailHidesPrice()
    {
        using var client = factory.CreateClient();
        using var list = JsonDocument.Parse(await (await client.GetAsync("/api/v1/venues?zone=Centro")).Content.ReadAsStringAsync());
        var id = list.RootElement.GetProperty("data")[0].GetProperty("id").GetString();

        using var response = await client.GetAsync($"/api/v1/venues/{id}");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.DoesNotContain("price", body.RootElement.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PublicVenueAvailabilityHidesInternalBookingDetails()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/v1/venues/11111111-1111-4111-8111-111111111111/availability?from=2027-05-01&to=2027-05-31");
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var date = Assert.Single(body.RootElement.GetProperty("data").EnumerateArray());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("2027-05-20", date.GetProperty("date").GetString());
        Assert.Equal("blocked", date.GetProperty("kind").GetString());
        Assert.DoesNotContain("note", date.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
        Assert.DoesNotContain("bookingId", date.EnumerateObject().Select(property => property.Name), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task VenueAvailabilityRejectsAnInvalidDateRange()
    {
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/v1/venues/11111111-1111-4111-8111-111111111111/availability?from=2027-06-01&to=2027-05-01");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
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
        var bookingId = bookingJson.RootElement.GetProperty("id").GetString();
        Assert.Equal("pending_payment", bookingJson.RootElement.GetProperty("status").GetString());

        using var replayRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
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
    public async Task VenueOwnerCanAnswerAQuoteForTheirVenue()
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Cotizable");
        var quoteId = await CreateClientQuoteAsync(venueId);

        using var response = await ownerClient.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response", new { total = 18000m, deposit = 5000m, message = "Incluye banquete." });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("answered", body.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task OwnerCannotAnswerAQuoteForAnotherOwnersVenue()
    {
        using var ownerClient = factory.CreateClient();
        ownerClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));
        var venueId = await CreatePublishedVenueAsync(ownerClient, "Salon Ajeno");
        var quoteId = await CreateClientQuoteAsync(venueId);
        using var otherOwner = factory.CreateClient();
        otherOwner.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Owner));

        using var response = await otherOwner.PostAsJsonAsync($"/api/v1/quotes/{quoteId}/response", new { total = 18000m });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProviderCannotAnswerAVenueQuote()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Provider));

        using var response = await client.PostAsJsonAsync($"/api/v1/quotes/{Guid.NewGuid():D}/response", new { total = 18000m });

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task QuoteValidationReturnsFieldErrors()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await LoginAsync(client));

        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = "11111111-1111-4111-8111-111111111111" },
            eventType = "Boda",
            eventDate = "2020-01-01",
            guests = 0,
            requestedServices = RequestedServices
        });
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var errors = body.RootElement.GetProperty("errors");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.True(errors.TryGetProperty("eventDate", out _));
        Assert.True(errors.TryGetProperty("guests", out _));
        Assert.False(errors.TryGetProperty("eventType", out _));
    }

    [Fact]
    public async Task ClientPaymentIsRecordedAsPendingNotValidated()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await LoginAsync(client));
        using var bookingRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bookings")
        {
            Content = JsonContent.Create(new { venueId = "11111111-1111-4111-8111-111111111111", date = "2027-06-20", hours = 1, deposit = 1000 })
        };
        bookingRequest.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));
        using var bookingResponse = await client.SendAsync(bookingRequest);
        Assert.Equal(HttpStatusCode.Created, bookingResponse.StatusCode);
        using var booking = JsonDocument.Parse(await bookingResponse.Content.ReadAsStringAsync());
        var bookingId = booking.RootElement.GetProperty("id").GetString();

        using var paymentRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/v1/bookings/{bookingId}/payments")
        {
            Content = JsonContent.Create(new { amount = 1000 })
        };
        paymentRequest.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString("D"));
        using var paymentResponse = await client.SendAsync(paymentRequest);
        using var payment = JsonDocument.Parse(await paymentResponse.Content.ReadAsStringAsync());

        Assert.Equal(HttpStatusCode.Created, paymentResponse.StatusCode);
        Assert.Equal("pending", payment.RootElement.GetProperty("status").GetString());
        using var bookingAfter = await client.GetAsync($"/api/v1/bookings/{bookingId}");
        using var bookingAfterJson = JsonDocument.Parse(await bookingAfter.Content.ReadAsStringAsync());
        Assert.Equal("pending_payment", bookingAfterJson.RootElement.GetProperty("status").GetString());
    }

    private static async Task<string> CreatePublishedVenueAsync(HttpClient ownerClient, string name)
    {
        using var create = await ownerClient.PostAsJsonAsync("/api/v1/venues", new
        {
            name,
            shortName = name,
            description = "Un salon creado para probar cotizaciones.",
            address = "Centro, Querétaro",
            zone = "Centro",
            capacity = 200,
            price = new { @base = 1200m, unit = "hour", minHours = 2 },
            services = Array.Empty<object>(),
            categories = NewVenueCategories,
            amenities = Array.Empty<string>()
        });
        Assert.Equal(HttpStatusCode.Created, create.StatusCode);
        using var created = JsonDocument.Parse(await create.Content.ReadAsStringAsync());
        var venueId = created.RootElement.GetProperty("id").GetString()!;
        using var publish = await ownerClient.PostAsync($"/api/v1/venues/{venueId}/publish", null);
        Assert.Equal(HttpStatusCode.OK, publish.StatusCode);
        return venueId;
    }

    private async Task<string> CreateClientQuoteAsync(string venueId)
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await CreateUserTokenAsync(UserRole.Client));
        using var response = await client.PostAsJsonAsync("/api/v1/quotes", new
        {
            target = new { type = "venue", id = venueId },
            eventType = "Boda",
            eventDate = "2027-05-15",
            guests = 150,
            requestedServices = RequestedServices
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return body.RootElement.GetProperty("id").GetString()!;
    }

    private static async Task<string> LoginAsync(HttpClient client)
    {
        using var response = await client.PostAsJsonAsync(
            "/api/v1/auth/login",
            new { email = "demo@virgo.mx", password = "VirGo-local-only-password" });
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return json.RootElement.GetProperty("accessToken").GetString()!;
    }

    private async Task<string> CreateUserTokenAsync(UserRole role, string? userId = null)
    {
        var email = $"{role.ToString().ToLowerInvariant()}-{Guid.NewGuid():N}@virgo.mx";
        using var scope = factory.Services.CreateScope();
        var users = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var clock = scope.ServiceProvider.GetRequiredService<IClock>();
        var user = new User
        {
            Id = userId ?? Guid.NewGuid().ToString("D"),
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
