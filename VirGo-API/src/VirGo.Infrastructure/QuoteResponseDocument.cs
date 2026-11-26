using MongoDB.Bson;
using VirGo.Domain.Marketplace;

namespace VirGo.Infrastructure;

/// <summary>
/// Forma de <c>quotes.response</c> que exige el validador estricto de <c>db/schema/quotes.json</c>:
/// <c>items</c>, <c>note</c>, <c>subtotal</c>, <c>tax</c>, <c>total</c>, <c>validUntil</c> y <c>answeredAt</c>.
/// La API solo recibe total, anticipo y nota, así que no hay conceptos (<c>items</c> vacío),
/// el impuesto es 0 y el subtotal es el total. El anticipo se guarda como campo adicional.
/// </summary>
public static class QuoteResponseDocument
{
    /// <summary>Límite de <c>response.note</c> en el esquema.</summary>
    public const int MaxNoteLength = 1000;

    /// <summary>Vigencia de la respuesta: la misma ventana de 15 días con la que vencen las cotizaciones.</summary>
    public static readonly TimeSpan Validity = TimeSpan.FromDays(15);

    public static BsonDocument From(QuoteResponse response, DateTime answeredAt)
    {
        var document = new BsonDocument
        {
            ["items"] = new BsonArray(),
            ["note"] = response.Message ?? string.Empty,
            ["subtotal"] = new BsonDecimal128(response.Total),
            ["tax"] = new BsonDecimal128(0m),
            ["total"] = new BsonDecimal128(response.Total),
            ["validUntil"] = new BsonDateTime(answeredAt.Add(Validity)),
            ["answeredAt"] = new BsonDateTime(answeredAt)
        };
        if (response.Deposit is not null)
        {
            document["deposit"] = new BsonDecimal128(response.Deposit.Value);
        }

        return document;
    }

    /// <summary>Lee la forma del esquema y, por compatibilidad, la anterior (<c>message</c> en lugar de <c>note</c>).</summary>
    public static QuoteResponse To(BsonDocument document)
    {
        var note = document.TryGetValue("note", out var noteValue) && noteValue.IsString ? noteValue.AsString
            : document.TryGetValue("message", out var legacy) && legacy.IsString ? legacy.AsString
            : null;
        return new QuoteResponse
        {
            Total = document.GetValue("total", 0m).ToDecimal(),
            Deposit = document.TryGetValue("deposit", out var deposit) && !deposit.IsBsonNull ? deposit.ToDecimal() : null,
            Message = string.IsNullOrEmpty(note) ? null : note
        };
    }
}
