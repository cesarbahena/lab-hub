using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiosHub.Models;

[Table("pending_samples")]
public class PendingSample
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shift_handover_id")]
    [Required]
    public int ShiftHandoverId { get; set; }

    [ForeignKey("ShiftHandoverId")]
    public ShiftHandover ShiftHandover { get; set; } = null!;

    [Column("sample_id")]
    [Required]
    public int SampleId { get; set; }

    [ForeignKey("SampleId")]
    public Sample Sample { get; set; } = null!;

    [Column("reason")]
    [MaxLength(500)]
    public string? Reason { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
