using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Events;
using VirGo.Application.Marketplace;
using VirGo.Domain.Events;

namespace VirGo.Api.Controllers;

// Eventos del cliente, sus invitados y sus pendientes. Solo el dueño (rol client) los ve y los cambia;
// un evento ajeno, inexistente o con id inválido responde 404 para no revelar que existe.
[ApiController]
[Route("api/v1/events")]
[Authorize(Roles = "client")]
public sealed class EventsController(IEventRepository events, IVenueRepository venues, IEventGuestRepository guests, IEventTaskRepository tasks) : ControllerBase
{
    private static readonly string[] EventTypes = ["Boda", "XV años", "Cumpleaños", "Corporativo", "Otro"];
    private static readonly string[] GuestStatuses = ["confirmed", "pending", "declined"];

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var result = await events.ListForOwnerAsync(userId, cancellationToken);
        return Ok(new { data = result.Select(ToResponse) });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var errors = Validate(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        var (venue, venueError) = await ResolveVenueAsync(request!.VenueId, cancellationToken);
        if (venueError is not null) return venueError;

        var now = DateTime.UtcNow;
        var item = new VirGoEvent
        {
            Id = Guid.NewGuid().ToString("D"), OwnerId = userId, Name = request.Name!.Trim(), EventType = request.EventType!,
            Date = request.Date!, Guests = request.Guests!.Value, Budget = request.Budget!.Value, Hours = request.Hours!.Value,
            Venue = venue, CreatedAt = now, UpdatedAt = now
        };
        var created = await events.CreateAsync(item, cancellationToken);
        return Created($"/api/v1/events/{created.Id}", ToResponse(created));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(string id, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var item = await FindOwnedAsync(id, userId, cancellationToken);
        return item is null ? NotFound() : Ok(ToResponse(item));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, CreateEventRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var current = await FindOwnedAsync(id, userId, cancellationToken);
        if (current is null) return NotFound();
        var errors = Validate(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        var (venue, venueError) = await ResolveVenueAsync(request!.VenueId, cancellationToken);
        if (venueError is not null) return venueError;

        var updated = new VirGoEvent
        {
            Id = current.Id, OwnerId = current.OwnerId, Name = request.Name!.Trim(), EventType = request.EventType!, Date = request.Date!,
            Guests = request.Guests!.Value, Budget = request.Budget!.Value, Hours = request.Hours!.Value, Venue = venue,
            CreatedAt = current.CreatedAt, UpdatedAt = DateTime.UtcNow
        };
        var result = await events.UpdateForOwnerAsync(updated, userId, cancellationToken);
        return result is null ? NotFound() : Ok(ToResponse(result));
    }

    [HttpGet("{id}/guests")]
    public async Task<IActionResult> Guests(string id, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var result = await guests.ListAsync(owned.Id, cancellationToken);
        return Ok(new { data = result.Select(ToGuestResponse) });
    }

    [HttpPost("{id}/guests")]
    public async Task<IActionResult> CreateGuest(string id, CreateGuestRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var errors = ValidateGuest(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var guest = new EventGuest { Id = Guid.NewGuid().ToString("D"), EventId = owned.Id, Name = request!.Name!.Trim(), Phone = request.Phone!.Trim(), Companions = request.Companions!.Value, Status = request.Status!, Table = NormalizeTable(request.Table) };
        var created = await guests.CreateAsync(guest, cancellationToken);
        return Created($"/api/v1/events/{owned.Id}/guests/{created.Id}", ToGuestResponse(created));
    }

    [HttpPatch("{id}/guests/{guestId}")]
    public async Task<IActionResult> UpdateGuest(string id, string guestId, CreateGuestRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        if (!Guid.TryParse(guestId, out var parsedGuestId)) return NotFound();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var errors = ValidateGuest(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var updated = new EventGuest { Id = parsedGuestId.ToString("D"), EventId = owned.Id, Name = request!.Name!.Trim(), Phone = request.Phone!.Trim(), Companions = request.Companions!.Value, Status = request.Status!, Table = NormalizeTable(request.Table) };
        var result = await guests.UpdateAsync(updated, cancellationToken);
        return result is null ? NotFound() : Ok(ToGuestResponse(result));
    }

    [HttpDelete("{id}/guests/{guestId}")]
    public async Task<IActionResult> DeleteGuest(string id, string guestId, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        if (!Guid.TryParse(guestId, out var parsedGuestId)) return NotFound();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        return await guests.DeleteAsync(owned.Id, parsedGuestId.ToString("D"), cancellationToken) ? NoContent() : NotFound();
    }

    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> Tasks(string id, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var result = await tasks.ListAsync(owned.Id, cancellationToken);
        return Ok(new { data = result.Select(ToTaskResponse) });
    }

    [HttpPost("{id}/tasks")]
    public async Task<IActionResult> CreateTask(string id, EventTaskRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var errors = ValidateTask(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var task = new EventTask { Id = Guid.NewGuid().ToString("D"), EventId = owned.Id, Title = request!.Title!.Trim(), DueDate = request.DueDate!, Done = request.Done!.Value, Suggested = false };
        var created = await tasks.CreateAsync(task, cancellationToken);
        return Created($"/api/v1/events/{owned.Id}/tasks/{created.Id}", ToTaskResponse(created));
    }

    [HttpPatch("{id}/tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(string id, string taskId, EventTaskRequest? request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        if (!Guid.TryParse(taskId, out var parsedTaskId)) return NotFound();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        var errors = ValidateTask(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var task = new EventTask { Id = parsedTaskId.ToString("D"), EventId = owned.Id, Title = request!.Title!.Trim(), DueDate = request.DueDate!, Done = request.Done!.Value, Suggested = false };
        var updated = await tasks.UpdateAsync(task, cancellationToken);
        return updated is null ? NotFound() : Ok(ToTaskResponse(updated));
    }

    [HttpDelete("{id}/tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(string id, string taskId, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();
        if (userId is null) return Unauthorized();
        if (!Guid.TryParse(taskId, out var parsedTaskId)) return NotFound();
        var owned = await FindOwnedAsync(id, userId, cancellationToken);
        if (owned is null) return NotFound();
        return await tasks.DeleteAsync(owned.Id, parsedTaskId.ToString("D"), cancellationToken) ? NoContent() : NotFound();
    }

    private string? CurrentUserId()
    {
        var userId = User.FindFirstValue("sub");
        return string.IsNullOrWhiteSpace(userId) ? null : userId;
    }

    private async Task<VirGoEvent?> FindOwnedAsync(string id, string userId, CancellationToken cancellationToken) =>
        Guid.TryParse(id, out var parsedId) ? await events.FindForOwnerAsync(parsedId.ToString("D"), userId, cancellationToken) : null;

    private async Task<(EventVenue? Venue, IActionResult? Error)> ResolveVenueAsync(string? venueId, CancellationToken cancellationToken)
    {
        if (venueId is null) return (null, null);
        var publishedVenue = await venues.FindPublishedAsync(Guid.Parse(venueId).ToString("D"), cancellationToken);
        if (publishedVenue is null)
        {
            return (null, ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal) { ["venueId"] = ["El salón debe existir y estar publicado."] }));
        }

        return (new EventVenue(publishedVenue.Id, publishedVenue.Name, publishedVenue.Photos.FirstOrDefault() ?? string.Empty), null);
    }

    private static string? NormalizeTable(string? table) => string.IsNullOrWhiteSpace(table) ? null : table.Trim();

    private static Dictionary<string, string[]> Validate(CreateEventRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100) errors["name"] = ["name es obligatorio y debe tener máximo 100 caracteres."];
        if (request.EventType is null || !EventTypes.Contains(request.EventType, StringComparer.Ordinal)) errors["eventType"] = ["eventType no es válido."];
        if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) errors["date"] = ["date debe tener formato yyyy-MM-dd."];
        if (request.Guests is null or < 1) errors["guests"] = ["guests debe ser mayor que cero."];
        if (request.Budget is null or < 0) errors["budget"] = ["budget debe ser mayor o igual que cero."];
        if (request.Hours is null or < 1) errors["hours"] = ["hours debe ser mayor que cero."];
        if (request.VenueId is not null && !Guid.TryParse(request.VenueId, out _)) errors["venueId"] = ["venueId debe ser un UUID válido."];
        return errors;
    }

    private static Dictionary<string, string[]> ValidateGuest(CreateGuestRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100) errors["name"] = ["name es obligatorio y debe tener máximo 100 caracteres."];
        if (string.IsNullOrWhiteSpace(request.Phone) || request.Phone.Trim().Length > 16) errors["phone"] = ["phone es obligatorio y debe tener máximo 16 caracteres."];
        if (request.Companions is null or < 0) errors["companions"] = ["companions debe ser mayor o igual que cero."];
        if (request.Status is null || !GuestStatuses.Contains(request.Status, StringComparer.Ordinal)) errors["status"] = ["status no es válido."];
        if (request.Table?.Trim().Length > 20) errors["table"] = ["table debe tener máximo 20 caracteres."];
        return errors;
    }

    private static Dictionary<string, string[]> ValidateTask(EventTaskRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Trim().Length > 150) errors["title"] = ["title es obligatorio y debe tener máximo 150 caracteres."];
        if (!DateOnly.TryParseExact(request.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) errors["dueDate"] = ["dueDate debe tener formato yyyy-MM-dd."];
        if (request.Done is null) errors["done"] = ["done es obligatorio."];
        return errors;
    }

    private static object ToGuestResponse(EventGuest guest) => new { id = guest.Id, eventId = guest.EventId, name = guest.Name, phone = guest.Phone, companions = guest.Companions, status = guest.Status, table = guest.Table };

    private static object ToTaskResponse(EventTask task) => new { id = task.Id, eventId = task.EventId, title = task.Title, dueDate = task.DueDate, done = task.Done, suggested = task.Suggested };

    // Sin ownerId ni otros datos internos: el dueño ya sabe que es suyo.
    private static object ToResponse(VirGoEvent item) => new
    {
        id = item.Id, name = item.Name, eventType = item.EventType, date = item.Date,
        guests = item.Guests, budget = item.Budget, hours = item.Hours,
        venue = item.Venue is null ? null : new { venueId = item.Venue.VenueId, name = item.Venue.Name, photoUrl = item.Venue.PhotoUrl },
        design = (object?)null, budgetPlan = (object?)null, invitation = (object?)null,
        reminders = Array.Empty<object>(), vendors = Array.Empty<object>(), externalPayments = Array.Empty<object>(),
        collaborators = Array.Empty<object>(), createdAt = item.CreatedAt, updatedAt = item.UpdatedAt
    };

    private ObjectResult ValidationProblem(Dictionary<string, string[]> errors)
    {
        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "La solicitud tiene datos inválidos.",
            Instance = Request.Path
        };
        problem.Extensions["traceId"] = HttpContext.TraceIdentifier;
        return new ObjectResult(problem)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = ["application/problem+json"]
        };
    }
}
