using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SamplesController : ControllerBase
{
    private readonly QuimiosDbContext _context;
    private readonly IWebHostEnvironment _env;

    public SamplesController(QuimiosDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    private static DateTime? ToUtc(DateTime? dateTime)
    {
        if (!dateTime.HasValue) return null;
        if (dateTime.Value.Kind == DateTimeKind.Utc) return dateTime.Value;
        if (dateTime.Value.Kind == DateTimeKind.Local) return dateTime.Value.ToUniversalTime();
        // Unspecified - assume UTC
        return DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);
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

    [HttpDelete("dev/clear")]
    public async Task<ActionResult> ClearAllSamples()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Samples\" RESTART IDENTITY CASCADE");
        return Ok(new { message = "Development database cleared", count = 0 });
    }

    [HttpPost]
    public async Task<ActionResult<SampleDto>> CreateSample([FromBody] CreateSampleDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Sample data is required" });

        Sample? sample = null;
        bool isNew = true;

        // Check if sample already exists by FolioGrd (unique identifier)
        if (dto.FolioGrd.HasValue)
        {
            sample = await _context.Samples.FirstOrDefaultAsync(s => s.FolioGrd == dto.FolioGrd.Value);

            if (sample != null)
            {
                // Update existing sample
                isNew = false;
                sample.FechaGrd = ToUtc(dto.FechaGrd);
                sample.FechaRecep = ToUtc(dto.FechaRecep);
                sample.ClienteGrd = dto.ClienteGrd;
                sample.PacienteGrd = dto.PacienteGrd;
                sample.EstPerGrd = dto.EstPerGrd;
                sample.Label1 = dto.Label1;
                sample.FecCapRes = ToUtc(dto.FecCapRes);
                sample.FecLibera = ToUtc(dto.FecLibera);
                sample.SucProc = dto.SucProc;
                sample.Maquilador = dto.Maquilador;
                sample.Label3 = dto.Label3;
                sample.FecNac = ToUtc(dto.FecNac);
            }
        }

        // Create new sample if not found
        if (sample == null)
        {
            sample = new Sample
            {
                FechaGrd = ToUtc(dto.FechaGrd),
                FechaRecep = ToUtc(dto.FechaRecep),
                FolioGrd = dto.FolioGrd,
                ClienteGrd = dto.ClienteGrd,
                PacienteGrd = dto.PacienteGrd,
                EstPerGrd = dto.EstPerGrd,
                Label1 = dto.Label1,
                FecCapRes = ToUtc(dto.FecCapRes),
                FecLibera = ToUtc(dto.FecLibera),
                SucProc = dto.SucProc,
                Maquilador = dto.Maquilador,
                Label3 = dto.Label3,
                FecNac = ToUtc(dto.FecNac),
                CreatedAt = DateTime.UtcNow
            };
            _context.Samples.Add(sample);
        }

        await _context.SaveChangesAsync();

        var result = new SampleDto
        {
            Id = sample.Id,
            FechaGrd = sample.FechaGrd,
            FechaRecep = sample.FechaRecep,
            FolioGrd = sample.FolioGrd,
            ClienteGrd = sample.ClienteGrd,
            PacienteGrd = sample.PacienteGrd,
            Label1 = sample.Label1,
            FecCapRes = sample.FecCapRes,
            FecLibera = sample.FecLibera,
            SucProc = sample.SucProc
        };

        if (isNew)
            return CreatedAtAction(nameof(GetSample), new { id = sample.Id }, result);
        else
            return Ok(result);
    }
}
