using Microsoft.EntityFrameworkCore;
using Priority1.ToDo.Core.Services.Interfaces;
using Priority1.ToDo.Core.Data;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Core.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Todo>> GetAllAsync(int? taskItemId = null, CancellationToken ct = default)
    {
        var query = _context.Todos.Where(t => t.IsActive);

        if (taskItemId.HasValue)
        {
            query = query.Where(t => t.TaskItemId == taskItemId.Value);
        }

        return await query.ToListAsync(ct);
    }

    public async Task<Todo?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Todos.FirstOrDefaultAsync( t => t.Id == id && t.IsActive,ct);
    }

    public async Task<Todo> CreateAsync(Todo itemToCreate, CancellationToken ct = default)
    {
        _context.Todos.Add(itemToCreate);
        await _context.SaveChangesAsync(ct);
        return itemToCreate;
    }

    public async Task<Todo?> UpdateAsync(Todo itemToUpdate, CancellationToken ct = default)
    {
        var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == itemToUpdate.Id && t.IsActive,ct);

        if (todo is null)
        {
            return null;
        }

        todo.Title = itemToUpdate.Title;
        todo.IsComplete = itemToUpdate.IsComplete;
        todo.TaskItemId = itemToUpdate.TaskItemId;
        todo.DueDate = itemToUpdate.DueDate;

        await _context.SaveChangesAsync(ct);

        return todo;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var todo = await _context.Todos.FirstOrDefaultAsync( t => t.Id == id && t.IsActive,ct);

        if (todo is null)
        {
            return false;
        }

        todo.IsActive = false;

        await _context.SaveChangesAsync(ct);

        return true;
    }
}