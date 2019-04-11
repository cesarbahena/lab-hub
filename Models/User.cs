using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuimiosHub.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Column("full_name")]
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Column("email")]
    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [Column("role")]
    [MaxLength(50)]
    public string? Role { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ShiftHandover> ShiftHandovers { get; set; } = new List<ShiftHandover>();
    public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    [NotMapped]
    public string DisplayName => $"{FullName} ({Username})";
}
