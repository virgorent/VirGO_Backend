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
[Route("api/v1/conversations")]
public sealed class ConversationsController(IConversationRepository conversations, IVenueRepository venues, IProviderRepository providers) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(new { data = await conversations.ListAsync(User.FindFirstValue("sub")!, cancellationToken) });

    [HttpPost]
    public async Task<IActionResult> Create(CreateConversationRequest? request, CancellationToken cancellationToken)
    {
        var clientId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(clientId) || request is null || request.TargetType is not ("venue" or "provider") || !Guid.TryParse(request.TargetId, out _)) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La conversación es inválida.");
        string targetUserId; string targetName;
        if (request.TargetType == "venue") { var venue = await venues.FindPublishedAsync(request.TargetId!, cancellationToken); if (venue is null) return NotFound(); targetUserId = venue.OwnerId; targetName = venue.Name; }
        else { var provider = await providers.FindPublishedAsync(request.TargetId!, cancellationToken); if (provider is null) return NotFound(); targetUserId = provider.UserId; targetName = provider.BusinessName; }
        var conversation = new Conversation { Id = Guid.NewGuid().ToString("D"), ClientId = clientId, TargetType = request.TargetType, TargetId = request.TargetId!, TargetUserId = targetUserId, TargetName = targetName, QuoteId = request.QuoteId, LastMessage = string.Empty, LastAt = DateTime.UtcNow };
        var created = await conversations.CreateAsync(conversation, cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/conversations/{created.Id}", created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) => !Guid.TryParse(id, out _) ? NotFound() : (await conversations.FindAsync(id, User.FindFirstValue("sub")!, cancellationToken) is { } item ? Ok(item) : NotFound());

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> Messages(string id, CancellationToken cancellationToken) => !Guid.TryParse(id, out _) || await conversations.FindAsync(id, User.FindFirstValue("sub")!, cancellationToken) is null ? NotFound() : Ok(new { data = await conversations.ListMessagesAsync(id, User.FindFirstValue("sub")!, cancellationToken) });

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> AddMessage(string id, CreateMessageRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || request is null || string.IsNullOrWhiteSpace(request.Text) || request.Text.Length > 2000) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "El mensaje es inválido.");
        var message = new ConversationMessage { Id = Guid.NewGuid().ToString("D"), ConversationId = id, SenderId = userId, Text = request.Text.Trim(), SentAt = DateTime.UtcNow };
        var created = await conversations.AddMessageAsync(id, userId, message, cancellationToken);
        return created is null ? NotFound() : Created($"/api/v1/conversations/{id}/messages/{message.Id}", created);
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> Read(string id, CancellationToken cancellationToken) => await conversations.MarkReadAsync(id, User.FindFirstValue("sub")!, cancellationToken) ? Ok(new { id, read = true }) : NotFound();
}
