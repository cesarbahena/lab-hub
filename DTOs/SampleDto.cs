namespace QuimiOSHub.DTOs;

public class SampleDto
{
    public int Id { get; set; }
    public DateTime? FechaGrd { get; set; }
    public DateTime? FechaRecep { get; set; }
    public int? FolioGrd { get; set; }
    public int? ClienteGrd { get; set; }
    public int? PacienteGrd { get; set; }
    public string? Label1 { get; set; }
    public DateTime? FecCapRes { get; set; }
    public DateTime? FecLibera { get; set; }
    public string? SucProc { get; set; }
}

public class SampleFilterDto
{
    public int? ClienteGrd { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
