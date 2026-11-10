using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Grade;

public class GradeGetDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int EnrollmentId { get; set; }
    public int GradeValue { get; set; }
    public DateTime DateGrade { get; set; }
    public bool IsApproved { get; set; }
}
