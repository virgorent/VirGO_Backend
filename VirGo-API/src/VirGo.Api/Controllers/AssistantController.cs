using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Assistant;
using VirGo.Api.Dto;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/assistant")]
public sealed class AssistantController(IAssistantMessageRepository messages) : ControllerBase
{
    [HttpGet("messages")]
    public async Task<IActionResult> Messages(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await messages.ListByUserAsync(userId, DateTime.UtcNow, cancellationToken);
        return Ok(new
        {
            data = result.Select(message => new
            {
                id = message.Id,
                role = message.Role,
                text = message.Text,
                createdAt = message.CreatedAt
            })
        });
    }

    [HttpPost("messages")]
    public async Task<IActionResult> Add(AssistantMessageRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || request is null || string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 4000) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "El mensaje es inválido.");
        var message = new VirGo.Domain.Assistant.AssistantMessage { Id = Guid.NewGuid().ToString("D"), UserId = userId, Role = "user", Text = request.Text.Trim(), CreatedAt = DateTime.UtcNow, PurgeAt = DateTime.UtcNow.AddDays(30) };
        var created = await messages.AddAsync(message, cancellationToken);
        return Created($"/api/v1/assistant/messages/{created.Id}", new { id = created.Id, role = created.Role, text = created.Text, createdAt = created.CreatedAt });
    }
}
