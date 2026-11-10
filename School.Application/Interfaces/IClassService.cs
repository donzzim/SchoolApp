using School.Application.DTO.Class;

namespace School.Application.Interfaces;

public interface IClassService
{
    Task<ClassGetDto?> GetByIdAsync(int id);
    Task<List<ClassGetDto>> GetAllAsync();
    Task<ClassGetDto> AddAsync(ClassPostDto dto);
    Task<ClassGetDto?> UpdateAsync(ClassPutDto dto);
    Task<ClassGetDto?> DeleteAsync(int id);
}
