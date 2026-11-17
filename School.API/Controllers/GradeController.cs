using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.Grade;
using School.Application.Interfaces;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController : Controller
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllGrades()
    {
        var grades = await _gradeService.GetAllAsync();
        return Ok(grades);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetGradeById(int id)
    {
        var grade = await _gradeService.GetByIdAsync(id);
        if (grade == null) return NotFound();
        return Ok(grade);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGrade(int id)
    {
        var deletedGrade = await _gradeService.DeleteAsync(id);
        if (deletedGrade == null) return NotFound();
        return Ok(deletedGrade);
    }

    [HttpPost]
    public async Task<ActionResult> CreateGrade(GradePostDto gradeDto)
    {
        var createdGrade = await _gradeService.AddAsync(gradeDto);
        if (createdGrade == null) return BadRequest();
        return Ok(createdGrade);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateGrade(GradePutDto gradeDto)
    {
        var updatedGrade = await _gradeService.UpdateAsync(gradeDto);
        if (updatedGrade == null) return BadRequest();
        return Ok(updatedGrade);
    }
}
