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
            .Where(s => s.FecLibera == null)
            .OrderBy(s => s.FechaRecep)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sample>> FilterSamplesAsync(
        int? clienteGrd,
        DateTime? fechaInicio,
        DateTime? fechaFin)
    {
        var query = _context.Samples.AsQueryable();

        if (clienteGrd.HasValue)
        {
            query = query.Where(s => s.ClienteGrd == clienteGrd.Value);
        }

        if (fechaInicio.HasValue)
        {
            query = query.Where(s => s.FechaRecep >= fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(s => s.FechaRecep <= fechaFin.Value);
        }

        return await query.OrderByDescending(s => s.FechaRecep).ToListAsync();
    }

    public async Task<(List<SampleDto> Samples, int Total, int TotalPages)> GetPaginatedSamplesAsync(
        int? clienteGrd, DateTime? startDate, DateTime? endDate, int page, int pageSize)
    {
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

        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return (samples, total, totalPages);
    }

    public async Task<Dictionary<string, int>> GetSampleStatisticsAsync(
        DateTime? startDate, DateTime? endDate)
    {
        var query = _context.Samples.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(s => s.FechaRecep >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(s => s.FechaRecep <= endDate.Value);

        var total = await query.CountAsync();
        var pending = await query.Where(s => s.FecLibera == null).CountAsync();
        var completed = await query.Where(s => s.FecLibera != null).CountAsync();

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
        return sample != null && sample.FecLibera == null;
    }
}
