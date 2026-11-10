using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Course;

public class CourseGetDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
