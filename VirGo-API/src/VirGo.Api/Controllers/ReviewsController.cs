using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Api.Dto;
using VirGo.Application.Auth;
using VirGo.Application.Marketplace;
using VirGo.Application.Providers;
using VirGo.Domain.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class ReviewsController(IReviewRepository reviews, IUserRepository users, IVenueRepository venues, IProviderRepository providers, IBookingRepository bookings) : ControllerBase
{
    [HttpGet("venues/{id}/reviews")]
    [AllowAnonymous]
    public Task<IActionResult> VenueReviews(string id, CancellationToken cancellationToken) => List("venue", id, cancellationToken);

    [HttpGet("providers/{id}/reviews")]
    [AllowAnonymous]
    public Task<IActionResult> ProviderReviews(string id, CancellationToken cancellationToken) => List("provider", id, cancellationToken);

    [HttpPost("reviews")]
    [Authorize]
    public async Task<IActionResult> Create(CreateReviewRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        var user = string.IsNullOrWhiteSpace(userId) ? null : await users.FindByIdAsync(userId, cancellationToken);
        if (user is null || request is null || request.TargetType is not ("venue" or "provider") || !Guid.TryParse(request.TargetId, out _) || request.Stars is < 1 or > 5 || string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 1000) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La reseña es inválida.");
        if (request.TargetType == "venue")
        {
            var venue = await venues.FindPublishedAsync(request.TargetId!, cancellationToken);
            if (venue is null || !Guid.TryParse(request.BookingId, out var bookingId)) return NotFound();
            var booking = await bookings.FindVisibleAsync(bookingId.ToString("D"), user.Id, [request.TargetId!], cancellationToken);
            if (booking is null || booking.Status != "confirmed") return Conflict();
        }
        else if (await providers.FindPublishedAsync(request.TargetId!, cancellationToken) is null) return NotFound();
        var review = new Review { Id = Guid.NewGuid().ToString("D"), TargetType = request.TargetType, TargetId = request.TargetId!, AuthorId = user.Id, AuthorName = user.Name, Stars = request.Stars, Text = request.Text.Trim(), BookingId = request.BookingId, CreatedAt = DateTime.UtcNow };
        var created = await reviews.AddAsync(review, cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/reviews/{review.Id}", created);
    }

    [HttpPost("reviews/{id}/reply")]
    [Authorize]
    public async Task<IActionResult> Reply(string id, ReviewReplyRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || request is null || string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 1000) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La respuesta es inválida.");
        var target = await FindOwnedTarget(id, userId, cancellationToken);
        if (target is null) return NotFound();
        var updated = await reviews.AddReplyAsync(id, target.Value.Type, target.Value.Id, new ReviewReply { AuthorId = userId, Text = request.Text.Trim(), CreatedAt = DateTime.UtcNow }, cancellationToken);
        return updated is null ? Conflict() : Ok(updated);
    }

    private async Task<IActionResult> List(string targetType, string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out _) || (targetType == "venue" ? await venues.FindPublishedAsync(id, cancellationToken) is null : await providers.FindPublishedAsync(id, cancellationToken) is null)) return NotFound();
        return Ok(new { data = await reviews.ListAsync(targetType, id, cancellationToken) });
    }

    private async Task<(string Type, string Id)?> FindOwnedTarget(string reviewId, string userId, CancellationToken cancellationToken)
    {
        var review = await reviews.FindByIdAsync(reviewId, cancellationToken);
        if (review is null) return null;
        if (review.TargetType == "venue" && (await venues.ListOwnedAsync(userId, cancellationToken)).Any(venue => venue.Id == review.TargetId)) return ("venue", review.TargetId);
        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        return review.TargetType == "provider" && provider?.Id == review.TargetId ? ("provider", review.TargetId) : null;
    }
}
