using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Api.Dto;
using VirGo.Application.Contact;
using VirGo.Domain.Contact;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/contact")]
public sealed class ContactController(IContactMessageRepository messages) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(ContactRequest? request, CancellationToken cancellationToken)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var message = new ContactMessage
        {
            Id = Guid.NewGuid().ToString("D"),
            UserId = User.FindFirstValue("sub"),
            Name = request!.Name!.Trim(),
            Email = request.Email!.Trim().ToLowerInvariant(),
            Message = request.Message!.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        await messages.AddAsync(message, cancellationToken);

        return Created($"/api/v1/contact/{message.Id}", new { id = message.Id });
    }

    private static Dictionary<string, string[]> Validate(ContactRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null)
        {
            errors["body"] = ["El cuerpo es obligatorio."];
            return errors;
        }
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Trim().Length > 100)
        {
            errors["name"] = ["El nombre es obligatorio y no puede superar 100 caracteres."];
        }
        if (string.IsNullOrWhiteSpace(request.Email)
            || request.Email.Trim().Length > 150
            || !new EmailAddressAttribute().IsValid(request.Email.Trim()))
        {
            errors["email"] = ["El correo debe tener un formato válido y no puede superar 150 caracteres."];
        }
        if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Trim().Length > 2000)
        {
            errors["message"] = ["El mensaje es obligatorio y no puede superar 2000 caracteres."];
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
}
