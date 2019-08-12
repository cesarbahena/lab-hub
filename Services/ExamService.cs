using Microsoft.EntityFrameworkCore;
using LIMSApi.Data;
using LIMSApi.DTOs;
using LIMSApi.Models;

namespace LIMSApi.Services;

public class ExamService : IExamService
{
    private readonly LIMSDbContext _context;

    public ExamService(LIMSDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Exam>> GetAllExamsAsync()
    {
        return await _context.Exams.ToListAsync();
    }

    public async Task<Exam?> GetExamByIdAsync(int id)
    {
        return await _context.Exams.FindAsync(id);
    }

    public async Task<IEnumerable<Exam>> GetPendingExamsAsync()
    {
        return await _context.Exams
            .Where(e => e.ValidatedAt == null)
            .OrderBy(e => e.ReceivedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exam>> FilterExamsAsync(
        int? clientId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = _context.Exams.AsQueryable();

        if (clientId.HasValue)
        {
            query = query.Where(e => e.ClientId == clientId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.ReceivedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.ReceivedAt <= endDate.Value);
        }

        return await query.OrderByDescending(e => e.ReceivedAt).ToListAsync();
    }

    public async Task<(List<ExamDto> Exams, int Total, int TotalPages)> GetPaginatedExamsAsync(
        int? clientId, DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
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

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return (exams, total, totalPages);
    }

    public async Task<Dictionary<string, int>> GetExamStatisticsAsync(
        DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Exams.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(e => e.ReceivedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.ReceivedAt <= endDate.Value);

        var total = await query.CountAsync();
        var pending = await query.Where(e => e.ValidatedAt == null).CountAsync();
        var completed = await query.Where(e => e.ValidatedAt != null).CountAsync();

        return new Dictionary<string, int>
        {
            { "total", total },
            { "pending", pending },
            { "completed", completed }
        };
    }

    public async Task<bool> IsExamPendingAsync(int examId)
    {
        var exam = await _context.Exams.FindAsync(examId);
        return exam != null && exam.ValidatedAt == null;
    }
}