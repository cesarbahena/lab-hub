using System.ComponentModel.DataAnnotations;

namespace QuimiOSHub.DTOs;

public class ShiftDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; }
}

public class CreateShiftDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }
}

public class UpdateShiftDto
{
    [MaxLength(100)]
    public string? Name { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public bool? IsActive { get; set; }
}
