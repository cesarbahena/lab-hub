using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("collection_routes")]
public class CollectionRoute
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    [NotMapped]
    public int TotalStops => RouteStops?.Count ?? 0;

    [NotMapped]
    public int ActiveStops => RouteStops?.Count(rs => rs.IsActive) ?? 0;
}
