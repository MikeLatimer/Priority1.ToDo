using System.ComponentModel.DataAnnotations;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models.Requests;

public class UpdateTodoRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; }

    public int TaskItemId { get; set; }

    public DateTime? DueDate { get; set; }

    public Todo ToModel(int id)
    {
        return new Todo
        {
            Id = id,
            Title = Title,
            IsComplete = IsComplete,
            TaskItemId = TaskItemId,
            DueDate = DueDate
        };
    }
}