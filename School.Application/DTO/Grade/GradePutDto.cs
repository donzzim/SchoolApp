using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Grade;

public class GradePutDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }
    [Range(1, int.MaxValue)]
    public int EnrollmentId { get; set; }
    [Range(0, int.MaxValue)]
    public int GradeValue { get; set; }
    public DateTime DateGrade { get; set; }
    public bool IsApproved { get; set; }
}
