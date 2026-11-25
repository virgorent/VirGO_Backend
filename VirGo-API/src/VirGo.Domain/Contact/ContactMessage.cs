namespace VirGo.Domain.Contact;

public sealed class ContactMessage
{
    public required string Id { get; init; }
    public string? UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Message { get; init; }
    public required DateTime CreatedAt { get; init; }
}
