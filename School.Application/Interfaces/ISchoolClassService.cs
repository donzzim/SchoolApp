using School.Application.Exceptions;
using School.Application.DTO.SchoolClass;

namespace School.Application.Interfaces;

public interface ISchoolClassService
{
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<SchoolClassGetDetailDto?> GetByIdAsync(int id);
    Task<List<SchoolClassGetDto>> GetAllAsync();
    /// <exception cref="NotFoundException">O curso informado não foi encontrado.</exception>
    Task<SchoolClassGetDto> AddAsync(SchoolClassPostDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<SchoolClassGetDto?> UpdateAsync(SchoolClassPutDto dto);
    /// <exception cref="NotFoundException">O registro solicitado ou um vínculo necessário não foi encontrado.</exception>
    Task<SchoolClassGetDto?> DeleteAsync(int id);
}
