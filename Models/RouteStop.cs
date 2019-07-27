using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("route_stops")]
public class RouteStop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("collection_route_id")]
    [Required]
    public int CollectionRouteId { get; set; }

    [ForeignKey("CollectionRouteId")]
    public CollectionRoute CollectionRoute { get; set; } = null!;

    [Column("sequence_order")]
    [Required]
    public int SequenceOrder { get; set; }

    [Column("location_name")]
    [Required]
    [MaxLength(200)]
    public string LocationName { get; set; } = string.Empty;

    [Column("address")]
    [MaxLength(500)]
    public string? Address { get; set; }

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    [Column("contact_name")]
    [MaxLength(200)]
    public string? ContactName { get; set; }

    [Column("contact_phone")]
    [MaxLength(50)]
    public string? ContactPhone { get; set; }

    [Column("notes")]
    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
}
