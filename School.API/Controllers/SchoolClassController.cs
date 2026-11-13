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
        if (createdSchoolClass == null) return BadRequest("Something went wrong on creating School Class");
        return Ok(createdSchoolClass);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateSchoolClass(SchoolClassPutDto schoolClassPutDto)
    {
        var updatedSchoolClass = await _schoolClassService.UpdateAsync(schoolClassPutDto);
        if(updatedSchoolClass == null) return BadRequest("Something went wrong on updating School Class");
        return Ok(updatedSchoolClass);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSchoolClass(int id)
    {
        var deletedSchoolClass = await _schoolClassService.DeleteAsync(id);
        if(deletedSchoolClass == null) return BadRequest("Something went wrong on deleting School Class");
        return Ok(deletedSchoolClass);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetSchoolClassById(int id)
    {
        var schoolClass = await _schoolClassService.GetByIdAsync(id);
        if (schoolClass == null) return BadRequest("Something went wrong on find the School Class");
        return Ok(schoolClass);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllSchoolClasses(int id)
    {
        var schoolClasses = await _schoolClassService.GetAllAsync();
        return Ok(schoolClasses);
    }
}