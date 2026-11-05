using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Auth;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController(
    LoginHandler loginHandler,
    RegisterHandler registerHandler,
    LogoutHandler logoutHandler) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors["name"] = ["El nombre es obligatorio."];
        }
        if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email.Trim()))
        {
            errors["email"] = ["El correo debe tener un formato válido."];
        }
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8 || request.Password.Length > 128)
        {
            errors["password"] = ["La contraseña debe tener entre 8 y 128 caracteres."];
        }
        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            errors["phone"] = ["El teléfono es obligatorio."];
        }
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        RegisterOutcome outcome;
        try
        {
            outcome = await registerHandler.HandleAsync(
                new RegisterCommand(request.Name!, request.Email!, request.Password!, request.Phone!),
                cancellationToken);
        }
        catch (DuplicateUserEmailException)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "El correo ya está registrado.");
        }
        if (outcome.Failure == RegisterFailure.EmailAlreadyExists)
        {
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "El correo ya está registrado.");
        }

        var user = outcome.User!;
        return Created($"/api/v1/users/{user.Id}", new
        {
            id = user.Id,
            name = user.Name,
            email = user.Email,
            phone = user.Phone,
            role = user.Role.ToString().ToLowerInvariant(),
            isVerified = user.IsVerified
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var validationErrors = Validate(request);
        if (validationErrors.Count > 0)
        {
            return ValidationProblem(validationErrors);
        }

        var outcome = await loginHandler.HandleAsync(
            new LoginCommand(request.Email!.Trim(), request.Password!),
            cancellationToken);
        if (!outcome.IsSuccess)
        {
            return InvalidCredentials();
        }

        var result = outcome.Result!;
        return Ok(new
        {
            accessToken = result.AccessToken,
            tokenType = "Bearer",
            expiresIn = 900,
            refreshToken = result.RefreshToken,
            refreshTokenExpiresAt = result.RefreshTokenExpiresAt,
            user = new
            {
                id = result.User.Id,
                name = result.User.Name,
                email = result.User.Email,
                role = result.User.Role.ToString().ToLowerInvariant()
            }
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        var sessionId = User.FindFirst("sid")?.Value;
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
        {
            return InvalidCredentials();
        }

        var deleted = await logoutHandler.HandleAsync(userId, sessionId, cancellationToken);
        return deleted ? NoContent() : InvalidCredentials();
    }

    private static Dictionary<string, string[]> Validate(LoginRequest request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(request.Email) || !new EmailAddressAttribute().IsValid(request.Email.Trim()))
        {
            errors["email"] = ["El correo debe tener un formato válido."];
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length > 128)
        {
            errors["password"] = ["La contraseña es obligatoria y no puede superar 128 caracteres."];
        }

        return errors;
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

    private ObjectResult InvalidCredentials()
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Correo o contraseña incorrectos.",
            Instance = Request.Path
        };
        problem.Extensions["traceId"] = HttpContext.TraceIdentifier;
        return new ObjectResult(problem)
        {
            StatusCode = StatusCodes.Status401Unauthorized,
            ContentTypes = ["application/problem+json"]
        };
    }
}

public sealed record LoginRequest(string? Email, string? Password);

public sealed record RegisterRequest(string? Name, string? Email, string? Password, string? Phone);
