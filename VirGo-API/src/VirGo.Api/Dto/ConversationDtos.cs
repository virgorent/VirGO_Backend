namespace VirGo.Api.Dto;

public sealed record CreateConversationRequest(string? TargetType, string? TargetId, string? QuoteId);
public sealed record CreateMessageRequest(string? Text);
