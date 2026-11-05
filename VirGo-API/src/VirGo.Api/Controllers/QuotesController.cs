using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/quotes")]
public sealed class QuotesController(QuoteHandler handler, IUserRepository users) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateQuoteRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Target?.Id)
            || string.IsNullOrWhiteSpace(request.EventType)
            || request.Guests < 1
            || !DateOnly.TryParseExact(request.EventDate, "yyyy-MM-dd", out var eventDate))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de cotización es inválida.");
        }

        var user = await GetCurrentUser(cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        var quote = await handler.HandleAsync(new CreateQuoteCommand(
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
        return quote is null
            ? NotFound()
            : Created($"/api/v1/quotes/{quote.Id}", quote);
    }

    private Task<VirGo.Domain.Auth.User?> GetCurrentUser(CancellationToken cancellationToken) =>
        users.FindByIdAsync(User.FindFirstValue("sub")!, cancellationToken);
}

public sealed record CreateQuoteRequest(
    QuoteTargetRequest? Target,
    string? EventId,
    string? EventType,
    string? EventDate,
    int Guests,
    string[]? RequestedServices,
    string? Message);

public sealed record QuoteTargetRequest(string? Type, string? Id);
