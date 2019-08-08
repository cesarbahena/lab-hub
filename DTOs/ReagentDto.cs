namespace QuimiOSHub.DTOs;

public class ReagentDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int CalibrationConsumption { get; set; }
    public string? Category { get; set; }
    public string? Unit { get; set; }
    public bool IsActive { get; set; }
}
