using School.Application.DTO.SchoolClass;
using School.Application.DTO.User;

namespace School.Application.DTO.Enrollment;

public class EnrollmentGetDetailDto
{
    public int Id { get; set; }
    public UserGetDto User { get; set; }
    public SchoolClassGetDto SchoolClass { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime EnrollmentExpiration { get; set; }
    public bool IsActive { get; set; }
}