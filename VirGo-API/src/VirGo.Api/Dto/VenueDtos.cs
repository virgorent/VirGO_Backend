namespace VirGo.Api.Dto;

public sealed record CreateVenueRequest(
    string? Name,
    string? ShortName,
    string? Description,
    string? Address,
    string? Zone,
    VenueLocationRequest? Location,
    int? Capacity,
    VenuePriceRequest? Price,
    VenueServiceRequest[]? Services,
    VenuePhotoRequest[]? Photos,
    string[]? Categories,
    string[]? Amenities);

public sealed record PatchVenueRequest(
    string? Name,
    string? ShortName,
    string? Description,
    string? Address,
    string? Zone,
    VenueLocationRequest? Location,
    int? Capacity,
    VenuePriceRequest? Price,
    VenueServiceRequest[]? Services,
    VenuePhotoRequest[]? Photos,
    string[]? Categories,
    string[]? Amenities);

public sealed record VenueLocationRequest(string? Type, double[]? Coordinates);

public sealed record VenuePriceRequest(decimal? Base, string? Unit, int? MinHours);

public sealed record VenueServiceRequest(string? Name, bool? Included, decimal? Price);

public sealed record VenuePhotoRequest(string? Url, int? Order);

public sealed record CreateVenueBlockRequest(string? Date, string? Reason, string? Note);
