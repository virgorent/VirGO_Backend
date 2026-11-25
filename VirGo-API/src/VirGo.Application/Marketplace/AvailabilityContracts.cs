namespace VirGo.Application.Marketplace;

public sealed record VenueDateRecord(
    string VenueId,
    DateOnly Date,
    string Kind,
    string? Reason,
    string? Note,
    string? BookingId)
{
    public string? Id { get; init; }
}

public sealed record VenueBlockRequest(DateOnly Date, string? Reason, string? Note);

public interface IVenueAvailabilityRepository
{
    Task<IReadOnlyList<VenueDateRecord>> ListAsync(
        string venueId,
        DateOnly from,
        DateOnly toDate,
        CancellationToken cancellationToken = default);
    Task<VenueDateRecord?> AddBlockAsync(string venueId, VenueBlockRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteBlockAsync(string venueId, string blockId, CancellationToken cancellationToken = default);
}
