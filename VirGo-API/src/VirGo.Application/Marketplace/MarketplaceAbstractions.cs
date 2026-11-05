using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public interface IVenueRepository
{
    Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default);
    Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default);
}

public interface IQuoteRepository
{
    Task AddAsync(Quote quote, CancellationToken cancellationToken = default);
}

public interface IBookingRepository
{
    Task<string> NextFolioAsync(int year, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
}
