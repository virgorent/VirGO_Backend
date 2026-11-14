using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public sealed class UsersController(
    IUserRepository users,
    DeleteAccountHandler deleteAccountHandler) : ControllerBase
{
    [HttpGet("me")]
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

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMe(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        await deleteAccountHandler.HandleAsync(userId, cancellationToken);
        return NoContent();
    }
}
