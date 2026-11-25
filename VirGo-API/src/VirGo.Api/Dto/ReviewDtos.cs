namespace VirGo.Api.Dto;

public sealed record CreateReviewRequest(string? TargetType, string? TargetId, int Stars, string? Text, string? BookingId);
public sealed record ReviewReplyRequest(string? Text);
