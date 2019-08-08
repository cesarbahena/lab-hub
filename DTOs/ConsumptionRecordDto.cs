namespace QuimiOSHub.DTOs;

public class ConsumptionRecordDto
{
    public int Id { get; set; }
    public int ReagentId { get; set; }
    public string ReagentCode { get; set; } = string.Empty;
    public string ReagentName { get; set; } = string.Empty;
    public DateTime ConsumptionDate { get; set; }
    public decimal ResearchConsumption { get; set; }
    public decimal RepeatConsumption { get; set; }
    public decimal QCConsumption { get; set; }
    public decimal ManualConsumption { get; set; }
    public decimal CalibrationConsumption { get; set; }
    public decimal TotalConsumption { get; set; }
    public DateTime CreatedAt { get; set; }
}
