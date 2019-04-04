using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiosHub.Data;
using QuimiosHub.Models;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionRoutesController : ControllerBase
{
    private readonly QuimiosDbContext _context;

    public CollectionRoutesController(QuimiosDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CollectionRoute>>> GetCollectionRoutes(
        [FromQuery] bool? isActive = null)
    {
        var query = _context.CollectionRoutes
            .Include(cr => cr.RouteStops.OrderBy(rs => rs.SequenceOrder))
            .AsQueryable();

        if (isActive.HasValue)
            query = query.Where(cr => cr.IsActive == isActive.Value);

        var routes = await query.OrderBy(cr => cr.Name).ToListAsync();

        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionRoute>> GetCollectionRoute(int id)
    {
        var route = await _context.CollectionRoutes
            .Include(cr => cr.RouteStops.OrderBy(rs => rs.SequenceOrder))
            .FirstOrDefaultAsync(cr => cr.Id == id);

        if (route == null)
            return NotFound();

        return Ok(route);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionRoute>> CreateCollectionRoute(CollectionRoute route)
    {
        _context.CollectionRoutes.Add(route);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCollectionRoute), new { id = route.Id }, route);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollectionRoute(int id, CollectionRoute route)
    {
        if (id != route.Id)
            return BadRequest();

        _context.Entry(route).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.CollectionRoutes.AnyAsync(cr => cr.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
}
