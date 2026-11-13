namespace School.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Deleted { get; set; }
    public ICollection<SchoolClass>? SchoolClasses { get; set; }
}