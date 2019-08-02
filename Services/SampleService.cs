using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Services;

public class SampleService : ISampleService
{
    private readonly QuimiosDbContext _context;

    public SampleService(QuimiosDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sample>> GetAllSamplesAsync()
    {
        return await _context.Samples.ToListAsync();
    }

    public async Task<Sample?> GetSampleByIdAsync(int id)
    {
        return await _context.Samples.FindAsync(id);
    }

    public async Task<IEnumerable<Sample>> GetPendingSamplesAsync()
    {
        return await _context.Samples
            .Where(s => s.ValidatedAt == null)
            .OrderBy(s => s.ReceivedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sample>> FilterSamplesAsync(
        int? clientId,
        DateTime? startDate,
        DateTime? endDate)
    {
        var query = _context.Samples.AsQueryable();

        if (clientId.HasValue)
        {
            query = query.Where(s => s.ClientId == clientId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(s => s.ReceivedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(s => s.ReceivedAt <= endDate.Value);
        }

        return await query.OrderByDescending(s => s.ReceivedAt).ToListAsync();
    }

    public async Task<(List<SampleDto> Samples, int Total, int TotalPages)> GetPaginatedSamplesAsync(
        int? clientId, DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
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

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return (samples, total, totalPages);
    }

    public async Task<Dictionary<string, int>> GetSampleStatisticsAsync(
        DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Samples.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.ReceivedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.ReceivedAt <= endDate.Value);

        var total = await query.CountAsync();
        var pending = await query.Where(s => s.ValidatedAt == null).CountAsync();
        var completed = await query.Where(s => s.ValidatedAt != null).CountAsync();

        return new Dictionary<string, int>
        {
            { "total", total },
            { "pending", pending },
            { "completed", completed }
        };
    }

    public async Task<bool> IsSamplePendingAsync(int sampleId)
    {
        var sample = await _context.Samples.FindAsync(sampleId);
        return sample != null && sample.ValidatedAt == null;
    }
}
