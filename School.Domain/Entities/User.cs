namespace School.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; } // Validate/Verify the password hash
    public string? Profile { get; set; }
    public ICollection<Enrollment>? Enrollments { get; set; }
}