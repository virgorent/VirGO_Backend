using VirGo.Domain.Marketplace;

namespace VirGo.Application.Marketplace;

public sealed record CreateQuoteCommand(
    string ClientId,
    string ClientName,
    string TargetType,
    string TargetId,
    string EventId,
    string EventType,
    DateOnly EventDate,
    int Guests,
    string[] RequestedServices,
    string? Message);

public sealed record QuoteCreationResult(Quote? Quote, bool InvalidInput);

public sealed record CreateBookingCommand(
    string ClientId,
    string IdempotencyKey,
    string ClientName,
    string? QuoteId,
    string? EventId,
    string VenueId,
    string Date,
    int Hours,
    decimal? Deposit,
    string? CorrelationId = null);

public sealed record BookingCreationResult(Booking? Booking, bool InvalidInput);

public sealed record CreateVenueCommand(
    string OwnerId,
    string Name,
    string ShortName,
    string Description,
    string Address,
    string Zone,
    double[]? Coordinates,
    int Capacity,
    decimal BasePrice,
    string PriceUnit,
    int MinimumHours,
    VenueServiceCommand[] Services,
    VenuePhotoCommand[] Photos,
    string[] Categories,
    string[] Amenities);

public sealed record VenueServiceCommand(string Name, bool Included, decimal Price);

public sealed record VenuePhotoCommand(string Url, int Order);

public sealed record PatchVenueCommand(
    string OwnerId,
    string VenueId,
    string? Name,
    string? ShortName,
    string? Description,
    string? Address,
    string? Zone,
    double[]? Coordinates,
    int? Capacity,
    decimal? BasePrice,
    string? PriceUnit,
    int? MinimumHours,
    VenueServiceCommand[]? Services,
    VenuePhotoCommand[]? Photos,
    string[]? Categories,
    string[]? Amenities);
