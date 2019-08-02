using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

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
    public async Task<ActionResult<IEnumerable<ShiftHandoverDto>>> GetShiftHandovers(
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
            .Select(sh => new ShiftHandoverDto
            {
                Id = sh.Id,
                ShiftId = sh.ShiftId,
                ShiftName = sh.Shift.Name,
                UserId = sh.UserId,
                UserName = sh.User.FullName,
                HandoverDate = sh.HandoverDate,
                Notes = sh.Notes,
                PendingSamplesCount = sh.PendingSamplesCount,
                PendingSamples = sh.PendingSamples.Select(ps => new PendingSampleDto
                {
                    SampleId = ps.SampleId,
                    Folio = ps.Sample.Folio,
                    Reason = ps.Reason
                }).ToList()
            })
            .ToListAsync();

        return Ok(handovers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftHandoverDto>> GetShiftHandover(int id)
    {
        var handover = await _context.ShiftHandovers
            .Include(sh => sh.Shift)
            .Include(sh => sh.User)
            .Include(sh => sh.PendingSamples)
                .ThenInclude(ps => ps.Sample)
            .Where(sh => sh.Id == id)
            .Select(sh => new ShiftHandoverDto
            {
                Id = sh.Id,
                ShiftId = sh.ShiftId,
                ShiftName = sh.Shift.Name,
                UserId = sh.UserId,
                UserName = sh.User.FullName,
                HandoverDate = sh.HandoverDate,
                Notes = sh.Notes,
                PendingSamplesCount = sh.PendingSamplesCount,
                PendingSamples = sh.PendingSamples.Select(ps => new PendingSampleDto
                {
                    SampleId = ps.SampleId,
                    Folio = ps.Sample.Folio,
                    Reason = ps.Reason
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (handover == null)
            return NotFound(new { message = "Shift handover not found" });

        return Ok(handover);
    }

    [HttpPost]
    public async Task<ActionResult<ShiftHandoverDto>> CreateShiftHandover(CreateShiftHandoverDto dto)
    {
        if (!await _context.Shifts.AnyAsync(s => s.Id == dto.ShiftId))
            return BadRequest(new { message = "Invalid shift ID" });

        if (!await _context.Users.AnyAsync(u => u.Id == dto.UserId))
            return BadRequest(new { message = "Invalid user ID" });

        var handover = new ShiftHandover
        {
            ShiftId = dto.ShiftId,
            UserId = dto.UserId,
            HandoverDate = dto.HandoverDate,
            Notes = dto.Notes,
            PendingSamplesCount = dto.PendingSamples.Count,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShiftHandovers.Add(handover);
        await _context.SaveChangesAsync();

        foreach (var ps in dto.PendingSamples)
        {
            var pendingSample = new PendingSample
            {
                ShiftHandoverId = handover.Id,
                SampleId = ps.SampleId,
                Reason = ps.Reason,
                CreatedAt = DateTime.UtcNow
            };
            _context.PendingSamples.Add(pendingSample);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShiftHandover), new { id = handover.Id },
            await GetShiftHandover(handover.Id));
    }
}
