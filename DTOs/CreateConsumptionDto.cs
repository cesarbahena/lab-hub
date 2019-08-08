namespace QuimiOSHub.DTOs;

public class CreateConsumptionDto
{
    public int ReagentId { get; set; }
    public DateTime ConsumptionDate { get; set; }
    public decimal ResearchConsumption { get; set; }
    public decimal RepeatConsumption { get; set; }
    public decimal QCConsumption { get; set; }
    public decimal ManualConsumption { get; set; }
    public decimal CalibrationConsumption { get; set; }
}

public class CreateConsumptionBatchDto
{
    public List<CreateConsumptionDto> Consumptions { get; set; } = new();
}
