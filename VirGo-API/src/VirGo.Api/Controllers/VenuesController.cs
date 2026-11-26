using System.Security.Claims;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Api.Dto;
using VirGo.Application.Marketplace;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/venues")]
public sealed class VenuesController(
    IVenueRepository venues,
    IVenueAvailabilityRepository availability) : ControllerBase
{
    // Spec 031 RF-03: from y to son inclusivos y el rango cubre como máximo 366 días.
    private const int MaxAvailabilityRangeDays = 366;
    private static readonly string[] Zones = ["Juriquilla", "Centro", "El Refugio", "Santa Fe", "Antea", "Jurica", "Corregidora", "El Marqués"];
    private static readonly string[] Categories = ["Salon", "Jardin", "Terraza", "Hacienda", "Rooftop", "Salon de eventos"];
    private static readonly string[] Amenities = ["Estacionamiento", "Cocina", "Aire acondicionado", "Pista de baile", "Sonido", "Pantalla LED"];

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List([FromQuery] string? zone, [FromQuery] string? category, CancellationToken cancellationToken)
    {
        var result = await venues.ListPublishedAsync(zone, category, cancellationToken);
        return Ok(new
        {
            data = result.Select(venue => new
            {
                id = venue.Id,
                name = venue.Name,
                shortName = venue.ShortName,
                description = venue.Description,
                address = venue.Address,
                zone = venue.Zone,
                capacity = venue.Capacity,
                price = new { baseAmount = venue.BasePrice, unit = venue.PriceUnit },
                categories = venue.Categories,
                amenities = venue.Amenities,
                photos = venue.Photos,
                rating = new { average = venue.RatingAverage, count = venue.RatingCount }
            })
        });
    }

    [HttpGet("mine")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Mine(CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            return Unauthorized();
        }

        var result = await venues.ListOwnedAsync(ownerId, cancellationToken);
        return Ok(new
        {
            data = result.Select(venue => new
            {
                id = venue.Id,
                name = venue.Name,
                shortName = venue.ShortName,
                description = venue.Description,
                address = venue.Address,
                zone = venue.Zone,
                capacity = venue.Capacity,
                minimumHours = venue.MinimumHours,
                price = new { baseAmount = venue.BasePrice, unit = venue.PriceUnit },
                categories = venue.Categories,
                amenities = venue.Amenities,
                photos = venue.Photos,
                status = venue.Status,
                rating = new { average = venue.RatingAverage, count = venue.RatingCount }
            })
        });
    }

    [HttpPost]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Create(CreateVenueRequest? request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            return Unauthorized();
        }

        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var command = new CreateVenueCommand(
            ownerId,
            request!.Name!.Trim(),
            request.ShortName!.Trim(),
            request.Description!.Trim(),
            request.Address!.Trim(),
            request.Zone!,
            request.Location?.Coordinates,
            request.Capacity!.Value,
            request.Price!.Base!.Value,
            request.Price.Unit!,
            request.Price.MinHours!.Value,
            request.Services!.Select(service => new VenueServiceCommand(
                service.Name!.Trim(), service.Included!.Value, service.Price!.Value)).ToArray(),
            request.Photos?.Select(photo => new VenuePhotoCommand(photo.Url!.Trim(), photo.Order!.Value)).ToArray() ?? [],
            request.Categories!,
            request.Amenities!);

        var venue = await venues.CreateAsync(command, cancellationToken);
        return CreatedAtAction(nameof(Detail), new { id = venue.Id }, new
        {
            id = venue.Id,
            name = venue.Name,
            shortName = venue.ShortName,
            description = venue.Description,
            address = venue.Address,
            zone = venue.Zone,
            capacity = venue.Capacity,
            minimumHours = venue.MinimumHours,
            price = new { baseAmount = venue.BasePrice, unit = venue.PriceUnit },
            categories = venue.Categories,
            amenities = venue.Amenities,
            photos = venue.Photos,
            status = venue.Status,
            rating = new { average = venue.RatingAverage, count = venue.RatingCount }
        });
    }

    [HttpPost("{id}/publish")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Publish(string id, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(ownerId) || !Guid.TryParse(id, out var parsedId))
        {
            return NotFound();
        }

        var venue = await venues.PublishAsync(parsedId.ToString("D"), ownerId, cancellationToken);
        return venue is null
            ? NotFound()
            : Ok(new { id = venue.Id, status = venue.Status });
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "owner")]
    public async Task<IActionResult> Patch(string id, PatchVenueRequest? request, CancellationToken cancellationToken)
    {
        var ownerId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(ownerId))
        {
            return Unauthorized();
        }
        if (!Guid.TryParse(id, out var parsedId))
        {
            return NotFound();
        }

        var errors = ValidatePatch(request);
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var command = new PatchVenueCommand(
            ownerId,
            parsedId.ToString("D"),
            request!.Name?.Trim(),
            request.ShortName?.Trim(),
            request.Description?.Trim(),
            request.Address?.Trim(),
            request.Zone,
            request.Location?.Coordinates,
            request.Capacity,
            request.Price?.Base,
            request.Price?.Unit,
            request.Price?.MinHours,
            request.Services?.Select(service => new VenueServiceCommand(
                service.Name!.Trim(), service.Included!.Value, service.Price!.Value)).ToArray(),
            request.Photos?.Select(photo => new VenuePhotoCommand(photo.Url!.Trim(), photo.Order!.Value)).ToArray(),
            request.Categories,
            request.Amenities);

        var venue = await venues.UpdateAsync(command, cancellationToken);
        if (venue is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = venue.Id,
            name = venue.Name,
            shortName = venue.ShortName,
            description = venue.Description,
            address = venue.Address,
            zone = venue.Zone,
            capacity = venue.Capacity,
            minimumHours = venue.MinimumHours,
            price = new { baseAmount = venue.BasePrice, unit = venue.PriceUnit },
            categories = venue.Categories,
            amenities = venue.Amenities,
            photos = venue.Photos,
            status = venue.Status,
            rating = new { average = venue.RatingAverage, count = venue.RatingCount }
        });
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Detail(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            return NotFound();
        }

        var venue = await venues.FindPublishedAsync(parsedId.ToString(), cancellationToken);
        if (venue is null)
        {
            return NotFound();
        }

        var response = new Dictionary<string, object?>
        {
            ["id"] = venue.Id,
            ["name"] = venue.Name,
            ["shortName"] = venue.ShortName,
            ["description"] = venue.Description,
            ["address"] = venue.Address,
            ["zone"] = venue.Zone,
            ["capacity"] = venue.Capacity,
            ["minimumHours"] = venue.MinimumHours,
            ["categories"] = venue.Categories,
            ["amenities"] = venue.Amenities,
            ["photos"] = venue.Photos,
            ["rating"] = new { average = venue.RatingAverage, count = venue.RatingCount }
        };
        if (User.Identity?.IsAuthenticated == true)
        {
            response["price"] = new { baseAmount = venue.BasePrice, unit = venue.PriceUnit };
        }

        return Ok(response);
    }

    [HttpGet("{id}/availability")]
    [AllowAnonymous]
    public async Task<IActionResult> Availability(
        string id,
        [FromQuery(Name = "from")] string? fromValue,
        [FromQuery(Name = "to")] string? toValue,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var parsedId))
        {
            return NotFound();
        }

        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (!DateOnly.TryParseExact(fromValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var from))
        {
            errors["from"] = ["La fecha inicial debe usar el formato yyyy-MM-dd."];
        }
        if (!DateOnly.TryParseExact(toValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var to))
        {
            errors["to"] = ["La fecha final debe usar el formato yyyy-MM-dd."];
        }
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }
        if (from > to)
        {
            return ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                ["range"] = ["La fecha inicial no puede ser posterior a la fecha final."]
            });
        }
        if (to.DayNumber - from.DayNumber + 1 > MaxAvailabilityRangeDays)
        {
            return ValidationProblem(new Dictionary<string, string[]>(StringComparer.Ordinal)
            {
                ["range"] = [$"El rango no puede superar {MaxAvailabilityRangeDays} días, contando ambas fechas."]
            });
        }

        var venue = await venues.FindPublishedAsync(parsedId.ToString("D"), cancellationToken);
        if (venue is null)
        {
            return NotFound();
        }

        var dates = await availability.ListAsync(venue.Id, from, to, cancellationToken);
        return Ok(new
        {
            data = dates.Select(date => new
            {
                date = date.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                kind = date.Kind,
                reason = date.Reason
            })
        });
    }

    [HttpPost("{id}/blocks")]
    [Authorize]
    public async Task<IActionResult> Block(string id, CreateVenueBlockRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId) || request is null || !DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out var date) || request.Reason is not ("maintenance" or "own_event" or "other") || request.Note?.Length > 200) return Problem(statusCode: StatusCodes.Status400BadRequest, title: "El bloqueo es inválido.");
        var owned = (await venues.ListOwnedAsync(userId, cancellationToken)).Any(venue => venue.Id == parsedId.ToString("D"));
        if (!owned) return NotFound();
        var created = await availability.AddBlockAsync(parsedId.ToString("D"), new VenueBlockRequest(date, request.Reason, request.Note), cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/venues/{id}/blocks/{created.Id}", new { id = created.Id, date = created.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), kind = created.Kind, reason = created.Reason, note = created.Note });
    }

    [HttpDelete("{id}/blocks/{blockId}")]
    [Authorize]
    public async Task<IActionResult> DeleteBlock(string id, string blockId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(id, out var parsedId)) return NotFound();
        if (!(await venues.ListOwnedAsync(userId, cancellationToken)).Any(venue => venue.Id == parsedId.ToString("D"))) return NotFound();
        return await availability.DeleteBlockAsync(parsedId.ToString("D"), blockId, cancellationToken) ? NoContent() : NotFound();
    }

    private static Dictionary<string, string[]> Validate(CreateVenueRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null)
        {
            errors["body"] = ["El cuerpo es obligatorio."];
            return errors;
        }

        ValidateText(errors, "name", request.Name, 1, 100);
        ValidateText(errors, "shortName", request.ShortName, 1, 20);
        ValidateText(errors, "description", request.Description, 20, 2000);
        ValidateText(errors, "address", request.Address, 1, 200);
        if (request.Zone is null || !Zones.Contains(request.Zone, StringComparer.Ordinal))
        {
            errors["zone"] = ["La zona no es válida."];
        }
        if (request.Location is not null
            && (!string.Equals(request.Location.Type, "Point", StringComparison.Ordinal)
                || request.Location.Coordinates is null
                || request.Location.Coordinates.Length != 2
                || request.Location.Coordinates.Any(value => !double.IsFinite(value))))
        {
            errors["location"] = ["La ubicación debe ser un punto con exactamente dos coordenadas numéricas."];
        }
        if (request.Capacity is null or < 1)
        {
            errors["capacity"] = ["La capacidad debe ser al menos 1."];
        }
        if (request.Price is null)
        {
            errors["price"] = ["El precio es obligatorio."];
        }
        else
        {
            if (request.Price.Base is null || request.Price.Base < 0)
            {
                errors["price.base"] = ["El precio base debe ser mayor o igual a cero."];
            }
            if (request.Price.Unit is not ("hour" or "day"))
            {
                errors["price.unit"] = ["La unidad debe ser hour o day."];
            }
            if (request.Price.MinHours is null or < 1)
            {
                errors["price.minHours"] = ["Las horas mínimas deben ser al menos 1."];
            }
        }
        if (request.Services is null)
        {
            errors["services"] = ["Los servicios son obligatorios."];
        }
        else
        {
            for (var index = 0; index < request.Services.Length; index++)
            {
                var service = request.Services[index];
                if (string.IsNullOrWhiteSpace(service.Name) || service.Name.Trim().Length > 100)
                {
                    errors[$"services[{index}].name"] = ["El nombre del servicio es obligatorio y no puede superar 100 caracteres."];
                }
                if (service.Included is null)
                {
                    errors[$"services[{index}].included"] = ["Debe indicar si el servicio está incluido."];
                }
                if (service.Price is null || service.Price < 0)
                {
                    errors[$"services[{index}].price"] = ["El precio del servicio debe ser mayor o igual a cero."];
                }
            }
        }
        if (request.Photos is not null)
        {
            for (var index = 0; index < request.Photos.Length; index++)
            {
                var photo = request.Photos[index];
                if (string.IsNullOrWhiteSpace(photo.Url) || photo.Url.Trim().Length > 500)
                {
                    errors[$"photos[{index}].url"] = ["La URL de la foto es obligatoria y no puede superar 500 caracteres."];
                }
                if (photo.Order is null or < 0)
                {
                    errors[$"photos[{index}].order"] = ["El orden de la foto no puede ser negativo."];
                }
            }
        }
        ValidateEnumArray(errors, "categories", request.Categories, Categories);
        ValidateEnumArray(errors, "amenities", request.Amenities, Amenities);
        return errors;
    }

    private static Dictionary<string, string[]> ValidatePatch(PatchVenueRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null)
        {
            errors["body"] = ["El cuerpo es obligatorio."];
            return errors;
        }

        if (request.Name is null && request.ShortName is null && request.Description is null && request.Address is null
            && request.Zone is null && request.Location is null && request.Capacity is null && request.Price is null
            && request.Services is null && request.Photos is null && request.Categories is null && request.Amenities is null)
        {
            errors["body"] = ["Debe proporcionar al menos un campo actualizable."];
            return errors;
        }
        if (request.Name is not null) ValidateText(errors, "name", request.Name, 1, 100);
        if (request.ShortName is not null) ValidateText(errors, "shortName", request.ShortName, 1, 20);
        if (request.Description is not null) ValidateText(errors, "description", request.Description, 20, 2000);
        if (request.Address is not null) ValidateText(errors, "address", request.Address, 1, 200);
        if (request.Zone is not null && !Zones.Contains(request.Zone, StringComparer.Ordinal))
        {
            errors["zone"] = ["La zona no es válida."];
        }
        if (request.Location is not null
            && (!string.Equals(request.Location.Type, "Point", StringComparison.Ordinal)
                || request.Location.Coordinates is null
                || request.Location.Coordinates.Length != 2
                || request.Location.Coordinates.Any(value => !double.IsFinite(value))))
        {
            errors["location"] = ["La ubicación debe ser un punto con exactamente dos coordenadas numéricas."];
        }
        if (request.Capacity is < 1)
        {
            errors["capacity"] = ["La capacidad debe ser al menos 1."];
        }
        if (request.Price is not null)
        {
            if (request.Price.Base is null || request.Price.Base < 0)
            {
                errors["price.base"] = ["El precio base debe ser mayor o igual a cero."];
            }
            if (request.Price.Unit is not ("hour" or "day"))
            {
                errors["price.unit"] = ["La unidad debe ser hour o day."];
            }
            if (request.Price.MinHours is null or < 1)
            {
                errors["price.minHours"] = ["Las horas mínimas deben ser al menos 1."];
            }
        }
        if (request.Services is not null)
        {
            for (var index = 0; index < request.Services.Length; index++)
            {
                var service = request.Services[index];
                if (string.IsNullOrWhiteSpace(service.Name) || service.Name.Trim().Length > 100)
                {
                    errors[$"services[{index}].name"] = ["El nombre del servicio es obligatorio y no puede superar 100 caracteres."];
                }
                if (service.Included is null)
                {
                    errors[$"services[{index}].included"] = ["Debe indicar si el servicio está incluido."];
                }
                if (service.Price is null || service.Price < 0)
                {
                    errors[$"services[{index}].price"] = ["El precio del servicio debe ser mayor o igual a cero."];
                }
            }
        }
        if (request.Photos is not null)
        {
            for (var index = 0; index < request.Photos.Length; index++)
            {
                var photo = request.Photos[index];
                if (string.IsNullOrWhiteSpace(photo.Url) || photo.Url.Trim().Length > 500)
                {
                    errors[$"photos[{index}].url"] = ["La URL de la foto es obligatoria y no puede superar 500 caracteres."];
                }
                if (photo.Order is null or < 0)
                {
                    errors[$"photos[{index}].order"] = ["El orden de la foto no puede ser negativo."];
                }
            }
        }
        if (request.Categories is not null) ValidateEnumArray(errors, "categories", request.Categories, Categories);
        if (request.Amenities is not null) ValidateEnumArray(errors, "amenities", request.Amenities, Amenities);
        return errors;
    }

    private static void ValidateText(Dictionary<string, string[]> errors, string field, string? value, int minimum, int maximum)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < minimum || value.Trim().Length > maximum)
        {
            errors[field] = [$"El campo debe tener entre {minimum} y {maximum} caracteres."];
        }
    }

    private static void ValidateEnumArray(Dictionary<string, string[]> errors, string field, string[]? values, string[] allowed)
    {
        if (values is null || values.Any(value => !allowed.Contains(value, StringComparer.Ordinal)))
        {
            errors[field] = ["La lista contiene valores no válidos."];
        }
    }

    private ObjectResult ValidationProblem(Dictionary<string, string[]> errors)
    {
        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "La solicitud tiene datos inválidos.",
            Instance = Request.Path
        };
        problem.Extensions["traceId"] = HttpContext.TraceIdentifier;
        return new ObjectResult(problem)
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ContentTypes = ["application/problem+json"]
        };
    }
}
