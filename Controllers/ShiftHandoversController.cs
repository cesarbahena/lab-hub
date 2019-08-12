using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LIMSApi.Data;
using LIMSApi.DTOs;
using LIMSApi.Models;

namespace LIMSApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftHandoversController : ControllerBase
{
    private readonly LIMSDbContext _context;

    public ShiftHandoversController(LIMSDbContext context)
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
            .Include(sh => sh.PendingExams)
                .ThenInclude(pe => pe.Exam)
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
                PendingExamsCount = sh.PendingExamsCount,
                PendingExams = sh.PendingExams.Select(pe => new PendingExamDto
                {
                    ExamId = pe.ExamId,
                    Folio = pe.Exam.Folio,
                    Reason = pe.Reason
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
            .Include(sh => sh.PendingExams)
                .ThenInclude(pe => pe.Exam)
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
                PendingExamsCount = sh.PendingExamsCount,
                PendingExams = sh.PendingExams.Select(pe => new PendingExamDto
                {
                    ExamId = pe.ExamId,
                    Folio = pe.Exam.Folio,
                    Reason = pe.Reason
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
            PendingExamsCount = dto.PendingExams.Count,
            CreatedAt = DateTime.UtcNow
        };

        _context.ShiftHandovers.Add(handover);
        await _context.SaveChangesAsync();

        foreach (var pe in dto.PendingExams)
        {
            var pendingExam = new PendingExam
            {
                ShiftHandoverId = handover.Id,
                ExamId = pe.ExamId,
                Reason = pe.Reason,
                CreatedAt = DateTime.UtcNow
            };
            _context.PendingExams.Add(pendingExam);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShiftHandover), new { id = handover.Id },
            await GetShiftHandover(handover.Id));
    }
}