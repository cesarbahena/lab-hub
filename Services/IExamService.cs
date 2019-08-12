using LIMSApi.DTOs;
using LIMSApi.Models;

namespace LIMSApi.Services;

public interface IExamService
{
    Task<IEnumerable<Exam>> GetAllExamsAsync();
    Task<Exam?> GetExamByIdAsync(int id);
    Task<IEnumerable<Exam>> GetPendingExamsAsync();
    Task<IEnumerable<Exam>> FilterExamsAsync(int? clienteGrd, DateTime? fechaInicio, DateTime? fechaFin);
    Task<(List<ExamDto> Exams, int Total, int TotalPages)> GetPaginatedExamsAsync(
        int? clienteGrd, DateTime? startDate, DateTime? endDate, int page, int pageSize);
    Task<Dictionary<string, int>> GetExamStatisticsAsync(DateTime? startDate, DateTime? endDate);
    Task<bool> IsExamPendingAsync(int examId);
}