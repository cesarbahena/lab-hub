using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiosHub.Models;

[Table("inventory_items")]
public class InventoryItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("name")]
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column("category")]
    [MaxLength(100)]
    public string? Category { get; set; }

    [Column("unit")]
    [MaxLength(50)]
    public string? Unit { get; set; }

    [Column("current_stock")]
    public decimal CurrentStock { get; set; }

    [Column("min_stock")]
    public decimal? MinStock { get; set; }

    [Column("max_stock")]
    public decimal? MaxStock { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    [NotMapped]
    public bool IsLowStock => MinStock.HasValue && CurrentStock <= MinStock.Value;
}
