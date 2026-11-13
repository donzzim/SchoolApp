using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Grade;

public class GradeGetDto
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public int GradeValue { get; set; }
    public bool IsApproved { get; set; }
    public DateTime DateGrade { get; set; }
}
