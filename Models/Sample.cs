using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiOSHub.Models;

[Table("samples")]
public class Sample
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("received_at")]
    public DateTime? ReceivedAt { get; set; }

    [Column("folio")]
    public int? Folio { get; set; }

    [Column("client_id")]
    public int? ClientId { get; set; }

    [Column("patient_id")]
    public int? PatientId { get; set; }

    [Column("exam_id")]
    public int? ExamId { get; set; }

    [Column("exam_name")]
    [MaxLength(255)]
    public string? ExamName { get; set; }

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("validated_at")]
    public DateTime? ValidatedAt { get; set; }

    [Column("location")]
    [MaxLength(255)]
    public string? Location { get; set; }

    [Column("outsourcer")]
    [MaxLength(255)]
    public string? Outsourcer { get; set; }

    [Column("priority")]
    [MaxLength(255)]
    public string? Priority { get; set; }

    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }

    [NotMapped]
    public bool IsCompleted => ValidatedAt.HasValue;

    [NotMapped]
    public bool IsPending => !ValidatedAt.HasValue;
}
