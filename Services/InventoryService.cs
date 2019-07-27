using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Services;

public interface IInventoryService
{
    Task<(bool Success, string Message, decimal? NewStock)> ProcessMovementAsync(
        int itemId, CreateInventoryMovementDto dto);
    Task<List<InventoryItemDto>> GetLowStockItemsAsync();
    Task<bool> IsItemLowStockAsync(int itemId);
}

public class InventoryService : IInventoryService
{
    private readonly QuimiosDbContext _context;

    public InventoryService(QuimiosDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string Message, decimal? NewStock)> ProcessMovementAsync(
        int itemId, CreateInventoryMovementDto dto)
    {
        var item = await _context.InventoryItems.FindAsync(itemId);
        if (item == null)
            return (false, "Inventory item not found", null);

        if (dto.MovementType != "IN" && dto.MovementType != "OUT" && dto.MovementType != "ADJUSTMENT")
            return (false, "Invalid movement type. Use IN, OUT, or ADJUSTMENT", null);

        if (dto.Quantity <= 0)
            return (false, "Quantity must be greater than zero", null);

        if (dto.MovementType == "OUT" && item.CurrentStock < dto.Quantity)
            return (false, $"Insufficient stock. Available: {item.CurrentStock}", null);

        var movement = new InventoryMovement
        {
            InventoryItemId = itemId,
            MovementType = dto.MovementType,
            Quantity = dto.Quantity,
            Reference = dto.Reference,
            Notes = dto.Notes,
            MovementDate = dto.MovementDate,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryMovements.Add(movement);

        if (movement.MovementType == "IN")
            item.CurrentStock += movement.Quantity;
        else if (movement.MovementType == "OUT")
            item.CurrentStock -= movement.Quantity;
        else if (movement.MovementType == "ADJUSTMENT")
            item.CurrentStock = movement.Quantity;

        await _context.SaveChangesAsync();

        return (true, "Movement processed successfully", item.CurrentStock);
    }

    public async Task<List<InventoryItemDto>> GetLowStockItemsAsync()
    {
        var items = await _context.InventoryItems
            .Where(i => i.IsActive && i.MinStock.HasValue && i.CurrentStock <= i.MinStock.Value)
            .OrderBy(i => i.CurrentStock)
            .Select(i => new InventoryItemDto
            {
                Id = i.Id,
                Code = i.Code,
                Name = i.Name,
                Description = i.Description,
                Category = i.Category,
                Unit = i.Unit,
                CurrentStock = i.CurrentStock,
                MinStock = i.MinStock,
                MaxStock = i.MaxStock,
                IsActive = i.IsActive,
                IsLowStock = true
            })
            .ToListAsync();

        return items;
    }

    public async Task<bool> IsItemLowStockAsync(int itemId)
    {
        var item = await _context.InventoryItems.FindAsync(itemId);
        if (item == null || !item.IsActive)
            return false;

        return item.MinStock.HasValue && item.CurrentStock <= item.MinStock.Value;
    }
}
