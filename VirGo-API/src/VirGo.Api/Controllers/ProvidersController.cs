using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirGo.Application.Providers;
using VirGo.Application.Auth;
using VirGo.Domain.Providers;
using System.Globalization;

namespace VirGo.Api.Controllers;

[ApiController]
[Route("api/v1/providers")]
public sealed class ProvidersController(IProviderRepository providers, IUserRepository users, IGigRepository gigs) : ControllerBase
{
    private static readonly string[] Categories = ["Catering", "Música", "Fotografía", "Decoración", "Mobiliario"];

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> List(
        [FromQuery] string? category,
        [FromQuery] string? zone,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] decimal? rating,
        [FromQuery(Name = "q")] string? query,
        [FromQuery] string? cursor,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (category is not null && !Categories.Contains(category, StringComparer.Ordinal))
        {
            errors["category"] = ["La categoría no es válida."];
        }
        if (minPrice is < 0 || maxPrice is < 0)
        {
            errors["price"] = ["Los precios no pueden ser negativos."];
        }
        if (minPrice is not null && maxPrice is not null && minPrice > maxPrice)
        {
            errors["price"] = ["minPrice no puede ser mayor que maxPrice."];
        }
        if (rating is < 0 or > 5)
        {
            errors["rating"] = ["La calificación debe estar entre 0 y 5."];
        }
        if (limit is < 1 or > 50)
        {
            errors["limit"] = ["limit debe estar entre 1 y 50."];
        }
        if (cursor is not null && !Guid.TryParse(cursor, out _))
        {
            errors["cursor"] = ["cursor debe ser un UUID válido."];
        }
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        var result = await providers.SearchAsync(
            new ProviderSearch(category, zone, minPrice, maxPrice, rating, query?.Trim(), cursor, limit),
            cancellationToken);
        var data = result.Select(ToResponse).ToArray();

