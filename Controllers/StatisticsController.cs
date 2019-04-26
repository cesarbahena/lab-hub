using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public StatisticsController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult> GetDashboardStats([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;

        var stats = new
        {
            samples = new
            {
                total = await _context.Samples
                    .Where(s => s.FechaRecep >= start && s.FechaRecep <= end)
                    .CountAsync(),
                pending = await _context.Samples
                    .Where(s => s.FechaRecep >= start && s.FechaRecep <= end && s.FecLibera == null)
                    .CountAsync(),
                completed = await _context.Samples
                    .Where(s => s.FechaRecep >= start && s.FechaRecep <= end && s.FecLibera != null)
                    .CountAsync(),
                byClient = await _context.Samples
                    .Where(s => s.FechaRecep >= start && s.FechaRecep <= end)
                    .GroupBy(s => s.ClienteGrd)
                    .Select(g => new { clientId = g.Key, count = g.Count() })
                    .OrderByDescending(x => x.count)
                    .Take(10)
                    .ToListAsync()
            },
            inventory = new
            {
                total = await _context.InventoryItems.CountAsync(i => i.IsActive),
                lowStock = await _context.InventoryItems
                    .CountAsync(i => i.IsActive && i.MinStock.HasValue && i.CurrentStock <= i.MinStock.Value),
                categories = await _context.InventoryItems
                    .Where(i => i.IsActive)
                    .GroupBy(i => i.Category)
                    .Select(g => new { category = g.Key, count = g.Count() })
                    .ToListAsync()
            },
            users = new
            {
                total = await _context.Users.CountAsync(),
                active = await _context.Users.CountAsync(u => u.IsActive)
            },
            shiftHandovers = new
            {
                total = await _context.ShiftHandovers
                    .Where(sh => sh.HandoverDate >= start && sh.HandoverDate <= end)
                    .CountAsync(),
                avgPendingSamples = await _context.ShiftHandovers
                    .Where(sh => sh.HandoverDate >= start && sh.HandoverDate <= end)
                    .AverageAsync(sh => (double?)sh.PendingSamplesCount) ?? 0
            },
            dateRange = new
            {
                start,
                end
            }
        };

        return Ok(stats);
    }

    [HttpGet("samples/daily")]
    public async Task<ActionResult> GetDailySampleStats([FromQuery] int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var dailyStats = await _context.Samples
            .Where(s => s.FechaRecep >= startDate)
            .GroupBy(s => s.FechaRecep.Date)
            .Select(g => new
            {
                date = g.Key,
                total = g.Count(),
                completed = g.Count(s => s.FecLibera != null),
                pending = g.Count(s => s.FecLibera == null)
            })
            .OrderBy(x => x.date)
            .ToListAsync();

        return Ok(dailyStats);
    }

    [HttpGet("inventory/movements")]
    public async Task<ActionResult> GetInventoryMovementStats([FromQuery] int days = 30)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);

        var movementStats = await _context.InventoryMovements
            .Where(m => m.MovementDate >= startDate)
            .GroupBy(m => m.MovementType)
            .Select(g => new
            {
                movementType = g.Key,
                count = g.Count(),
                totalQuantity = g.Sum(m => m.Quantity)
            })
            .ToListAsync();

        return Ok(movementStats);
    }
}
