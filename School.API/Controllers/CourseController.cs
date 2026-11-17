using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.Course;
using School.Application.Interfaces;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : Controller
{
    private readonly ICourseService _courseService;
    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCourseById(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null) return NotFound();
        return Ok(course);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(int id)
    {
        var deletedCourse = await _courseService.DeleteAsync(id);
        if (deletedCourse == null) return NotFound();
        return Ok(deletedCourse);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCourse(CoursePostDto courseDto)
    {
        var createdCourse = await _courseService.AddAsync(courseDto);
        if (createdCourse == null) return BadRequest();
        return Ok(createdCourse);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateCourse(CoursePutDto courseDto)
    {
        var updatedCourse = await _courseService.UpdateAsync(courseDto);
        if (updatedCourse == null) return BadRequest();
        return Ok(updatedCourse);
    }
}
