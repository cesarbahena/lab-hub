using Microsoft.AspNetCore.Mvc;
using LIMSApi.Data;

namespace LIMSApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly LIMSDbContext _context;

    public HealthController(LIMSDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetHealth()
    {
        var health = await _context.GetHealthStatusAsync();

        if (!health.IsHealthy)
            return StatusCode(503, health);

        return Ok(health);
    }

    [HttpGet("ping")]
    public ActionResult Ping()
    {
        return Ok(new { status = "alive", timestamp = DateTime.UtcNow });
    }
}
