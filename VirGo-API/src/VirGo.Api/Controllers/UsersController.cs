using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Api.Dto;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public sealed class UsersController(IUserRepository users) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var user = await users.FindByIdAsync(userId, cancellationToken);
        return user is null
            ? NotFound()
            : Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phone = user.Phone,
                role = user.Role.ToString().ToLowerInvariant(),
                isVerified = user.IsVerified
            });
    }

    [HttpGet("{id}/public")]
    [AllowAnonymous]
    public async Task<IActionResult> Public(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            return NotFound();
        }

        var user = await users.FindByIdAsync(parsedId.ToString(), cancellationToken);
        return user is null
            ? NotFound()
            : Ok(new PublicUserResponse(
                user.Id,
                user.Name,
                user.Role.ToString().ToLowerInvariant(),
                user.IsVerified));
    }
}
