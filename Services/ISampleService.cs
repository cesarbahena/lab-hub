using QuimiosHub.Models;

namespace QuimiosHub.Services;

public interface ISampleService
{
    Task<IEnumerable<Sample>> GetAllSamplesAsync();
    Task<Sample?> GetSampleByIdAsync(int id);
    Task<IEnumerable<Sample>> GetPendingSamplesAsync();
    Task<IEnumerable<Sample>> FilterSamplesAsync(int? clienteGrd, DateTime? fechaInicio, DateTime? fechaFin);
}
