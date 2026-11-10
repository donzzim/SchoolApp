using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.User;

public class UserPutDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required, MaxLength(250)]
    public string? Name { get; set; }
    [Required, EmailAddress, MaxLength(200)]
    public string? Email { get; set; }
    [Required, MaxLength(50)]
    public string? Profile { get; set; }
    [MinLength(8)]
    public string? Password { get; set; }
}
