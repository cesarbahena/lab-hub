using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.Models;

namespace QuimiosHub.Services;

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
}
