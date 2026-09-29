using Microsoft.EntityFrameworkCore;
using Priority1.ToDo.Core.Data;
using Priority1.ToDo.Core.Domain;
using Priority1.ToDo.Core.Services.Interfaces;

namespace Priority1.ToDo.Core.Services;

public class TaskItemService : ITaskItemService
{
	private readonly AppDbContext _context;

	public TaskItemService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
	{
		return await _context.TaskItems
			.Where(t => t.IsActive)
			.ToListAsync(ct);
	}

	public async Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
	{
		return await _context.TaskItems
			.FirstOrDefaultAsync(t => t.Id == id && t.IsActive, ct);
	}

	public async Task<TaskItem> CreateAsync(TaskItem itemToCreate, CancellationToken ct = default)
	{
		_context.TaskItems.Add(itemToCreate);

		await _context.SaveChangesAsync(ct);

		return itemToCreate;
	}

	public async Task<TaskItem?> UpdateAsync(TaskItem itemToUpdate, CancellationToken ct = default)
	{
		var taskItem = await _context.TaskItems
			.FirstOrDefaultAsync(t => t.Id == itemToUpdate.Id && t.IsActive, ct);

		if (taskItem is null)
		{
			return null;
		}

		taskItem.Name = itemToUpdate.Name;

		await _context.SaveChangesAsync(ct);

		return taskItem;
	}

	public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
	{
		var taskItem = await _context.TaskItems
			.FirstOrDefaultAsync(t => t.Id == id && t.IsActive, ct);

		if (taskItem is null)
		{
			return false;
		}

		taskItem.IsActive = false;

		await _context.SaveChangesAsync(ct);

		return true;
	}
}