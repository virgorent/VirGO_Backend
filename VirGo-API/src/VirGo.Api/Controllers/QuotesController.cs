using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Api.Dto;
using VirGo.Domain.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/quotes")]
public sealed class QuotesController(QuoteHandler handler, IUserRepository users, IQuoteRepository quotes, IVenueRepository venues) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> Create(CreateQuoteRequest request, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUser(cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.Target?.Id)
            || !Guid.TryParse(request.Target.Id, out _)
            || !string.Equals(request.Target.Type, "venue", StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(request.EventType)
            || !AllowedEventTypes.Contains(request.EventType, StringComparer.Ordinal)
            || request.Guests < 1
            || !DateOnly.TryParseExact(request.EventDate, "yyyy-MM-dd", out var eventDate)
            || request.EventId is not null && !Guid.TryParse(request.EventId, out _)
            || request.RequestedServices is not null && request.RequestedServices.Any(service => string.IsNullOrWhiteSpace(service) || service.Length > 100)
            || request.Message is not null && request.Message.Length > 2000)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de cotización es inválida.");
        }

        var result = await handler.HandleAsync(new CreateQuoteCommand(
            user.Id,
            user.Name,
            request.Target.Type ?? "venue",
            request.Target.Id,
            request.EventId ?? string.Empty,
            request.EventType,
            eventDate,
            request.Guests,
            request.RequestedServices ?? [],
            request.Message), cancellationToken);
        if (result.InvalidInput)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de cotización es inválida.");
        }

        return result.Quote is null
            ? NotFound()
            : Created($"/api/v1/quotes/{result.Quote.Id}", result.Quote);
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var result = await quotes.ListVisibleAsync(userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return Ok(new { data = result });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var quote = await quotes.FindVisibleAsync(parsedId.ToString("D"), userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return quote is null ? NotFound() : Ok(quote);
    }

    [HttpPost("{id}/response")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> Respond(string id, QuoteResponseRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId) || request?.Total is null || request.Total <= 0 || request.Message?.Length > 2000)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La respuesta de cotización es inválida.");
        }

        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var quote = await quotes.FindVisibleAsync(parsedId.ToString("D"), userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        if (quote is null || !ownedVenues.Any(venue => venue.Id == quote.TargetId)) return NotFound();
        if (quote.Status != "sent") return Conflict();
        var updated = await quotes.UpdateAsync(Transition(quote, "answered", new QuoteResponse { Total = request.Total.Value, Deposit = request.Deposit, Message = request.Message }), "sent", cancellationToken);
        return updated is null ? Conflict() : Ok(updated);
    }

    [HttpPost("{id}/accept")]
    [Authorize(Roles = "client")]
    public Task<IActionResult> Accept(string id, CancellationToken cancellationToken) => Decide(id, "accepted", cancellationToken);

    [HttpPost("{id}/reject")]
    [Authorize(Roles = "client")]
    public Task<IActionResult> Reject(string id, CancellationToken cancellationToken) => Decide(id, "rejected", cancellationToken);

    [HttpPost("{id}/expire")]
    [Authorize(Roles = "admin")]
    public Task<IActionResult> Expire(string id, CancellationToken cancellationToken) => Decide(id, "expired", cancellationToken, "sent", "answered");

    private async Task<IActionResult> Decide(string id, string status, CancellationToken cancellationToken, params string[] allowedStatuses)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        var quote = status == "expired"
            ? await quotes.FindByIdAsync(parsedId.ToString("D"), cancellationToken)
            : await quotes.FindVisibleAsync(parsedId.ToString("D"), userId, [], cancellationToken);
        if (quote is null || status != "expired" && quote.ClientId != userId) return NotFound();
        if (status == "expired" && quote.CreatedAt.AddDays(15) > DateTime.UtcNow) return Conflict();
        var expectedStatus = allowedStatuses.Length == 0 ? "answered" : allowedStatuses.FirstOrDefault(value => value == quote.Status);
        if (expectedStatus is null) return Conflict();
        var updated = await quotes.UpdateAsync(Transition(quote, status), expectedStatus, cancellationToken);
        return updated is null ? Conflict() : Ok(updated);
    }

    private static Quote Transition(Quote quote, string status, QuoteResponse? response = null) => new()
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
        Response = response ?? quote.Response,
        Status = status,
        CreatedAt = quote.CreatedAt,
        UpdatedAt = DateTime.UtcNow
    };

    private Task<VirGo.Domain.Auth.User?> GetCurrentUser(CancellationToken cancellationToken) =>
        users.FindByIdAsync(User.FindFirstValue("sub")!, cancellationToken);

    private static readonly string[] AllowedEventTypes = ["Boda", "XV años", "Cumpleaños", "Corporativo", "Otro"];
}
