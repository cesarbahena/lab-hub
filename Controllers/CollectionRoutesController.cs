using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.DTOs;
using QuimiOSHub.Models;

namespace QuimiOSHub.Controllers;

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
    public async Task<ActionResult<IEnumerable<CollectionRouteDto>>> GetCollectionRoutes(
        [FromQuery] bool? isActive = null)
    {
        var query = _context.CollectionRoutes
            .Include(cr => cr.RouteStops)
            .AsQueryable();

        if (isActive.HasValue)
            query = query.Where(cr => cr.IsActive == isActive.Value);

        var routes = await query
            .OrderBy(cr => cr.Name)
            .Select(cr => new CollectionRouteDto
            {
                Id = cr.Id,
                Name = cr.Name,
                Description = cr.Description,
                IsActive = cr.IsActive,
                TotalStops = cr.RouteStops.Count,
                ActiveStops = cr.RouteStops.Count(rs => rs.IsActive),
                RouteStops = cr.RouteStops
                    .OrderBy(rs => rs.SequenceOrder)
                    .Select(rs => new RouteStopDto
                    {
                        Id = rs.Id,
                        SequenceOrder = rs.SequenceOrder,
                        LocationName = rs.LocationName,
                        Address = rs.Address,
                        Latitude = rs.Latitude,
                        Longitude = rs.Longitude,
                        ContactName = rs.ContactName,
                        ContactPhone = rs.ContactPhone,
                        IsActive = rs.IsActive
                    }).ToList()
            })
            .ToListAsync();

        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionRouteDto>> GetCollectionRoute(int id)
    {
        var route = await _context.CollectionRoutes
            .Include(cr => cr.RouteStops)
            .Where(cr => cr.Id == id)
            .Select(cr => new CollectionRouteDto
            {
                Id = cr.Id,
                Name = cr.Name,
                Description = cr.Description,
                IsActive = cr.IsActive,
                TotalStops = cr.RouteStops.Count,
                ActiveStops = cr.RouteStops.Count(rs => rs.IsActive),
                RouteStops = cr.RouteStops
                    .OrderBy(rs => rs.SequenceOrder)
                    .Select(rs => new RouteStopDto
                    {
                        Id = rs.Id,
                        SequenceOrder = rs.SequenceOrder,
                        LocationName = rs.LocationName,
                        Address = rs.Address,
                        Latitude = rs.Latitude,
                        Longitude = rs.Longitude,
                        ContactName = rs.ContactName,
                        ContactPhone = rs.ContactPhone,
                        IsActive = rs.IsActive
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (route == null)
            return NotFound(new { message = "Collection route not found" });

        return Ok(route);
    }

    [HttpPost]
    public async Task<ActionResult<CollectionRouteDto>> CreateCollectionRoute(CreateCollectionRouteDto dto)
    {
        var route = new CollectionRoute
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.CollectionRoutes.Add(route);
        await _context.SaveChangesAsync();

        foreach (var stopDto in dto.RouteStops)
        {
            var stop = new RouteStop
            {
                CollectionRouteId = route.Id,
                SequenceOrder = stopDto.SequenceOrder,
                LocationName = stopDto.LocationName,
                Address = stopDto.Address,
                Latitude = stopDto.Latitude,
                Longitude = stopDto.Longitude,
                ContactName = stopDto.ContactName,
                ContactPhone = stopDto.ContactPhone,
                Notes = stopDto.Notes,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.RouteStops.Add(stop);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCollectionRoute), new { id = route.Id },
            await GetCollectionRoute(route.Id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollectionRoute(int id, UpdateCollectionRouteDto dto)
    {
        var route = await _context.CollectionRoutes.FindAsync(id);
        if (route == null)
            return NotFound(new { message = "Collection route not found" });

        if (dto.Name != null)
            route.Name = dto.Name;

        if (dto.Description != null)
            route.Description = dto.Description;

        if (dto.IsActive.HasValue)
            route.IsActive = dto.IsActive.Value;

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
