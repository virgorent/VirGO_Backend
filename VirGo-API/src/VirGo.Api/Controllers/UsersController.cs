using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;
using VirGo.Api.Dto;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
public sealed class UsersController(
    IUserRepository users,
    DeleteAccountHandler deleteAccountHandler,
    UpdateProfileHandler updateProfileHandler,
    ChangePasswordHandler changePasswordHandler) : ControllerBase
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

    [HttpPatch("me")]
    [Authorize]
    public async Task<IActionResult> PatchMe(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request.Name is not null && (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100))
        {
            errors["name"] = ["El nombre no puede estar vacío ni superar 100 caracteres."];
        }
        if (request.Phone is not null && (string.IsNullOrWhiteSpace(request.Phone) || request.Phone.Trim().Length > 16))
        {
            errors["phone"] = ["El teléfono no puede estar vacío ni superar 16 caracteres."];
        }
        if (request.City is not null && request.City.Trim().Length > 80)
        {
            errors["city"] = ["La ciudad no puede superar 80 caracteres."];
        }
        if (request.DefaultGuests is < 0)
        {
            errors["defaultGuests"] = ["El número de invitados no puede ser negativo."];
        }
        if (request.Name is null && request.Phone is null && request.City is null && request.DefaultGuests is null)
        {
            errors["body"] = ["Debe proporcionar al menos un campo actualizable."];
        }
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var user = await updateProfileHandler.HandleAsync(
            userId,
            new UpdateProfileCommand(
                request.Name?.Trim(),
                request.Phone?.Trim(),
                request.City?.Trim(),
                request.DefaultGuests),
            cancellationToken);
        return user is null
            ? NotFound()
            : Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                phone = user.Phone,
                city = user.City,
                defaultGuests = user.DefaultGuests,
                role = user.Role.ToString().ToLowerInvariant(),
                isVerified = user.IsVerified
            });
    }

    [HttpPatch("me/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(request.CurrentPassword) || request.CurrentPassword.Length > 128)
        {
            errors["currentPassword"] = ["La contraseña actual es obligatoria y no puede superar 128 caracteres."];
        }
        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 8 || request.NewPassword.Length > 128)
        {
            errors["newPassword"] = ["La nueva contraseña debe tener entre 8 y 128 caracteres."];
        }
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var changed = await changePasswordHandler.HandleAsync(
            userId,
            new ChangePasswordCommand(request.CurrentPassword!, request.NewPassword!),
            cancellationToken);
        return changed
            ? NoContent()
            : Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: "La contraseña actual no es correcta.");
    }

    [HttpDelete("me")]
    [Authorize]
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
