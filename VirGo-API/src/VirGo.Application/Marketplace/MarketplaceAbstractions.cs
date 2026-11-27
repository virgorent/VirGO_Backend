using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed class DuplicateBookingIdempotencyException : Exception
{
    public DuplicateBookingIdempotencyException() : base("The booking idempotency key already exists.")
    {
    }
}

public interface IVenueRepository
{
    Task<IReadOnlyList<Venue>> ListPublishedAsync(string? zone, string? category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Venue>> ListOwnedAsync(string ownerId, CancellationToken cancellationToken = default);
    Task<Venue?> FindPublishedAsync(string id, CancellationToken cancellationToken = default);
    Task<Venue> CreateAsync(CreateVenueCommand command, CancellationToken cancellationToken = default);
    Task<Venue?> UpdateAsync(PatchVenueCommand command, CancellationToken cancellationToken = default);
    Task<Venue?> PublishAsync(string venueId, string ownerId, CancellationToken cancellationToken = default);
}

public interface IQuoteRepository
{
    Task AddAsync(Quote quote, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Quote>> ListVisibleAsync(string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default);
    Task<Quote?> FindVisibleAsync(string id, string clientId, IReadOnlyCollection<string> targetIds, CancellationToken cancellationToken = default);
    Task<Quote?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Quote?> UpdateAsync(Quote quote, string expectedStatus, CancellationToken cancellationToken = default);
}

public interface IBookingRepository
{
    Task<IReadOnlyList<Booking>> ListVisibleAsync(string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default);
    Task<Booking?> FindVisibleAsync(string id, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default);
    Task<Booking?> FindByFolioVisibleAsync(string folio, string userId, IReadOnlyCollection<string> venueIds, CancellationToken cancellationToken = default);
    Task<Booking?> FindByIdempotencyKeyAsync(string clientId, string idempotencyKey, CancellationToken cancellationToken = default);
    Task<string> NextFolioAsync(int year, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
}
