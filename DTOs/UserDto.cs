using System.ComponentModel.DataAnnotations;

namespace QuimiosHub.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Role { get; set; }
    public bool IsActive { get; set; }
}

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Role { get; set; }
}

public class UpdateUserDto
{
    [MaxLength(200)]
    public string? FullName { get; set; }

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Role { get; set; }

    public bool? IsActive { get; set; }
}
