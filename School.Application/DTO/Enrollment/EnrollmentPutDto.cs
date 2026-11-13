using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Enrollment;

public class EnrollmentPutDto
{
    [Required(ErrorMessage = "Enrollment is required")]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required(ErrorMessage = "SchoolClass is required")]
    [Range(1, int.MaxValue)]
    public int SchoolClassId { get; set; }
    public DateTime ExpirationDate { get; set; }
}
