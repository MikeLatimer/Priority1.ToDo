using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Core.Services.Interfaces;

public interface ITaskItemService
{
    Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default);

    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<TaskItem> CreateAsync(TaskItem itemToCreate, CancellationToken ct = default);

    Task<TaskItem?> UpdateAsync(TaskItem itemToUpdate, CancellationToken ct = default);

    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}