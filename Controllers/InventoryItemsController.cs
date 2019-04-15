using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.DTOs;
using QuimiosHub.Models;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryItemsController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public InventoryItemsController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetInventoryItems(
        [FromQuery] string? category = null,
        [FromQuery] bool? isActive = null)
    {
        var query = _context.InventoryItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(i => i.Category == category);

        if (isActive.HasValue)
            query = query.Where(i => i.IsActive == isActive.Value);

        var items = await query
            .OrderBy(i => i.Name)
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
                IsLowStock = i.MinStock.HasValue && i.CurrentStock <= i.MinStock.Value
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItem>> GetInventoryItem(int id)
    {
        var item = await _context.InventoryItems
            .Include(i => i.InventoryMovements.OrderByDescending(m => m.MovementDate).Take(10))
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetLowStockItems()
    {
        var items = await _context.InventoryItems
            .Where(i => i.IsActive && i.MinStock.HasValue && i.CurrentStock <= i.MinStock.Value)
            .OrderBy(i => i.CurrentStock)
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDto>> CreateInventoryItem(CreateInventoryItemDto dto)
    {
        if (await _context.InventoryItems.AnyAsync(i => i.Code == dto.Code))
            return BadRequest(new { message = "Item code already exists" });

        var item = new InventoryItem
        {
            Code = dto.Code,
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            Unit = dto.Unit,
            CurrentStock = dto.CurrentStock,
            MinStock = dto.MinStock,
            MaxStock = dto.MaxStock,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        var itemDto = new InventoryItemDto
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            Unit = item.Unit,
            CurrentStock = item.CurrentStock,
            MinStock = item.MinStock,
            MaxStock = item.MaxStock,
            IsActive = item.IsActive,
            IsLowStock = item.MinStock.HasValue && item.CurrentStock <= item.MinStock.Value
        };

        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, itemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryItem(int id, UpdateInventoryItemDto dto)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            return NotFound(new { message = "Inventory item not found" });

        if (dto.Name != null)
            item.Name = dto.Name;

        if (dto.Description != null)
            item.Description = dto.Description;

        if (dto.Category != null)
            item.Category = dto.Category;

        if (dto.Unit != null)
            item.Unit = dto.Unit;

        if (dto.MinStock.HasValue)
            item.MinStock = dto.MinStock.Value;

        if (dto.MaxStock.HasValue)
            item.MaxStock = dto.MaxStock.Value;

        if (dto.IsActive.HasValue)
            item.IsActive = dto.IsActive.Value;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.InventoryItems.AnyAsync(i => i.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpPost("{id}/movements")]
    public async Task<ActionResult> CreateMovement(int id, CreateInventoryMovementDto dto)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            return NotFound(new { message = "Inventory item not found" });

        if (dto.MovementType != "IN" && dto.MovementType != "OUT" && dto.MovementType != "ADJUSTMENT")
            return BadRequest(new { message = "Invalid movement type. Use IN, OUT, or ADJUSTMENT" });

        if (dto.Quantity <= 0)
            return BadRequest(new { message = "Quantity must be greater than zero" });

        var movement = new InventoryMovement
        {
            InventoryItemId = id,
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

        return Ok(new { message = "Movement created successfully", currentStock = item.CurrentStock });
    }
}
