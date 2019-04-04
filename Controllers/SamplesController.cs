using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.Models;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SamplesController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public SamplesController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sample>>> GetSamples(
        [FromQuery] int? clienteGrd = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = _context.Samples.AsQueryable();

        if (clienteGrd.HasValue)
            query = query.Where(s => s.ClienteGrd == clienteGrd.Value);

        if (startDate.HasValue)
            query = query.Where(s => s.FechaRecep >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.FechaRecep <= endDate.Value);

        var samples = await query
            .OrderByDescending(s => s.FechaRecep)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(samples);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sample>> GetSample(int id)
    {
        var sample = await _context.Samples.FindAsync(id);

        if (sample == null)
            return NotFound();

        return Ok(sample);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<Sample>>> GetPendingSamples()
    {
        var samples = await _context.Samples
            .Where(s => s.FecLibera == null)
            .OrderBy(s => s.FechaRecep)
            .ToListAsync();

        return Ok(samples);
    }
}
