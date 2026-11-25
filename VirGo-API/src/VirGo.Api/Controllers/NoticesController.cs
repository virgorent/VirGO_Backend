using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Jobs;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/notices")]
public sealed class NoticesController(INoticeRepository notices) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await notices.ListByUserAsync(userId, cancellationToken);
        return Ok(new
        {
            data = result.Select(notice => new
            {
                id = notice.Id,
                kind = notice.Kind,
                title = notice.Title,
                body = notice.Body,
                link = notice.Link,
                read = notice.Read,
                createdAt = notice.CreatedAt,
                purgeAt = notice.PurgeAt
            })
        });
    }

    [HttpPatch("{id}/read")]
    public async Task<IActionResult> MarkRead(string id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var notice = await notices.MarkReadAsync(userId, id, cancellationToken);
        return notice is null
            ? NotFound()
            : Ok(new { id = notice.Id, read = notice.Read });
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllRead(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var updatedCount = await notices.MarkAllReadAsync(userId, cancellationToken);
        return Ok(new { updatedCount });
    }
}
