using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.User;

public class UserPostDto
{
    [Required, MaxLength(250)]
    public string? Name { get; set; }
    [Required, EmailAddress, MaxLength(200)]
    public string? Email { get; set; }
    [Required, MaxLength(50)]
    public string? Profile { get; set; }
    [Required, MinLength(8)]
    public string? Password { get; set; }
}
