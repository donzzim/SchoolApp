namespace School.Domain.Entities;

public class Grade
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int EnrollmentId { get; set; }
    public int GradeValue { get; set; }
    public Enrollment? Enrollment { get; set; }
    public bool IsApproved { get; set; }
    public Course? Course { get; set; }
}