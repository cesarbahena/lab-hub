using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("inventory_movements")]
public class InventoryMovement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("inventory_item_id")]
    [Required]
    public int InventoryItemId { get; set; }

    [ForeignKey("InventoryItemId")]
    public InventoryItem InventoryItem { get; set; } = null!;

    [Column("movement_type")]
    [Required]
    [MaxLength(20)]
    public string MovementType { get; set; } = string.Empty; // IN, OUT, ADJUSTMENT

    [Column("quantity")]
    [Required]
    public decimal Quantity { get; set; }

    [Column("reference")]
    [MaxLength(200)]
    public string? Reference { get; set; }

    [Column("notes")]
    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Column("movement_date")]
    [Required]
    public DateTime MovementDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public bool IsStockIncrease => MovementType == "IN" || MovementType == "ADJUSTMENT" && Quantity > 0;

    [NotMapped]
    public string FormattedMovement => $"{MovementType}: {(IsStockIncrease ? "+" : "")}{Quantity}";
}
