using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("samples")]
public class Sample
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fecha_grd")]
    public DateTime? FechaGrd { get; set; }

    [Column("fecha_recep")]
    public DateTime? FechaRecep { get; set; }

    [Column("folio_grd")]
    public int? FolioGrd { get; set; }

    [Column("cliente_grd")]
    public int? ClienteGrd { get; set; }

    [Column("paciente_grd")]
    public int? PacienteGrd { get; set; }

    [Column("est_per_grd")]
    public int? EstPerGrd { get; set; }

    [Column("label1")]
    [MaxLength(255)]
    public string? Label1 { get; set; }

    [Column("fec_cap_res")]
    public DateTime? FecCapRes { get; set; }

    [Column("fec_libera")]
    public DateTime? FecLibera { get; set; }

    [Column("suc_proc")]
    [MaxLength(255)]
    public string? SucProc { get; set; }

    [Column("maquilador")]
    [MaxLength(255)]
    public string? Maquilador { get; set; }

    [Column("label3")]
    [MaxLength(255)]
    public string? Label3 { get; set; }

    [Column("fec_nac")]
    public DateTime? FecNac { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public bool IsCompleted => FecLibera.HasValue;

    [NotMapped]
    public bool IsPending => !FecLibera.HasValue;
}
