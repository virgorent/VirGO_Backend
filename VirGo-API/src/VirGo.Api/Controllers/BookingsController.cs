using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/bookings")]
public sealed class BookingsController(BookingHandler handler, IUserRepository users) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        if (!Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey)
            || string.IsNullOrWhiteSpace(idempotencyKey.ToString()))
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Idempotency-Key es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.VenueId)
            || string.IsNullOrWhiteSpace(request.Date)
            || request.Hours < 1)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de reservación es inválida.");
        }

        var userId = User.FindFirst("sub")?.Value;
        var user = string.IsNullOrWhiteSpace(userId) ? null : await users.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        var booking = await handler.HandleAsync(new CreateBookingCommand(
            user.Id,
            user.Name,
            request.QuoteId,
            request.EventId,
            request.VenueId,
            request.Date,
            request.Hours,
            request.Deposit), cancellationToken);
        return booking is null
            ? NotFound()
            : Created($"/api/v1/bookings/{booking.Id}", booking);
    }
}

public sealed record CreateBookingRequest(
    string? QuoteId,
    string? EventId,
    string? VenueId,
    string? Date,
    int Hours,
    decimal? Deposit);
