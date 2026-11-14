using System.Text.Json;
using System.Text.Json.Serialization;
using VirGo.Domain.Notifications;

namespace VirGo.Application.Jobs;

public static class JobTypes
{
    public const string BookingCreated = "booking.created";
    public const string EmailSend = "email.send";
    public const string QuotesExpire = "quotes.expire";
}

public sealed record JobMessage(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("data")] IReadOnlyDictionary<string, string>? Data = null,
    [property: JsonPropertyName("correlationId")] string? CorrelationId = null)
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public string Get(string key) =>
        Data is not null && Data.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new InvalidJobMessageException($"The job message has no '{key}'.");

    public string ToJson() => JsonSerializer.Serialize(this, SerializerOptions);

    public static JobMessage Parse(string json)
    {
        try
        {
            var message = JsonSerializer.Deserialize<JobMessage>(json, SerializerOptions);
            return message is null || string.IsNullOrWhiteSpace(message.Type)
                ? throw new InvalidJobMessageException("The job message has no type.")
                : message;
        }
        catch (JsonException exception)
        {
            throw new InvalidJobMessageException("The job message is not valid JSON.", exception);
        }
    }
}

public sealed class InvalidJobMessageException : Exception
{
    public InvalidJobMessageException(string message) : base(message)
    {
    }

    public InvalidJobMessageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

/// <summary>Publishes background work. Implementations never throw: a failed publish is logged (fail-open).</summary>
public interface IJobPublisher
{
    Task PublishAsync(JobMessage message, CancellationToken cancellationToken = default);
}

public interface IJobHandler
{
    string Type { get; }

    Task HandleAsync(JobMessage message, CancellationToken cancellationToken = default);
}

public interface INoticeRepository
{
    /// <summary>Inserts the notice; returns false when a notice with the same id already exists.</summary>
    Task<bool> AddIfAbsentAsync(Notice notice, CancellationToken cancellationToken = default);
}

public interface IQuoteExpiry
{
    /// <summary>Marks as expired the sent or answered quotes whose event date is before <paramref name="today"/>.</summary>
    Task<long> ExpireBeforeAsync(DateOnly today, DateTime now, CancellationToken cancellationToken = default);
}
