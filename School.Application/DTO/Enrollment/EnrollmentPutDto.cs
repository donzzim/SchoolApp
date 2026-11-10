using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Enrollment;

public class EnrollmentPutDto
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    [Range(1, int.MaxValue)]
    public int ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}
