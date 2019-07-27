using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("shift_handovers")]
public class ShiftHandover
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shift_id")]
    [Required]
    public int ShiftId { get; set; }

    [ForeignKey("ShiftId")]
    public Shift Shift { get; set; } = null!;

    [Column("user_id")]
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("handover_date")]
    [Required]
    public DateTime HandoverDate { get; set; }

    [Column("notes")]
    [MaxLength(2000)]
    public string? Notes { get; set; }

    [Column("pending_samples_count")]
    public int PendingSamplesCount { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<PendingSample> PendingSamples { get; set; } = new List<PendingSample>();
}
