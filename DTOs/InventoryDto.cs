using System.ComponentModel.DataAnnotations;

namespace QuimiosHub.DTOs;

public class InventoryItemDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }
    public bool IsActive { get; set; }
    public bool IsLowStock { get; set; }
}

public class InventoryItemDetailDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }
    public bool IsActive { get; set; }
    public bool IsLowStock { get; set; }
    public List<InventoryMovementDto> RecentMovements { get; set; } = new();
}

public class InventoryMovementDto
{
    public int Id { get; set; }
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime MovementDate { get; set; }
}

public class CreateInventoryItemDto
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    [Required]
    public decimal CurrentStock { get; set; }

    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }
}

public class UpdateInventoryItemDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    public decimal? MinStock { get; set; }
    public decimal? MaxStock { get; set; }
    public bool? IsActive { get; set; }
}

public class CreateInventoryMovementDto
{
    [Required]
    [MaxLength(20)]
    public string MovementType { get; set; } = string.Empty;

    [Required]
    public decimal Quantity { get; set; }

    [MaxLength(200)]
    public string? Reference { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [Required]
    public DateTime MovementDate { get; set; }
}
