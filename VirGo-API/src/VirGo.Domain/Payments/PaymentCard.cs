namespace VirGo.Domain.Payments;

public sealed class PaymentCard
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Brand { get; init; }
    public required string Last4 { get; init; }
    public required int ExpMonth { get; init; }
    public required int ExpYear { get; init; }
    public required string GatewayToken { get; init; }
    public required bool IsDefault { get; init; }
}
