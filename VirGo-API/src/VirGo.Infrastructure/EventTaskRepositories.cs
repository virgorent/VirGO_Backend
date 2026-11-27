using MongoDB.Bson;
using MongoDB.Driver;
using VirGo.Application.Events;
using VirGo.Domain.Events;

namespace VirGo.Infrastructure;

public sealed class InMemoryEventTaskRepository : IEventTaskRepository
{
    private readonly List<EventTask> tasks =
    [new EventTask { Id = "f2222222-2222-4222-8222-222222222222", EventId = "eeeeeeee-eeee-4eee-8eee-eeeeeeeeeeee", Title = "Confirmar proveedores", DueDate = "2027-02-01", Done = false, Suggested = true }];

    public Task<IReadOnlyList<EventTask>> ListAsync(string eventId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (tasks) return Task.FromResult<IReadOnlyList<EventTask>>(tasks.Where(task => task.EventId == eventId).OrderBy(task => task.DueDate).ToArray());
    }

    public Task<EventTask> CreateAsync(EventTask task, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (tasks) tasks.Add(task);
        return Task.FromResult(task);
    }

    public Task<EventTask?> UpdateAsync(EventTask task, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (tasks)
        {
            var index = tasks.FindIndex(current => current.Id == task.Id && current.EventId == task.EventId);
            if (index < 0) return Task.FromResult<EventTask?>(null);
            // "suggested" lo decide el servidor: una edición no lo cambia.
            var updated = new EventTask { Id = task.Id, EventId = task.EventId, Title = task.Title, DueDate = task.DueDate, Done = task.Done, Suggested = tasks[index].Suggested };
            tasks[index] = updated;
            return Task.FromResult<EventTask?>(updated);
        }
    }

    public Task<bool> DeleteAsync(string eventId, string taskId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (tasks) return Task.FromResult(tasks.RemoveAll(task => task.EventId == eventId && task.Id == taskId) == 1);
    }
}

public sealed class MongoEventTaskRepository(IMongoDatabase database) : IEventTaskRepository
{
    private readonly IMongoCollection<BsonDocument> tasks = database.GetCollection<BsonDocument>("eventTasks");

    public async Task<IReadOnlyList<EventTask>> ListAsync(string eventId, CancellationToken cancellationToken = default)
    {
        var documents = await tasks.Find(Builders<BsonDocument>.Filter.Eq("eventId", eventId)).SortBy(document => document["dueDate"]).ToListAsync(cancellationToken);
        return documents.Select(Map).ToArray();
    }

    public async Task<EventTask> CreateAsync(EventTask task, CancellationToken cancellationToken = default)
    {
        await tasks.InsertOneAsync(new BsonDocument { ["_id"] = task.Id, ["eventId"] = task.EventId, ["title"] = task.Title, ["dueDate"] = task.DueDate, ["done"] = task.Done, ["suggested"] = task.Suggested }, cancellationToken: cancellationToken);
        return task;
    }

    public async Task<EventTask?> UpdateAsync(EventTask task, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", task.Id), Builders<BsonDocument>.Filter.Eq("eventId", task.EventId));
        var update = Builders<BsonDocument>.Update.Set("title", task.Title).Set("dueDate", task.DueDate).Set("done", task.Done);
        var document = await tasks.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<BsonDocument> { ReturnDocument = ReturnDocument.After }, cancellationToken);
        return document is null ? null : Map(document);
    }

    public async Task<bool> DeleteAsync(string eventId, string taskId, CancellationToken cancellationToken = default)
    {
        var result = await tasks.DeleteOneAsync(Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("_id", taskId), Builders<BsonDocument>.Filter.Eq("eventId", eventId)), cancellationToken);
        return result.DeletedCount == 1;
    }

    private static EventTask Map(BsonDocument document) => new()
    {
        Id = document["_id"].AsString, EventId = document["eventId"].AsString, Title = document["title"].AsString,
        DueDate = document["dueDate"].AsString, Done = document["done"].AsBoolean, Suggested = document["suggested"].AsBoolean
    };
}
