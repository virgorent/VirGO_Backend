using VirGo.Domain.Events;

namespace VirGo.Application.Events;

public interface IEventTaskRepository
{
    Task<IReadOnlyList<EventTask>> ListAsync(string eventId, CancellationToken cancellationToken = default);
    Task<EventTask> CreateAsync(EventTask task, CancellationToken cancellationToken = default);
    Task<EventTask?> UpdateAsync(EventTask task, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string eventId, string taskId, CancellationToken cancellationToken = default);
}

public sealed class EventTaskRequest
{
    public string? Title { get; init; }
    public string? DueDate { get; init; }
    public bool? Done { get; init; }
}
