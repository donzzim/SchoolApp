using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Class;

public class ClassPutDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string? Name { get; set; }
    [Required, MaxLength(150)]
    public string? Description { get; set; }
    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }
}
