using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Api.Dto;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/bookings")]
public sealed class BookingsController(BookingHandler handler, IUserRepository users, IBookingRepository bookings, IVenueRepository venues) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> Create(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        var user = string.IsNullOrWhiteSpace(userId) ? null : await users.FindByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Unauthorized();
        }

        if (!Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey)
            || string.IsNullOrWhiteSpace(idempotencyKey.ToString())
            || idempotencyKey.ToString().Length > 100)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Idempotency-Key es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(request.VenueId)
            || !Guid.TryParse(request.VenueId, out _)
            || string.IsNullOrWhiteSpace(request.Date)
            || !DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out _)
            || request.QuoteId is not null && !Guid.TryParse(request.QuoteId, out _)
            || request.EventId is not null && !Guid.TryParse(request.EventId, out _)
            || request.Hours < 1)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de reservación es inválida.");
        }

        var result = await handler.HandleAsync(new CreateBookingCommand(
            user.Id,
            idempotencyKey.ToString().Trim(),
            user.Name,
            request.QuoteId,
            request.EventId,
            request.VenueId,
            request.Date,
            request.Hours,
            request.Deposit,
            HttpContext.Items["X-Correlation-Id"] as string), cancellationToken);
        if (result.InvalidInput)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La solicitud de reservación es inválida.");
        }

        return result.Booking is null
            ? NotFound()
            : Created($"/api/v1/bookings/{result.Booking.Id}", result.Booking);
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var result = await bookings.ListVisibleAsync(userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return Ok(new { data = result });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var booking = await bookings.FindVisibleAsync(parsedId.ToString("D"), userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpGet("by-folio/{folio}")]
    public async Task<IActionResult> GetByFolio(string folio, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(folio) || folio.Length > 32) return NotFound();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var booking = await bookings.FindByFolioVisibleAsync(folio, userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> Cancel(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        var updated = await bookings.UpdateStatusAsync(parsedId.ToString("D"), userId, "cancelled", cancellationToken);
        return updated is null ? Conflict() : Ok(updated);
    }

    [HttpPost("{id}/payments")]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> AddPayment(string id, CreatePaymentRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId) || request?.Amount is null || request.Amount <= 0 || !Request.Headers.TryGetValue("Idempotency-Key", out var key) || string.IsNullOrWhiteSpace(key.ToString()) || key.ToString().Length > 128)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "El pago es inválido.");
        }
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var current = await bookings.FindVisibleAsync(parsedId.ToString("D"), userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        if (current is null) return NotFound();
        if (current.Status == "cancelled" || request.Amount > current.Total) return Conflict();
        var payment = new VirGo.Domain.Marketplace.BookingPayment { Id = Guid.NewGuid().ToString("D"), IdempotencyKey = key.ToString().Trim(), Amount = request.Amount.Value, Status = "validated", CreatedAt = DateTime.UtcNow };
        var updated = await bookings.AddPaymentAsync(current.Id, userId, [], payment, cancellationToken);
        return updated is null ? Conflict() : CreatedAtAction(nameof(Payments), new { id = current.Id }, payment);
    }

    [HttpGet("{id}/payments")]
    public async Task<IActionResult> Payments(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        var ownedVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var booking = await bookings.FindVisibleAsync(parsedId.ToString("D"), userId, ownedVenues.Select(venue => venue.Id).ToArray(), cancellationToken);
        return booking is null ? NotFound() : Ok(new { data = booking.Payments });
    }
}
