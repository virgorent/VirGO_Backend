using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Api.Dto;
using VirGo.Application.Marketplace;
using VirGo.Domain.Payments;

namespace VirGo.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/payment-cards")]
public sealed class PaymentCardsController(IPaymentCardRepository cards) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) => Ok(new { data = (await cards.ListAsync(User.FindFirstValue("sub")!, cancellationToken)).Select(Safe) });

    [HttpPost]
    public async Task<IActionResult> Add(PaymentCardRequest? request, CancellationToken cancellationToken)
    {
        if (!Valid(request)) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La tarjeta es inválida.");
        var card = new PaymentCard { Id = Guid.NewGuid().ToString("D"), UserId = User.FindFirstValue("sub")!, Brand = request!.Brand!.Trim(), Last4 = request.Last4!.Trim(), ExpMonth = request.ExpMonth, ExpYear = request.ExpYear, GatewayToken = request.GatewayToken!.Trim(), IsDefault = request.IsDefault };
        var created = await cards.AddAsync(card, cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/payment-cards/{card.Id}", Safe(created));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, PaymentCardRequest? request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out _) || !Valid(request)) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "La tarjeta es inválida.");
        var updated = await cards.UpdateAsync(new PaymentCard { Id = id, UserId = User.FindFirstValue("sub")!, Brand = request!.Brand!.Trim(), Last4 = request.Last4!.Trim(), ExpMonth = request.ExpMonth, ExpYear = request.ExpYear, GatewayToken = request.GatewayToken!.Trim(), IsDefault = request.IsDefault }, cancellationToken);
        return updated is null ? NotFound() : Ok(Safe(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) => await cards.DeleteAsync(User.FindFirstValue("sub")!, id, cancellationToken) ? NoContent() : NotFound();

    [HttpPost("{id}/default")]
    public async Task<IActionResult> Default(string id, CancellationToken cancellationToken) { var card = await cards.SetDefaultAsync(User.FindFirstValue("sub")!, id, cancellationToken); return card is null ? NotFound() : Ok(Safe(card)); }

    private static bool Valid(PaymentCardRequest? request) => request is not null && !string.IsNullOrWhiteSpace(request.Brand) && request.Brand.Length <= 20 && request.Last4?.Length == 4 && request.Last4.All(char.IsDigit) && request.ExpMonth is >= 1 and <= 12 && request.ExpYear >= DateTime.UtcNow.Year && !string.IsNullOrWhiteSpace(request.GatewayToken) && request.GatewayToken.Length <= 255;
    private static object Safe(PaymentCard card) => new { id = card.Id, brand = card.Brand, last4 = card.Last4, expMonth = card.ExpMonth, expYear = card.ExpYear, isDefault = card.IsDefault };
}
