using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsumptionsController : ControllerBase
{
    private readonly QuimiosDbContext _context;
    private readonly ILogger<ConsumptionsController> _logger;

    public ConsumptionsController(QuimiosDbContext context, ILogger<ConsumptionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> SubmitConsumption([FromBody] CreateConsumptionBatchDto batchDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var validationErrors = new List<string>();
            var createdRecords = new List<ConsumptionRecord>();

            foreach (var dto in batchDto.Consumptions)
            {
                var reagent = await _context.Reagents.FindAsync(dto.ReagentId);
                if (reagent == null)
                {
                    validationErrors.Add($"Reagent with ID {dto.ReagentId} not found");
                    continue;
                }

                var inventoryItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(i => i.ReagentId == dto.ReagentId && i.IsActive);

                if (inventoryItem == null)
                {
                    validationErrors.Add($"No inventory item found for reagent {reagent.Name}");
                    continue;
                }

                var totalConsumption = dto.ResearchConsumption + dto.RepeatConsumption +
                                     dto.QCConsumption + dto.ManualConsumption + dto.CalibrationConsumption;

                if (inventoryItem.CurrentStock < totalConsumption)
                {
                    validationErrors.Add($"Insufficient stock for {reagent.Name}. Available: {inventoryItem.CurrentStock}, Required: {totalConsumption}");
                    continue;
                }

                var consumptionRecord = new ConsumptionRecord
                {
                    ReagentId = dto.ReagentId,
                    ConsumptionDate = dto.ConsumptionDate,
                    ResearchConsumption = dto.ResearchConsumption,
                    RepeatConsumption = dto.RepeatConsumption,
                    QCConsumption = dto.QCConsumption,
                    ManualConsumption = dto.ManualConsumption,
                    CalibrationConsumption = dto.CalibrationConsumption,
                    TotalConsumption = totalConsumption,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ConsumptionRecords.Add(consumptionRecord);
                await _context.SaveChangesAsync();

                var movement = new InventoryMovement
                {
                    InventoryItemId = inventoryItem.Id,
                    MovementType = "OUT",
                    Quantity = totalConsumption,
                    Reference = $"Consumption {dto.ConsumptionDate:yyyy-MM-dd}",
                    Notes = $"res:{dto.ResearchConsumption} rep:{dto.RepeatConsumption} qc:{dto.QCConsumption} man:{dto.ManualConsumption} cal:{dto.CalibrationConsumption}",
                    MovementDate = dto.ConsumptionDate,
                    ConsumptionRecordId = consumptionRecord.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.InventoryMovements.Add(movement);
                inventoryItem.CurrentStock -= totalConsumption;

                createdRecords.Add(consumptionRecord);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = new
            {
                Success = validationErrors.Count == 0,
                ProcessedCount = createdRecords.Count,
                TotalSubmitted = batchDto.Consumptions.Count,
                ValidationErrors = validationErrors,
                CreatedRecords = createdRecords.Select(r => new
                {
                    r.Id,
                    r.ReagentId,
                    r.ConsumptionDate,
                    r.TotalConsumption
                })
            };

            return validationErrors.Count > 0 ? BadRequest(response) : Ok(response);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error submitting consumption batch");
            return StatusCode(500, new { Error = "Failed to process consumption batch" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsumptionRecordDto>>> GetConsumptions(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var query = _context.ConsumptionRecords
            .Include(c => c.Reagent)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(c => c.ConsumptionDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.ConsumptionDate <= endDate.Value);
        }

        var consumptions = await query
            .OrderByDescending(c => c.ConsumptionDate)
            .Select(c => new ConsumptionRecordDto
            {
                Id = c.Id,
                ReagentId = c.ReagentId,
                ReagentCode = c.Reagent.Code,
                ReagentName = c.Reagent.Name,
                ConsumptionDate = c.ConsumptionDate,
                ResearchConsumption = c.ResearchConsumption,
                RepeatConsumption = c.RepeatConsumption,
                QCConsumption = c.QCConsumption,
                ManualConsumption = c.ManualConsumption,
                CalibrationConsumption = c.CalibrationConsumption,
                TotalConsumption = c.TotalConsumption,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(consumptions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConsumptionRecordDto>> GetConsumption(int id)
    {
        var consumption = await _context.ConsumptionRecords
            .Include(c => c.Reagent)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (consumption == null)
        {
            return NotFound();
        }

        var dto = new ConsumptionRecordDto
        {
            Id = consumption.Id,
            ReagentId = consumption.ReagentId,
            ReagentCode = consumption.Reagent.Code,
            ReagentName = consumption.Reagent.Name,
            ConsumptionDate = consumption.ConsumptionDate,
            ResearchConsumption = consumption.ResearchConsumption,
            RepeatConsumption = consumption.RepeatConsumption,
            QCConsumption = consumption.QCConsumption,
            ManualConsumption = consumption.ManualConsumption,
            CalibrationConsumption = consumption.CalibrationConsumption,
            TotalConsumption = consumption.TotalConsumption,
            CreatedAt = consumption.CreatedAt
        };

        return Ok(dto);
    }
}