        return Ok(new
        {
            data,
            nextCursor = data.Length == limit ? result[^1].Id : null
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

        var provider = await providers.FindPublishedAsync(parsedId.ToString("D"), cancellationToken);
        return provider is null ? NotFound() : Ok(ToResponse(provider));
    }

    [HttpGet("me")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> Mine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        return provider is null ? NotFound() : Ok(ToOwnerResponse(provider));
    }

    [HttpPost]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> Create(CreateProviderRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var errors = ValidateCreate(request);
        if (errors.Count > 0)
        {
            return ValidationProblem(errors);
        }

        if (await providers.FindByUserAsync(userId, cancellationToken) is not null)
        {
            return Conflict();
        }

        var provider = new Provider
        {
            Id = Guid.NewGuid().ToString("D"),
            UserId = userId,
            BusinessName = request!.BusinessName!.Trim(),
            Category = request.Category!,
            Description = request.Description!.Trim(),
            Zones = request.Zones!,
            Packages = request.Packages!.Select(package => new ProviderPackage(
                package.Id!.Trim(), package.Name!.Trim(), package.Price!.Value, package.Includes!)).ToArray(),
            Gallery = request.Gallery!,
            CoverUrl = request.CoverUrl?.Trim(),
            Pricing = request.Pricing!.Trim(),
            ResponseTime = request.ResponseTime!.Trim(),
            RatingAverage = 0,
            RatingCount = 0,
            Status = "draft"
        };
        var created = await providers.CreateAsync(provider, cancellationToken);
        await users.SetProviderIdAsync(userId, created.Id, DateTime.UtcNow, cancellationToken);

        return CreatedAtAction(nameof(Detail), new { id = created.Id }, ToOwnerResponse(created));
    }

    [HttpPatch("me")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> PatchMine(CreateProviderRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var current = await providers.FindByUserAsync(userId, cancellationToken);
        if (current is null) return NotFound();
        var errors = ValidateCreate(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        var updated = new Provider
        {
            Id = current.Id,
            UserId = current.UserId,
            BusinessName = request!.BusinessName!.Trim(),
            Category = request.Category!,
            Description = request.Description!.Trim(),
            Zones = request.Zones!,
            Packages = request.Packages!.Select(package => new ProviderPackage(package.Id!.Trim(), package.Name!.Trim(), package.Price!.Value, package.Includes!)).ToArray(),
            Gallery = request.Gallery!,
            CoverUrl = request.CoverUrl?.Trim(),
            Pricing = request.Pricing!.Trim(),
            ResponseTime = request.ResponseTime!.Trim(),
            RatingAverage = current.RatingAverage,
            RatingCount = current.RatingCount,
            Status = current.Status
        };
        var result = await providers.UpdateAsync(updated, cancellationToken);
        return result is null ? NotFound() : Ok(ToOwnerResponse(result));
    }

    [HttpGet("me/gigs")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> MyGigs(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        if (provider is null) return NotFound();

        var result = await gigs.ListByProviderAsync(provider.Id, cancellationToken);
        return Ok(new
        {
            data = result.Select(gig => new
            {
                id = gig.Id,
                eventName = gig.EventName,
                clientName = gig.ClientName,
                place = gig.Place,
                date = gig.Date,
                start = gig.Start,
                end = gig.End,
                amount = gig.Amount,
                quoteId = gig.QuoteId
            })
        });
    }

    [HttpPost("me/gigs")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> CreateGig(GigCreateRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        var errors = ValidateGig(request);
        if (errors.Count > 0) return ValidationProblem(errors);
        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        if (provider is null) return NotFound();
        var gig = new VirGo.Domain.Gigs.Gig
        {
            Id = Guid.NewGuid().ToString("D"),
            ProviderId = provider.Id,
            EventName = request!.EventName!.Trim(),
            ClientName = request.ClientName!.Trim(),
            Place = request.Place!.Trim(),
            Date = request.Date!,
            Start = request.Start!,
            End = request.End!,
            Amount = request.Amount,
            QuoteId = request.QuoteId
        };
        var created = await gigs.CreateAsync(gig, cancellationToken);
        return created is null ? Conflict() : Created($"/api/v1/providers/me/gigs/{created.Id}", ToGigResponse(created));
    }

    [HttpPatch("me/gigs/{id}")]
    [Authorize(Roles = "provider")]
    public async Task<IActionResult> UpdateGig(string id, GigCreateRequest? request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();
        if (!Guid.TryParse(id, out var parsedId)) return NotFound();
        var provider = await providers.FindByUserAsync(userId, cancellationToken);
        if (provider is null) return NotFound();
        var errors = ValidateGig(request);
        if (errors.Count > 0) return ValidationProblem(errors);

        var gig = new VirGo.Domain.Gigs.Gig
        {
            Id = parsedId.ToString("D"),
            ProviderId = provider.Id,
            EventName = request!.EventName!.Trim(),
            ClientName = request.ClientName!.Trim(),
            Place = request.Place!.Trim(),
            Date = request.Date!,
            Start = request.Start!,
            End = request.End!,
            Amount = request.Amount,
            QuoteId = request.QuoteId
        };
        var updated = await gigs.UpdateAsync(gig, cancellationToken);
        return updated is null ? NotFound() : Ok(ToGigResponse(updated));
    }

    private static object ToResponse(VirGo.Domain.Providers.Provider provider) => new
    {
        id = provider.Id,
        businessName = provider.BusinessName,
        category = provider.Category,
        description = provider.Description,
        zones = provider.Zones,
        packages = provider.Packages.Select(package => new
        {
            id = package.Id,
            name = package.Name,
            price = package.Price,
            includes = package.Includes
        }),
        gallery = provider.Gallery,
        coverUrl = provider.CoverUrl,
        pricing = provider.Pricing,
        responseTime = provider.ResponseTime,
        rating = new { average = provider.RatingAverage, count = provider.RatingCount }
    };

    private static object ToOwnerResponse(VirGo.Domain.Providers.Provider provider) => new
    {
        id = provider.Id,
        businessName = provider.BusinessName,
        category = provider.Category,
        description = provider.Description,
        zones = provider.Zones,
        packages = provider.Packages.Select(package => new
        {
            id = package.Id,
            name = package.Name,
            price = package.Price,
            includes = package.Includes
        }),
        gallery = provider.Gallery,
        coverUrl = provider.CoverUrl,
        pricing = provider.Pricing,
        responseTime = provider.ResponseTime,
        status = provider.Status,
        rating = new { average = provider.RatingAverage, count = provider.RatingCount }
    };

    private static Dictionary<string, string[]> ValidateCreate(CreateProviderRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null)
        {
            errors["body"] = ["El cuerpo es obligatorio."];
            return errors;
        }
        if (string.IsNullOrWhiteSpace(request.BusinessName) || request.BusinessName.Length > 100) errors["businessName"] = ["businessName es obligatorio y debe tener máximo 100 caracteres."];
        if (request.Category is null || !Categories.Contains(request.Category, StringComparer.Ordinal)) errors["category"] = ["La categoría no es válida."];
        if (string.IsNullOrWhiteSpace(request.Description) || request.Description.Length > 1000) errors["description"] = ["description es obligatorio y debe tener máximo 1000 caracteres."];
        if (request.Zones is null || request.Zones.Length == 0) errors["zones"] = ["Debe indicar al menos una zona."];
        if (request.Packages is null || request.Packages.Length == 0 || request.Packages.Any(package => string.IsNullOrWhiteSpace(package.Id) || string.IsNullOrWhiteSpace(package.Name) || package.Price is < 0 || package.Includes is null)) errors["packages"] = ["Los paquetes no son válidos."];
        if (request.Gallery is null) errors["gallery"] = ["gallery es obligatorio."];
        if (request.CoverUrl?.Length > 500) errors["coverUrl"] = ["coverUrl debe tener máximo 500 caracteres."];
        if (string.IsNullOrWhiteSpace(request.Pricing)) errors["pricing"] = ["pricing es obligatorio."];
        if (string.IsNullOrWhiteSpace(request.ResponseTime)) errors["responseTime"] = ["responseTime es obligatorio."];
        return errors;
    }

    private static Dictionary<string, string[]> ValidateGig(GigCreateRequest? request)
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        if (request is null) { errors["body"] = ["El cuerpo es obligatorio."]; return errors; }
        if (string.IsNullOrWhiteSpace(request.EventName) || request.EventName.Length > 100) errors["eventName"] = ["eventName es obligatorio y debe tener máximo 100 caracteres."];
        if (string.IsNullOrWhiteSpace(request.ClientName) || request.ClientName.Length > 100) errors["clientName"] = ["clientName es obligatorio y debe tener máximo 100 caracteres."];
        if (string.IsNullOrWhiteSpace(request.Place) || request.Place.Length > 200) errors["place"] = ["place es obligatorio y debe tener máximo 200 caracteres."];
        if (!DateTime.TryParseExact(request.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) errors["date"] = ["date debe tener formato yyyy-MM-dd."];
        if (!TimeSpan.TryParseExact(request.Start, @"hh\:mm", CultureInfo.InvariantCulture, out var start)) errors["start"] = ["start debe tener formato HH:mm."];
        if (!TimeSpan.TryParseExact(request.End, @"hh\:mm", CultureInfo.InvariantCulture, out var end)) errors["end"] = ["end debe tener formato HH:mm."];
        if (request.Start is not null && request.End is not null && start >= end) errors["time"] = ["end debe ser posterior a start."];
        if (request.Amount is < 0) errors["amount"] = ["amount no puede ser negativo."];
        if (request.QuoteId is not null && !Guid.TryParse(request.QuoteId, out _)) errors["quoteId"] = ["quoteId debe ser un UUID válido."];
        return errors;
    }

    private static object ToGigResponse(VirGo.Domain.Gigs.Gig gig) => new
    {
        id = gig.Id,
        eventName = gig.EventName,
        clientName = gig.ClientName,
        place = gig.Place,
        date = gig.Date,
        start = gig.Start,
        end = gig.End,
        amount = gig.Amount,
        quoteId = gig.QuoteId
    };

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
