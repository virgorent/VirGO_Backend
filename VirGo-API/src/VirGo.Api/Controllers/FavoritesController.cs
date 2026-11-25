using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Api.Dto;
using VirGo.Application.Marketplace;
using VirGo.Application.Providers;
using VirGo.Domain.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/favorites")]
public sealed class FavoritesController(IFavoriteRepository favorites, IVenueRepository venues, IProviderRepository providers) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        return string.IsNullOrWhiteSpace(userId) ? Unauthorized() : Ok(new { data = await favorites.ListAsync(userId, cancellationToken) });
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateFavoriteRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || request is null || request.TargetType is not ("venue" or "provider") || !Guid.TryParse(request.TargetId, out _)) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "El favorito es inválido.");
        string name; string? photo; decimal rating;
        if (request.TargetType == "venue")
        {
            var venue = await venues.FindPublishedAsync(request.TargetId!, cancellationToken);
            if (venue is null) return NotFound();
            name = venue.Name; photo = venue.Photos.FirstOrDefault(); rating = venue.RatingAverage;
        }
        else
        {
            var provider = await providers.FindPublishedAsync(request.TargetId!, cancellationToken);
            if (provider is null) return NotFound();
            name = provider.BusinessName; photo = provider.CoverUrl ?? provider.Gallery.FirstOrDefault(); rating = provider.RatingAverage;
        }
        var favorite = new Favorite { Id = Guid.NewGuid().ToString("D"), UserId = userId, TargetType = request.TargetType, TargetId = request.TargetId!, Name = name, PhotoUrl = photo, Rating = rating, CreatedAt = DateTime.UtcNow };
        var created = await favorites.AddAsync(favorite, cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/favorites/{favorite.TargetType}/{favorite.TargetId}", created);
    }

    [HttpDelete("{targetType}/{targetId}")]
    public async Task<IActionResult> Delete(string targetType, string targetId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || targetType is not ("venue" or "provider") || !Guid.TryParse(targetId, out _)) return NotFound();
        return await favorites.DeleteAsync(userId, targetType, targetId, cancellationToken) ? NoContent() : NotFound();
    }
}
