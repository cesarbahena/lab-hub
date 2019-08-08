using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReagentsController : ControllerBase
{
    private readonly QuimiosDbContext _context;
    private readonly ILogger<ReagentsController> _logger;

    public ReagentsController(QuimiosDbContext context, ILogger<ReagentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReagentDto>>> GetReagents()
    {
        var reagents = await _context.Reagents
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .Select(r => new ReagentDto
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                CalibrationConsumption = r.CalibrationConsumption,
                Category = r.Category,
                Unit = r.Unit,
                IsActive = r.IsActive
            })
            .ToListAsync();

        return Ok(reagents);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReagentDto>> GetReagent(int id)
    {
        var reagent = await _context.Reagents.FindAsync(id);

        if (reagent == null)
        {
            return NotFound();
        }

        var reagentDto = new ReagentDto
        {
            Id = reagent.Id,
            Code = reagent.Code,
            Name = reagent.Name,
            CalibrationConsumption = reagent.CalibrationConsumption,
            Category = reagent.Category,
            Unit = reagent.Unit,
            IsActive = reagent.IsActive
        };

        return Ok(reagentDto);
    }

    [HttpGet("by-code/{code}")]
    public async Task<ActionResult<ReagentDto>> GetReagentByCode(string code)
    {
        var reagent = await _context.Reagents
            .FirstOrDefaultAsync(r => r.Code == code && r.IsActive);

        if (reagent == null)
        {
            return NotFound();
        }

        var reagentDto = new ReagentDto
        {
            Id = reagent.Id,
            Code = reagent.Code,
            Name = reagent.Name,
            CalibrationConsumption = reagent.CalibrationConsumption,
            Category = reagent.Category,
            Unit = reagent.Unit,
            IsActive = reagent.IsActive
        };

        return Ok(reagentDto);
    }
}
