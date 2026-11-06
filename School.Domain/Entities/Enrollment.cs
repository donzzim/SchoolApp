namespace School.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollmentDate  { get; set; }
    public DateTime ExpirationDate  { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Grade>? Grades { get; set; }
    public User? User { get; set; }
    public Class? Class { get; set; }
}