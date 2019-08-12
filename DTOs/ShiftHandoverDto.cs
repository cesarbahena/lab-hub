using System.ComponentModel.DataAnnotations;

namespace LIMSApi.DTOs;

public class ShiftHandoverDto
{
    public int Id { get; set; }
    public int ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime HandoverDate { get; set; }
    public string? Notes { get; set; }
    public int PendingExamsCount { get; set; }
    public List<PendingExamDto> PendingExams { get; set; } = new();
}

public class PendingExamDto
{
    public int ExamId { get; set; }
    public int? Folio { get; set; }
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

    public List<CreatePendingExamDto> PendingExams { get; set; } = new();
}

public class CreatePendingExamDto
{
    [Required]
    public int ExamId { get; set; }

    public int? Folio { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }
}