using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models;

public class TodoItem
{
    public int Id { get; set; }

    public required string Title { get; set; }

    public bool IsComplete { get; set; }

    public int TaskItemId { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public static TodoItem From(Todo todo)
    {
        return new TodoItem
        {
            Id = todo.Id,
            Title = todo.Title,
            IsComplete = todo.IsComplete,
            TaskItemId = todo.TaskItemId,
            DueDate = todo.DueDate,
            CreateDate = todo.CreateDate,
            UpdateDate = todo.UpdateDate
        };
    }
}