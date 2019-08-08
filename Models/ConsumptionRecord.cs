using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("consumption_records")]
public class ConsumptionRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reagent_id")]
    [Required]
    public int ReagentId { get; set; }

    [ForeignKey("ReagentId")]
    public Reagent Reagent { get; set; } = null!;

    [Column("consumption_date")]
    [Required]
    public DateTime ConsumptionDate { get; set; }

    [Column("research_consumption")]
    public decimal ResearchConsumption { get; set; }

    [Column("repeat_consumption")]
    public decimal RepeatConsumption { get; set; }

    [Column("qc_consumption")]
    public decimal QCConsumption { get; set; }

    [Column("manual_consumption")]
    public decimal ManualConsumption { get; set; }

    [Column("calibration_consumption")]
    public decimal CalibrationConsumption { get; set; }

    [Column("total_consumption")]
    public decimal TotalConsumption { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}
