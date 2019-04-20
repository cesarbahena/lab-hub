using QuimiosHub.DTOs;
using QuimiosHub.Models;

namespace QuimiosHub.Services;

public interface ISampleService
{
    Task<IEnumerable<Sample>> GetAllSamplesAsync();
    Task<Sample?> GetSampleByIdAsync(int id);
    Task<IEnumerable<Sample>> GetPendingSamplesAsync();
    Task<IEnumerable<Sample>> FilterSamplesAsync(int? clienteGrd, DateTime? fechaInicio, DateTime? fechaFin);
    Task<(List<SampleDto> Samples, int Total, int TotalPages)> GetPaginatedSamplesAsync(
        int? clienteGrd, DateTime? startDate, DateTime? endDate, int page, int pageSize);
    Task<Dictionary<string, int>> GetSampleStatisticsAsync(DateTime? startDate, DateTime? endDate);
    Task<bool> IsSamplePendingAsync(int sampleId);
}
