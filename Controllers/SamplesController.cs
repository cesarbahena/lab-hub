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
        [FromQuery] int? clientId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination parameters");

        var query = _context.Samples.AsQueryable();

        if (clientId.HasValue)
            query = query.Where(s => s.ClientId == clientId.Value);

        if (startDate.HasValue)
            query = query.Where(s => s.ReceivedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.ReceivedAt <= endDate.Value);

        var total = await query.CountAsync();

        var samples = await query
            .OrderByDescending(s => s.ReceivedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new SampleDto
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                ReceivedAt = s.ReceivedAt,
                Folio = s.Folio,
                ClientId = s.ClientId,
                PatientId = s.PatientId,
                ExamId = s.ExamId,
                ExamName = s.ExamName,
                ProcessedAt = s.ProcessedAt,
                ValidatedAt = s.ValidatedAt,
                Location = s.Location,
                Outsourcer = s.Outsourcer,
                Priority = s.Priority,
                BirthDate = s.BirthDate
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
                CreatedAt = s.CreatedAt,
                ReceivedAt = s.ReceivedAt,
                Folio = s.Folio,
                ClientId = s.ClientId,
                PatientId = s.PatientId,
                ExamId = s.ExamId,
                ExamName = s.ExamName,
                ProcessedAt = s.ProcessedAt,
                ValidatedAt = s.ValidatedAt,
                Location = s.Location,
                Outsourcer = s.Outsourcer,
                Priority = s.Priority,
                BirthDate = s.BirthDate
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
            .Where(s => s.ValidatedAt == null)
            .OrderBy(s => s.ReceivedAt)
            .Select(s => new SampleDto
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                ReceivedAt = s.ReceivedAt,
                Folio = s.Folio,
                ClientId = s.ClientId,
                PatientId = s.PatientId,
                ExamId = s.ExamId,
                ExamName = s.ExamName,
                ProcessedAt = s.ProcessedAt,
                ValidatedAt = s.ValidatedAt,
                Location = s.Location,
                Outsourcer = s.Outsourcer,
                Priority = s.Priority,
                BirthDate = s.BirthDate
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

        // Check if sample already exists by Folio (unique identifier)
        if (dto.Folio.HasValue)
        {
            sample = await _context.Samples.FirstOrDefaultAsync(s => s.Folio == dto.Folio.Value);

            if (sample != null)
            {
                // Update existing sample
                isNew = false;
                sample.CreatedAt = ToUtc(dto.CreatedAt);
                sample.ReceivedAt = ToUtc(dto.ReceivedAt);
                sample.ClientId = dto.ClientId;
                sample.PatientId = dto.PatientId;
                sample.ExamId = dto.ExamId;
                sample.ExamName = dto.ExamName;
                sample.ProcessedAt = ToUtc(dto.ProcessedAt);
                sample.ValidatedAt = ToUtc(dto.ValidatedAt);
                sample.Location = dto.Location;
                sample.Outsourcer = dto.Outsourcer;
                sample.Priority = dto.Priority;
                sample.BirthDate = ToUtc(dto.BirthDate);
            }
        }

        // Create new sample if not found
        if (sample == null)
        {
            sample = new Sample
            {
                CreatedAt = ToUtc(dto.CreatedAt),
                ReceivedAt = ToUtc(dto.ReceivedAt),
                Folio = dto.Folio,
                ClientId = dto.ClientId,
                PatientId = dto.PatientId,
                ExamId = dto.ExamId,
                ExamName = dto.ExamName,
                ProcessedAt = ToUtc(dto.ProcessedAt),
                ValidatedAt = ToUtc(dto.ValidatedAt),
                Location = dto.Location,
                Outsourcer = dto.Outsourcer,
                Priority = dto.Priority,
                BirthDate = ToUtc(dto.BirthDate)
            };
            _context.Samples.Add(sample);
        }

        await _context.SaveChangesAsync();

        var result = new SampleDto
        {
            Id = sample.Id,
            CreatedAt = sample.CreatedAt,
            ReceivedAt = sample.ReceivedAt,
            Folio = sample.Folio,
            ClientId = sample.ClientId,
            PatientId = sample.PatientId,
            ExamId = sample.ExamId,
            ExamName = sample.ExamName,
            ProcessedAt = sample.ProcessedAt,
            ValidatedAt = sample.ValidatedAt,
            Location = sample.Location,
            Outsourcer = sample.Outsourcer,
            Priority = sample.Priority,
            BirthDate = sample.BirthDate
        };

        if (isNew)
            return CreatedAtAction(nameof(GetSample), new { id = sample.Id }, result);
        else
            return Ok(result);
    }
}
