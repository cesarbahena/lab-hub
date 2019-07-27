using Microsoft.AspNetCore.Mvc;
using QuimiOSHub.Data;

namespace QuimiOSHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public HealthController(QuimiosDbContext context)
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
