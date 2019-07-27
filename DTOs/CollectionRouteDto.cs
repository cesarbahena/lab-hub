using System.ComponentModel.DataAnnotations;

namespace QuimiOSHub.DTOs;

public class CollectionRouteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int TotalStops { get; set; }
    public int ActiveStops { get; set; }
    public List<RouteStopDto> RouteStops { get; set; } = new();
}

public class RouteStopDto
{
    public int Id { get; set; }
    public int SequenceOrder { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public bool IsActive { get; set; }
}

public class CreateCollectionRouteDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public List<CreateRouteStopDto> RouteStops { get; set; } = new();
}

public class CreateRouteStopDto
{
    [Required]
    public int SequenceOrder { get; set; }

    [Required]
    [MaxLength(200)]
    public string LocationName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(200)]
    public string? ContactName { get; set; }

    [MaxLength(50)]
    public string? ContactPhone { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class UpdateCollectionRouteDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}
