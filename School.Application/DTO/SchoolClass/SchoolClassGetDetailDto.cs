using School.Application.DTO.SchoolClass;
using School.Application.DTO.Course;
using School.Application.DTO.User;

namespace School.Application.DTO.SchoolClass;

public class SchoolClassGetDetailDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public CourseGetDto? Course { get; set; }
}