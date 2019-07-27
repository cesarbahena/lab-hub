using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("check_ins")]
public class CheckIn
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("route_stop_id")]
    [Required]
    public int RouteStopId { get; set; }

    [ForeignKey("RouteStopId")]
    public RouteStop RouteStop { get; set; } = null!;

    [Column("user_id")]
    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [Column("check_in_time")]
    [Required]
    public DateTime CheckInTime { get; set; }

    [Column("latitude")]
    [Required]
    public double Latitude { get; set; }

    [Column("longitude")]
    [Required]
    public double Longitude { get; set; }

    [Column("accuracy")]
    public double? Accuracy { get; set; }

    [Column("notes")]
    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
