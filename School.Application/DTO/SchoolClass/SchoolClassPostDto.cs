using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.SchoolClass;

public class SchoolClassPostDto
{
    [Required(ErrorMessage = "Name is required"), MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Description is required"), MaxLength(150, ErrorMessage = "Description must be less than 150 characters")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }
}
