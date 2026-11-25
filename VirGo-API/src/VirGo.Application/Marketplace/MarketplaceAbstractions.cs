using VirGo.Domain.Marketplace;
using VirGo.Domain.Payments;

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
    Task<Booking?> UpdateStatusAsync(string id, string userId, string status, CancellationToken cancellationToken = default);
    Task<Booking?> AddPaymentAsync(string id, string userId, IReadOnlyCollection<string> venueIds, BookingPayment payment, CancellationToken cancellationToken = default);
}

public interface IReviewRepository
{
    Task<IReadOnlyList<Review>> ListAsync(string targetType, string targetId, CancellationToken cancellationToken = default);
    Task<Review?> AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Review?> AddReplyAsync(string id, string targetType, string targetId, ReviewReply reply, CancellationToken cancellationToken = default);
}

public interface IFavoriteRepository
{
    Task<IReadOnlyList<Favorite>> ListAsync(string userId, CancellationToken cancellationToken = default);
    Task<Favorite?> AddAsync(Favorite favorite, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, string targetType, string targetId, CancellationToken cancellationToken = default);
}

public interface IConversationRepository
{
    Task<IReadOnlyList<Conversation>> ListAsync(string userId, CancellationToken cancellationToken = default);
    Task<Conversation?> FindAsync(string id, string userId, CancellationToken cancellationToken = default);
    Task<Conversation?> CreateAsync(Conversation conversation, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConversationMessage>> ListMessagesAsync(string conversationId, string userId, CancellationToken cancellationToken = default);
    Task<ConversationMessage?> AddMessageAsync(string conversationId, string userId, ConversationMessage message, CancellationToken cancellationToken = default);
    Task<bool> MarkReadAsync(string conversationId, string userId, CancellationToken cancellationToken = default);
}

public interface IPaymentCardRepository
{
    Task<IReadOnlyList<PaymentCard>> ListAsync(string userId, CancellationToken cancellationToken = default);
    Task<PaymentCard?> AddAsync(PaymentCard card, CancellationToken cancellationToken = default);
    Task<PaymentCard?> UpdateAsync(PaymentCard card, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string userId, string id, CancellationToken cancellationToken = default);
    Task<PaymentCard?> SetDefaultAsync(string userId, string id, CancellationToken cancellationToken = default);
}
