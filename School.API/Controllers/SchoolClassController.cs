using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.SchoolClass;
using School.Application.Interfaces;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolClassController : Controller
{
    private readonly ISchoolClassService _schoolClassService;

    public SchoolClassController(ISchoolClassService schoolClassService)
    {
        _schoolClassService = schoolClassService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateSchoolClass(SchoolClassPostDto schoolClassPostDto)
    {
        var createdSchoolClass = await _schoolClassService.AddAsync(schoolClassPostDto);
        return Ok(createdSchoolClass);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateSchoolClass(SchoolClassPutDto schoolClassPutDto)
    {
        var updatedSchoolClass = await _schoolClassService.UpdateAsync(schoolClassPutDto);
        return Ok(updatedSchoolClass);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSchoolClass(int id)
    {
        var deletedSchoolClass = await _schoolClassService.DeleteAsync(id);
        return Ok(deletedSchoolClass);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSchoolClassById(int id)
    {
        var schoolClass = await _schoolClassService.GetByIdAsync(id);
        return Ok(schoolClass);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllSchoolClasses()
    {
        var schoolClasses = await _schoolClassService.GetAllAsync();
        return Ok(schoolClasses);
    }
}