using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Course;

public class CoursePutDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string? Name { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }
}
