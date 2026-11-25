using VirGo.Domain.Gigs;

namespace VirGo.Application.Providers;

public interface IGigRepository
{
    Task<IReadOnlyList<Gig>> ListByProviderAsync(string providerId, CancellationToken cancellationToken = default);
    Task<Gig?> CreateAsync(Gig gig, CancellationToken cancellationToken = default);
    Task<Gig?> UpdateAsync(Gig gig, CancellationToken cancellationToken = default);
}

public sealed class GigCreateRequest
{
    public string? EventName { get; init; }
    public string? ClientName { get; init; }
    public string? Place { get; init; }
    public string? Date { get; init; }
    public string? Start { get; init; }
    public string? End { get; init; }
    public decimal? Amount { get; init; }
    public string? QuoteId { get; init; }
}
