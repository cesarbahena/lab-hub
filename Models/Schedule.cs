using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("schedules")]
public class Schedule
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("collection_route_id")]
    [Required]
    public int CollectionRouteId { get; set; }

    [ForeignKey("CollectionRouteId")]
    public CollectionRoute CollectionRoute { get; set; } = null!;

    [Column("user_id")]
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("scheduled_date")]
    [Required]
    public DateTime ScheduledDate { get; set; }

    [Column("start_time")]
    public TimeSpan? StartTime { get; set; }

    [Column("end_time")]
    public TimeSpan? EndTime { get; set; }

    [Column("status")]
    [MaxLength(50)]
    public string Status { get; set; } = "PENDING"; // PENDING, IN_PROGRESS, COMPLETED, CANCELLED

    [Column("notes")]
    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
