using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Course;

public class CoursePostDto
{
    [Required, MaxLength(50)]
    public string? Name { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }
}
