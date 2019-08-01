using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly QuimiosDbContext _context;
    private readonly ILogger<ShiftsController> _logger;

    public ShiftsController(QuimiosDbContext context, ILogger<ShiftsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShiftDto>>> GetShifts([FromQuery] bool? isActive = null)
    {
        _logger.LogInformation("Fetching shifts (isActive: {IsActive})", isActive);

        var query = _context.Shifts.AsQueryable();

        if (isActive.HasValue)
            query = query.Where(s => s.IsActive == isActive.Value);

        var shifts = await query
            .OrderBy(s => s.StartTime)
            .Select(s => new ShiftDto
            {
                Id = s.Id,
                Name = s.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsActive = s.IsActive
            })
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} shifts", shifts.Count);

        return Ok(shifts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShiftDto>> GetShift(int id)
    {
        _logger.LogInformation("Fetching shift with ID: {Id}", id);

        var shift = await _context.Shifts
            .Where(s => s.Id == id)
            .Select(s => new ShiftDto
            {
                Id = s.Id,
                Name = s.Name,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsActive = s.IsActive
            })
            .FirstOrDefaultAsync();

        if (shift == null)
        {
            _logger.LogWarning("Shift with ID {Id} not found", id);
            return NotFound(new { message = "Shift not found" });
        }

        return Ok(shift);
    }

    [HttpPost]
    public async Task<ActionResult<ShiftDto>> CreateShift(CreateShiftDto dto)
    {
        _logger.LogInformation("Creating new shift: {Name}", dto.Name);

        if (await _context.Shifts.AnyAsync(s => s.Name == dto.Name))
        {
            _logger.LogWarning("Shift with name {Name} already exists", dto.Name);
            return BadRequest(new { message = "Shift name already exists" });
        }

        var shift = new Shift
        {
            Name = dto.Name,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created shift with ID: {Id}", shift.Id);

        var shiftDto = new ShiftDto
        {
            Id = shift.Id,
            Name = shift.Name,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            IsActive = shift.IsActive
        };

        return CreatedAtAction(nameof(GetShift), new { id = shift.Id }, shiftDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShift(int id, UpdateShiftDto dto)
    {
        _logger.LogInformation("Updating shift with ID: {Id}", id);

        var shift = await _context.Shifts.FindAsync(id);
        if (shift == null)
        {
            _logger.LogWarning("Shift with ID {Id} not found", id);
            return NotFound(new { message = "Shift not found" });
        }

        if (dto.Name != null)
            shift.Name = dto.Name;

        if (dto.StartTime.HasValue)
            shift.StartTime = dto.StartTime.Value;

        if (dto.EndTime.HasValue)
            shift.EndTime = dto.EndTime.Value;

        if (dto.IsActive.HasValue)
            shift.IsActive = dto.IsActive.Value;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated shift with ID: {Id}", id);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Shifts.AnyAsync(s => s.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
}
