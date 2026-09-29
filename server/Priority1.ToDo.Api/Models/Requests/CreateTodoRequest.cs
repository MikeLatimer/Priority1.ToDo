using System.ComponentModel.DataAnnotations;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Api.Models.Requests;

public class CreateTodoRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public bool IsComplete { get; set; } = false;

    public int TaskItemId { get; set; }

    public DateTime? DueDate { get; set; }

    public Todo ToModel()
    {
        return new Todo
        {
            Title = Title,
            IsComplete = IsComplete,
            TaskItemId = TaskItemId,
            DueDate = DueDate
        };
    }
}