using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Grade;

public class GradePutDto
{
    [Required(ErrorMessage = "The grade's identifier is required")]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required(ErrorMessage = "Enrollment is required")]
    [Range(1, int.MaxValue)]
    public int EnrollmentId { get; set; }
    [Required(ErrorMessage = "Grade Value is required")]
    [Range(0, 100, ErrorMessage = "Grade value must be between 0 and 100")]
    public int GradeValue { get; set; }
}
