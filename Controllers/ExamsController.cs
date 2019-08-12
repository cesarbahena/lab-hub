using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LIMSApi.Data;
using LIMSApi.DTOs;
using LIMSApi.Models;

namespace LIMSApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly LIMSDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ExamsController(LIMSDbContext context, IWebHostEnvironment env)
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
    public async Task<ActionResult> GetExams(
        [FromQuery] int? clientId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination parameters");

        var query = _context.Exams.AsQueryable();

        if (clientId.HasValue)
            query = query.Where(e => e.ClientId == clientId.Value);

        if (startDate.HasValue)
            query = query.Where(e => e.ReceivedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ReceivedAt <= endDate.Value);

        var total = await query.CountAsync();

        var exams = await query
            .OrderByDescending(e => e.ReceivedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                ReceivedAt = e.ReceivedAt,
                Folio = e.Folio,
                ClientId = e.ClientId,
                PatientId = e.PatientId,
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                ProcessedAt = e.ProcessedAt,
                ValidatedAt = e.ValidatedAt,
                Location = e.Location,
                Outsourcer = e.Outsourcer,
                Priority = e.Priority,
                BirthDate = e.BirthDate
            })
            .ToListAsync();

        return Ok(new
        {
            data = exams,
            page,
            pageSize,
            total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExamDto>> GetExam(int id)
    {
        var exam = await _context.Exams
            .Where(e => e.Id == id)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                ReceivedAt = e.ReceivedAt,
                Folio = e.Folio,
                ClientId = e.ClientId,
                PatientId = e.PatientId,
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                ProcessedAt = e.ProcessedAt,
                ValidatedAt = e.ValidatedAt,
                Location = e.Location,
                Outsourcer = e.Outsourcer,
                Priority = e.Priority,
                BirthDate = e.BirthDate
            })
            .FirstOrDefaultAsync();

        if (exam == null)
            return NotFound(new { message = "Exam not found" });

        return Ok(exam);
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<ExamDto>>> GetPendingExams()
    {
        var exams = await _context.Exams
            .Where(e => e.ValidatedAt == null)
            .OrderBy(e => e.ReceivedAt)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                ReceivedAt = e.ReceivedAt,
                Folio = e.Folio,
                ClientId = e.ClientId,
                PatientId = e.PatientId,
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                ProcessedAt = e.ProcessedAt,
                ValidatedAt = e.ValidatedAt,
                Location = e.Location,
                Outsourcer = e.Outsourcer,
                Priority = e.Priority,
                BirthDate = e.BirthDate
            })
            .ToListAsync();

        return Ok(exams);
    }

    [HttpDelete("partition")]
    public async Task<ActionResult> DeletePartitionExams(
        [FromQuery] string partitionDate,
        [FromQuery] int? clientId = null)
    {
        var query = _context.Exams
            .Where(e => e.ReceivedAt.HasValue && 
                       e.ReceivedAt.Value.Date.ToString("yyyy-MM-dd") == partitionDate);

        if (clientId.HasValue)
            query = query.Where(e => e.ClientId == clientId.Value);

        var count = await query.CountAsync();
        await query.ExecuteDeleteAsync();

        return Ok(new { deleted = count });
    }

    [HttpDelete("dev/clear")]
    public async Task<ActionResult> ClearAllExams()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Exams\" RESTART IDENTITY CASCADE");
        return Ok(new { message = "Development database cleared", count = 0 });
    }

    [HttpPost]
    public async Task<ActionResult<ExamDto>> CreateExam([FromBody] CreateExamDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Exam data is required" });

        Exam? exam = null;
        bool isNew = true;

        // Check if exam already exists by Folio (unique identifier)
        if (dto.Folio.HasValue)
        {
            exam = await _context.Exams.FirstOrDefaultAsync(e => e.Folio == dto.Folio.Value);

            if (exam != null)
            {
                // Update existing exam
                isNew = false;
                exam.CreatedAt = ToUtc(dto.CreatedAt);
                exam.ReceivedAt = ToUtc(dto.ReceivedAt);
                exam.ClientId = dto.ClientId;
                exam.PatientId = dto.PatientId;
                exam.ExamId = dto.ExamId;
                exam.ExamName = dto.ExamName;
                exam.ProcessedAt = ToUtc(dto.ProcessedAt);
                exam.ValidatedAt = ToUtc(dto.ValidatedAt);
                exam.Location = dto.Location;
                exam.Outsourcer = dto.Outsourcer;
                exam.Priority = dto.Priority;
                exam.BirthDate = ToUtc(dto.BirthDate);
            }
        }

        // Create new exam if not found
        if (exam == null)
        {
            exam = new Exam
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
            _context.Exams.Add(exam);
        }

        await _context.SaveChangesAsync();

        var result = new ExamDto
        {
            Id = exam.Id,
            CreatedAt = exam.CreatedAt,
            ReceivedAt = exam.ReceivedAt,
            Folio = exam.Folio,
            ClientId = exam.ClientId,
            PatientId = exam.PatientId,
            ExamId = exam.ExamId,
            ExamName = exam.ExamName,
            ProcessedAt = exam.ProcessedAt,
            ValidatedAt = exam.ValidatedAt,
            Location = exam.Location,
            Outsourcer = exam.Outsourcer,
            Priority = exam.Priority,
            BirthDate = exam.BirthDate
        };

        if (isNew)
            return CreatedAtAction(nameof(GetExam), new { id = exam.Id }, result);
        else
            return Ok(result);
    }
}