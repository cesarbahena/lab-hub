using System.ComponentModel.DataAnnotations;

namespace QuimiOSHub.DTOs;

public class ShiftHandoverDto
{
    public int Id { get; set; }
    public int ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime HandoverDate { get; set; }
    public string? Notes { get; set; }
    public int PendingSamplesCount { get; set; }
    public List<PendingSampleDto> PendingSamples { get; set; } = new();
}

public class PendingSampleDto
{
    public int SampleId { get; set; }
    public int? FolioGrd { get; set; }
    public string? Reason { get; set; }
}

public class CreateShiftHandoverDto
{
    [Required]
    public int ShiftId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public DateTime HandoverDate { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    public List<CreatePendingSampleDto> PendingSamples { get; set; } = new();
}

public class CreatePendingSampleDto
{
    [Required]
    public int SampleId { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}
