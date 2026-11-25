namespace VirGo.Domain.Gigs;

public sealed class Gig
{
    public required string Id { get; init; }
    public required string ProviderId { get; init; }
    public required string EventName { get; init; }
    public required string ClientName { get; init; }
    public required string Place { get; init; }
    public required string Date { get; init; }
    public required string Start { get; init; }
    public required string End { get; init; }
    public decimal? Amount { get; init; }
    public string? QuoteId { get; init; }
}
