using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckInsController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public CheckInsController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CheckIn>>> GetCheckIns(
        [FromQuery] int? userId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = _context.CheckIns
            .Include(c => c.User)
            .Include(c => c.RouteStop)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(c => c.UserId == userId.Value);

        if (startDate.HasValue)
            query = query.Where(c => c.CheckInTime >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CheckInTime <= endDate.Value);

        var checkIns = await query
            .OrderByDescending(c => c.CheckInTime)
            .ToListAsync();

        return Ok(checkIns);
    }

    [HttpPost]
    public async Task<ActionResult<CheckIn>> CreateCheckIn(CheckIn checkIn)
    {
        _context.CheckIns.Add(checkIn);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCheckIns), new { id = checkIn.Id }, checkIn);
    }
}
