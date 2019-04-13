using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.DTOs;
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
    public async Task<ActionResult> GetSamples(
        [FromQuery] int? clienteGrd = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination parameters");

        var query = _context.Samples.AsQueryable();

        if (clienteGrd.HasValue)
            query = query.Where(s => s.ClienteGrd == clienteGrd.Value);

        if (startDate.HasValue)
            query = query.Where(s => s.FechaRecep >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.FechaRecep <= endDate.Value);

        var total = await query.CountAsync();

        var samples = await query
            .OrderByDescending(s => s.FechaRecep)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SampleDto
            {
                Id = s.Id,
                FechaGrd = s.FechaGrd,
                FechaRecep = s.FechaRecep,
                FolioGrd = s.FolioGrd,
                ClienteGrd = s.ClienteGrd,
                PacienteGrd = s.PacienteGrd,
                Label1 = s.Label1,
                FecCapRes = s.FecCapRes,
                FecLibera = s.FecLibera,
                SucProc = s.SucProc
            })
            .ToListAsync();

        return Ok(new
        {
            data = samples,
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SampleDto>> GetSample(int id)
    {
        var sample = await _context.Samples
            .Where(s => s.Id == id)
            .Select(s => new SampleDto
            {
                Id = s.Id,
                FechaGrd = s.FechaGrd,
                FechaRecep = s.FechaRecep,
                FolioGrd = s.FolioGrd,
                ClienteGrd = s.ClienteGrd,
                PacienteGrd = s.PacienteGrd,
                Label1 = s.Label1,
                FecCapRes = s.FecCapRes,
                FecLibera = s.FecLibera,
                SucProc = s.SucProc
            })
            .FirstOrDefaultAsync();

        if (sample == null)
            return NotFound(new { message = "Sample not found" });

        return Ok(sample);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<SampleDto>>> GetPendingSamples()
    {
        var samples = await _context.Samples
            .Where(s => s.FecLibera == null)
            .OrderBy(s => s.FechaRecep)
            .Select(s => new SampleDto
            {
                Id = s.Id,
                FechaGrd = s.FechaGrd,
                FechaRecep = s.FechaRecep,
                FolioGrd = s.FolioGrd,
                ClienteGrd = s.ClienteGrd,
                PacienteGrd = s.PacienteGrd,
                Label1 = s.Label1,
                FecCapRes = s.FecCapRes,
                FecLibera = s.FecLibera,
                SucProc = s.SucProc
            })
            .ToListAsync();

        return Ok(samples);
    }
}
