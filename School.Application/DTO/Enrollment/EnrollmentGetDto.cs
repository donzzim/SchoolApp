using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Enrollment;

public class EnrollmentGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SchoolClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}
