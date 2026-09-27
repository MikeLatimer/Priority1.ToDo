using System.ComponentModel.DataAnnotations;

namespace Priority1.ToDo.Core.Domain;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; } = true;
}