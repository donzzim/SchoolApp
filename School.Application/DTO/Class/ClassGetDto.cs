using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Class;

public class ClassGetDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
}
