using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Grade;

public class GradePostDto
{
    [Required(ErrorMessage = "Enrollment is required")]
    [Range(1, int.MaxValue)]
    public int EnrollmentId { get; set; }
    [Required(ErrorMessage = "Grade is required")]
    [Range(0, 100, ErrorMessage = "Grade value must be between 0 and 100")]
    public int GradeValue { get; set; }
}
