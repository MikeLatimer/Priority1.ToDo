using System.ComponentModel.DataAnnotations;

namespace Priority1.ToDo.Core.Domain;

public class TaskItem : EntityBase
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Todo> Todos { get; set; } = [];

    //TODO: Add entity configurations for relationships, keys, constraints, and column mappings.
}