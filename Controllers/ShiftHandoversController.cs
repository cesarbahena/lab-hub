using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.Models;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftHandoversController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public ShiftHandoversController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShiftHandover>>> GetShiftHandovers(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = _context.ShiftHandovers
            .Include(sh => sh.Shift)
            .Include(sh => sh.User)
            .Include(sh => sh.PendingSamples)
                .ThenInclude(ps => ps.Sample)
            .AsQueryable();

        if (startDate.HasValue)
            query = query.Where(sh => sh.HandoverDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(sh => sh.HandoverDate <= endDate.Value);

        var handovers = await query
            .OrderByDescending(sh => sh.HandoverDate)
            .ToListAsync();

        return Ok(handovers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftHandover>> GetShiftHandover(int id)
    {
        var handover = await _context.ShiftHandovers
            .Include(sh => sh.Shift)
            .Include(sh => sh.User)
            .Include(sh => sh.PendingSamples)
                .ThenInclude(ps => ps.Sample)
            .FirstOrDefaultAsync(sh => sh.Id == id);

        if (handover == null)
            return NotFound();

        return Ok(handover);
    }

    [HttpPost]
    public async Task<ActionResult<ShiftHandover>> CreateShiftHandover(ShiftHandover handover)
    {
        _context.ShiftHandovers.Add(handover);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShiftHandover), new { id = handover.Id }, handover);
    }
}
