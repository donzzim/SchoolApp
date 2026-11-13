using System.ComponentModel.DataAnnotations;

namespace School.Application.DTO.Enrollment;

public class EnrollmentPostDto
{
    [Required(ErrorMessage = "User is required")]
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
    [Required(ErrorMessage = "SchoolClass is required")]
    [Range(1, int.MaxValue)]
    public int SchoolClassId { get; set; }
    [Required(ErrorMessage = "Expiration Date is required")]
    public DateTime ExpirationDate { get; set; }
}
