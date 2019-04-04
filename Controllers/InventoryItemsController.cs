using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
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
    public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventoryItems(
        [FromQuery] string? category = null,
        [FromQuery] bool? isActive = null)
    {
        var query = _context.InventoryItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(i => i.Category == category);

        if (isActive.HasValue)
            query = query.Where(i => i.IsActive == isActive.Value);

        var items = await query.OrderBy(i => i.Name).ToListAsync();

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
    public async Task<ActionResult<InventoryItem>> CreateInventoryItem(InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventoryItem(int id, InventoryItem item)
    {
        if (id != item.Id)
            return BadRequest();

        _context.Entry(item).State = EntityState.Modified;

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
    public async Task<ActionResult<InventoryMovement>> CreateMovement(int id, InventoryMovement movement)
    {
        var item = await _context.InventoryItems.FindAsync(id);
        if (item == null)
            return NotFound();

        movement.InventoryItemId = id;
        _context.InventoryMovements.Add(movement);

        if (movement.MovementType == "IN")
            item.CurrentStock += movement.Quantity;
        else if (movement.MovementType == "OUT")
            item.CurrentStock -= movement.Quantity;
        else if (movement.MovementType == "ADJUSTMENT")
            item.CurrentStock = movement.Quantity;

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, movement);
    }
}
