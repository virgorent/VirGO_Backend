using VirGo.Domain.Assistant;

namespace VirGo.Application.Assistant;

public interface IAssistantMessageRepository
{
    Task<IReadOnlyList<AssistantMessage>> ListByUserAsync(
        string userId,
        DateTime now,
        CancellationToken cancellationToken = default);
    Task<AssistantMessage> AddAsync(AssistantMessage message, CancellationToken cancellationToken = default);
}
