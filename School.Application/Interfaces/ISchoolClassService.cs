using School.Application.DTO.SchoolClass;

namespace School.Application.Interfaces;

public interface ISchoolClassService
{
    Task<SchoolClassGetDetailDto?> GetByIdAsync(int id);
    Task<List<SchoolClassGetDto>> GetAllAsync();
    Task<SchoolClassGetDto> AddAsync(SchoolClassPostDto dto);
    Task<SchoolClassGetDto?> UpdateAsync(SchoolClassPutDto dto);
    Task<SchoolClassGetDto?> DeleteAsync(int id);
}
