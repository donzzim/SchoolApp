namespace School.Domain.Entities;

public class SchoolClass
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public ICollection<Enrollment> ? Enrollments { get; set; }
    public Course? Course { get; set; }
}