using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Marketplace;
using VirGo.Application.Providers;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1")]
public sealed class StatsController(IVenueRepository venues, IProviderRepository providers) : ControllerBase
{
    [HttpGet("venues/{id}/stats")]
    public async Task<IActionResult> Venue(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out _)) return NotFound();
        var venue = (await venues.ListOwnedAsync(userId, cancellationToken)).FirstOrDefault(item => item.Id == id);
        return venue is null ? NotFound() : Ok(new { id = venue.Id, status = venue.Status, rating = venue.RatingAverage, reviews = venue.RatingCount, visits = 0 });
    }

    [HttpGet("providers/{id}/stats")]
    public async Task<IActionResult> Provider(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        var provider = string.IsNullOrWhiteSpace(userId) ? null : await providers.FindByUserAsync(userId, cancellationToken);
        return provider is null || provider.Id != id ? NotFound() : Ok(new { id = provider.Id, status = provider.Status, rating = provider.RatingAverage, reviews = provider.RatingCount, visits = 0 });
    }

    [HttpGet("stats/overview")]
    public async Task<IActionResult> Overview(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var ownVenues = await venues.ListOwnedAsync(userId, cancellationToken);
        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        return Ok(new { venues = ownVenues.Count, providers = provider is null ? 0 : 1, published = ownVenues.Count(venue => venue.Status == "published") + (provider?.Status == "published" ? 1 : 0), visits = 0 });
    }
}
