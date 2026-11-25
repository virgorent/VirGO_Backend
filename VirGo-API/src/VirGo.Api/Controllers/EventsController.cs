using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Events;
using VirGo.Application.Marketplace;
using VirGo.Domain.Events;
using System.Globalization;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/events")]
[Authorize]
public sealed class EventsController(IEventRepository events, IVenueRepository venues, IEventGuestRepository guests, IEventTaskRepository tasks) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var result = await events.ListForUserAsync(userId, cancellationToken);
        return Ok(new
        {
            data = result.Select(ToResponse)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var errors = Validate(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        EventVenue? venue = null;
        if (request!.VenueId is not null)
        {
            var publishedVenue = await venues.FindPublishedAsync(request.VenueId, cancellationToken);
            if (publishedVenue is null)
            {
                errors["venueId"] = ["El salón debe existir y estar publicado."];
                return ValidationProblem(errors);
            }
            venue = new EventVenue(publishedVenue.Id, publishedVenue.Name, publishedVenue.Photos.FirstOrDefault() ?? string.Empty);
        }

        var now = DateTime.UtcNow;
        var item = new VirGoEvent
        {
            Id = Guid.NewGuid().ToString("D"), OwnerId = userId, Name = request.Name!.Trim(), EventType = request.EventType!,
            Date = request.Date!, Guests = request.Guests!.Value, Budget = request.Budget!.Value, Hours = request.Hours!.Value,
            Venue = venue, Collaborators = [], CreatedAt = now, UpdatedAt = now
        };
        var created = await events.CreateAsync(item, cancellationToken);
        return CreatedAtAction(nameof(List), ToResponse(created));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var item = await events.FindForUserAsync(parsedId.ToString("D"), userId, cancellationToken);
        return item is null ? NotFound() : Ok(ToResponse(item));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, CreateEventRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var current = await events.FindForUserAsync(parsedId.ToString("D"), userId, cancellationToken);
        if (current is null) return NotFound();
        var errors = Validate(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        EventVenue? venue = null;
        if (request!.VenueId is not null)
        {
            var publishedVenue = await venues.FindPublishedAsync(request.VenueId, cancellationToken);
            if (publishedVenue is null)
            {
                errors["venueId"] = ["El salón debe existir y estar publicado."];
                return ValidationProblem(errors);
            }
            venue = new EventVenue(publishedVenue.Id, publishedVenue.Name, publishedVenue.Photos.FirstOrDefault() ?? string.Empty);
        }

        var updated = new VirGoEvent
        {
            Id = current.Id, OwnerId = current.OwnerId, Name = request.Name!.Trim(), EventType = request.EventType!, Date = request.Date!,
            Guests = request.Guests!.Value, Budget = request.Budget!.Value, Hours = request.Hours!.Value, Venue = venue,
            Collaborators = current.Collaborators, CreatedAt = current.CreatedAt, UpdatedAt = DateTime.UtcNow
        };
        var result = await events.UpdateForUserAsync(updated, userId, cancellationToken);
        return result is null ? NotFound() : Ok(ToResponse(result));
    }

    [HttpGet("{id}/guests")]
    public async Task<IActionResult> Guests(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var eventId = parsedId.ToString("D");
        if (await events.FindForUserAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var result = await guests.ListAsync(eventId, cancellationToken);
        return Ok(new { data = result.Select(guest => new { id = guest.Id, eventId = guest.EventId, name = guest.Name, phone = guest.Phone, companions = guest.Companions, status = guest.Status, table = guest.Table }) });
    }

    [HttpPost("{id}/guests")]
    public async Task<IActionResult> CreateGuest(string id, CreateGuestRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var eventId = parsedId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var errors = ValidateGuest(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var guest = new EventGuest { Id = Guid.NewGuid().ToString("D"), EventId = eventId, Name = request!.Name!.Trim(), Phone = request.Phone!.Trim(), Companions = request.Companions!.Value, Status = request.Status!, Table = request.Table?.Trim() };
        var created = await guests.CreateAsync(guest, cancellationToken);
        return CreatedAtAction(nameof(Guests), new { id = eventId }, new { id = created.Id, eventId = created.EventId, name = created.Name, phone = created.Phone, companions = created.Companions, status = created.Status, table = created.Table });
    }

    [HttpPatch("{id}/guests/{guestId}")]
    public async Task<IActionResult> UpdateGuest(string id, string guestId, CreateGuestRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedEventId) || !Guid.TryParse(guestId, out var parsedGuestId)) return NotFound();
        var eventId = parsedEventId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var errors = ValidateGuest(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var updated = new EventGuest { Id = parsedGuestId.ToString("D"), EventId = eventId, Name = request!.Name!.Trim(), Phone = request.Phone!.Trim(), Companions = request.Companions!.Value, Status = request.Status!, Table = request.Table?.Trim() };
        var result = await guests.UpdateAsync(updated, cancellationToken);
        return result is null ? NotFound() : Ok(new { id = result.Id, eventId = result.EventId, name = result.Name, phone = result.Phone, companions = result.Companions, status = result.Status, table = result.Table });
    }

    [HttpDelete("{id}/guests/{guestId}")]
    public async Task<IActionResult> DeleteGuest(string id, string guestId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedEventId) || !Guid.TryParse(guestId, out var parsedGuestId)) return NotFound();
        var eventId = parsedEventId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        return await guests.DeleteAsync(eventId, parsedGuestId.ToString("D"), cancellationToken) ? NoContent() : NotFound();
    }

    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> Tasks(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var eventId = parsedId.ToString("D");
        if (await events.FindForUserAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var result = await tasks.ListAsync(eventId, cancellationToken);
        return Ok(new { data = result.Select(ToTaskResponse) });
    }

    [HttpPost("{id}/tasks")]
    public async Task<IActionResult> CreateTask(string id, EventTaskRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var eventId = parsedId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var errors = ValidateTask(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var task = new EventTask { Id = Guid.NewGuid().ToString("D"), EventId = eventId, Title = request!.Title!.Trim(), DueDate = request.DueDate!, Done = request.Done!.Value, Suggested = false };
        var created = await tasks.CreateAsync(task, cancellationToken);
        return CreatedAtAction(nameof(Tasks), new { id = eventId }, ToTaskResponse(created));
    }

    [HttpPatch("{id}/tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(string id, string taskId, EventTaskRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedEventId) || !Guid.TryParse(taskId, out var parsedTaskId)) return NotFound();
        var eventId = parsedEventId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        var errors = ValidateTask(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var task = new EventTask { Id = parsedTaskId.ToString("D"), EventId = eventId, Title = request!.Title!.Trim(), DueDate = request.DueDate!, Done = request.Done!.Value, Suggested = false };
        var updated = await tasks.UpdateAsync(task, cancellationToken);
        return updated is null ? NotFound() : Ok(ToTaskResponse(updated));
    }

    [HttpDelete("{id}/tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(string id, string taskId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedEventId) || !Guid.TryParse(taskId, out var parsedTaskId)) return NotFound();
        var eventId = parsedEventId.ToString("D");
        if (await events.FindForEditAsync(eventId, userId, cancellationToken) is null) return NotFound();
        return await tasks.DeleteAsync(eventId, parsedTaskId.ToString("D"), cancellationToken) ? NoContent() : NotFound();
    }

    [HttpGet("{id}/collaborators")]
    public async Task<IActionResult> Collaborators(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return Unauthorized();
        var item = await events.FindForUserAsync(parsedId.ToString("D"), userId, cancellationToken);
        return item is null ? NotFound() : Ok(new { data = item.Collaborators.Select(ToCollaboratorResponse) });
    }

    [HttpPost("{id}/collaborators")]
    public async Task<IActionResult> AddCollaborator(string id, CollaboratorRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return Unauthorized();
        var current = await events.FindForUserAsync(parsedId.ToString("D"), userId, cancellationToken);
        if (current is null || current.OwnerId != userId) return NotFound();
        var errors = ValidateCollaborator(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        if (current.Collaborators.Any(collaborator => collaborator.UserId == request!.UserId)) return Conflict();
        var collaborator = new EventCollaborator(request!.UserId!, request.Name!.Trim(), request.Email!.Trim(), request.Permission!);
        var updated = await events.UpdateCollaboratorsAsync(current.Id, userId, [.. current.Collaborators, collaborator], cancellationToken);
        return updated is null ? NotFound() : CreatedAtAction(nameof(Collaborators), new { id = current.Id }, ToCollaboratorResponse(collaborator));
    }

    [HttpPatch("{id}/collaborators/{userId}")]
    public async Task<IActionResult> UpdateCollaborator(string id, string userId, CollaboratorRequest? request, CancellationToken cancellationToken)
    {
        var actorId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(actorId) || !Guid.TryParse(id, out var parsedId) || !Guid.TryParse(userId, out var parsedCollaboratorId)) return NotFound();
        var current = await events.FindForUserAsync(parsedId.ToString("D"), actorId, cancellationToken);
        if (current is null || current.OwnerId != actorId) return NotFound();
        var errors = ValidateCollaborator(request, requireDetails: false, requireUserId: false);
        if (errors.Count > 0) return ValidationProblem(errors);
        var index = Array.FindIndex(current.Collaborators, collaborator => collaborator.UserId == parsedCollaboratorId.ToString("D"));
        if (index < 0) return NotFound();
        var replacement = new EventCollaborator(current.Collaborators[index].UserId, request!.Name?.Trim() ?? current.Collaborators[index].Name, request.Email?.Trim() ?? current.Collaborators[index].Email, request.Permission!);
        var collaborators = current.Collaborators.ToArray(); collaborators[index] = replacement;
        var updated = await events.UpdateCollaboratorsAsync(current.Id, actorId, collaborators, cancellationToken);
        return updated is null ? NotFound() : Ok(ToCollaboratorResponse(replacement));
    }

    [HttpDelete("{id}/collaborators/{userId}")]
    public async Task<IActionResult> DeleteCollaborator(string id, string userId, CancellationToken cancellationToken)
    {
        var actorId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(actorId) || !Guid.TryParse(id, out var parsedId) || !Guid.TryParse(userId, out var parsedCollaboratorId)) return NotFound();
        var current = await events.FindForUserAsync(parsedId.ToString("D"), actorId, cancellationToken);
        if (current is null || current.OwnerId != actorId) return NotFound();
        var collaborators = current.Collaborators.Where(collaborator => collaborator.UserId != parsedCollaboratorId.ToString("D")).ToArray();
        if (collaborators.Length == current.Collaborators.Length) return NotFound();
        return await events.UpdateCollaboratorsAsync(current.Id, actorId, collaborators, cancellationToken) is null ? NotFound() : NoContent();
    }

    private static Dictionary<string, string[]> Validate(CreateEventRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        var types = new[] { "Boda", "XV años", "Cumpleaños", "Corporativo", "Otro" };
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100) errors["name"] = ["name es obligatorio y debe tener máximo 100 caracteres."];
        if (request.EventType is null || !types.Contains(request.EventType, StringComparer.Ordinal)) errors["eventType"] = ["eventType no es válido."];
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
        var statuses = new[] { "confirmed", "pending", "declined" };
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100) errors["name"] = ["name es obligatorio y debe tener máximo 100 caracteres."];
        if (string.IsNullOrWhiteSpace(request.Phone) || request.Phone.Length > 16) errors["phone"] = ["phone es obligatorio y debe tener máximo 16 caracteres."];
        if (request.Companions is null or < 0) errors["companions"] = ["companions debe ser mayor o igual que cero."];
        if (request.Status is null || !statuses.Contains(request.Status, StringComparer.Ordinal)) errors["status"] = ["status no es válido."];
        if (request.Table?.Length > 20) errors["table"] = ["table debe tener máximo 20 caracteres."];
        return errors;
    }

    private static Dictionary<string, string[]> ValidateTask(EventTaskRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 150) errors["title"] = ["title es obligatorio y debe tener máximo 150 caracteres."];
        if (!DateOnly.TryParseExact(request.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) errors["dueDate"] = ["dueDate debe tener formato yyyy-MM-dd."];
        if (request.Done is null) errors["done"] = ["done es obligatorio."];
        return errors;
    }

    private static Dictionary<string, string[]> ValidateCollaborator(CollaboratorRequest? request, bool requireDetails = true, bool requireUserId = true)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        var permissions = new[] { "view", "edit" };
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (requireUserId && !Guid.TryParse(request.UserId, out _)) errors["userId"] = ["userId debe ser un UUID válido."];
        if (requireDetails && (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 100)) errors["name"] = ["name es obligatorio y debe tener máximo 100 caracteres."];
        if (requireDetails && (string.IsNullOrWhiteSpace(request.Email) || request.Email.Length > 150 || !request.Email.Contains('@'))) errors["email"] = ["email no es válido."];
        if (request.Permission is null || !permissions.Contains(request.Permission, StringComparer.Ordinal)) errors["permission"] = ["permission debe ser view o edit."];
        return errors;
    }

    private static object ToTaskResponse(EventTask task) => new { id = task.Id, eventId = task.EventId, title = task.Title, dueDate = task.DueDate, done = task.Done, suggested = task.Suggested };

    private static object ToCollaboratorResponse(EventCollaborator collaborator) => new { userId = collaborator.UserId, name = collaborator.Name, email = collaborator.Email, permission = collaborator.Permission };

    private static object ToResponse(VirGoEvent item) => new
    {
        id = item.Id, ownerId = item.OwnerId, name = item.Name, eventType = item.EventType, date = item.Date,
        guests = item.Guests, budget = item.Budget, hours = item.Hours, venue = item.Venue,
        design = (object?)null, budgetPlan = (object?)null, invitation = (object?)null,
        reminders = Array.Empty<object>(), vendors = Array.Empty<object>(), externalPayments = Array.Empty<object>(),
        collaborators = item.Collaborators, createdAt = item.CreatedAt, updatedAt = item.UpdatedAt
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
