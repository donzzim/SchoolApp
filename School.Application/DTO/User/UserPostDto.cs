using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.User;

public class UserPostDto
{
    [Required(ErrorMessage = "Name is required"), MaxLength(250, ErrorMessage = "Name must be less than 250 characters")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Email is invalid"), MaxLength(200, ErrorMessage = "Email must be less than 200 characters")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required"),MinLength(8, ErrorMessage ="Password must be at least 8 characters" ), MaxLength(250, ErrorMessage = "Password must be less than 250 characters")]
    public string? Password { get; set; }
}
