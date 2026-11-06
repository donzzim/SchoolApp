namespace School.Domain.Entities;

public class Class
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public ICollection<Enrollment> ? Enrollments { get; set; }
    public Course? Course { get; set; }
}