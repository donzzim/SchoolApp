using Microsoft.AspNetCore.Mvc;
using School.Application.DTO.Enrollment;
using School.Application.Interfaces;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : Controller
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllEnrollments()
    {
        var enrollments = await _enrollmentService.GetAllAsync();
        return Ok(enrollments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEnrollmentById(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);
        if (enrollment == null) return NotFound();
        return Ok(enrollment);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEnrollment(int id)
    {
        var deletedEnrollment = await _enrollmentService.DeleteAsync(id);
        if (deletedEnrollment == null) return NotFound();
        return Ok(deletedEnrollment);
    }

    [HttpPost]
    public async Task<ActionResult> CreateEnrollment(EnrollmentPostDto enrollmentDto)
    {
        var createdEnrollment = await _enrollmentService.AddAsync(enrollmentDto);
        if (createdEnrollment == null) return BadRequest();
        return Ok(createdEnrollment);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateEnrollment(EnrollmentPutDto enrollmentDto)
    {
        var updatedEnrollment = await _enrollmentService.UpdateAsync(enrollmentDto);
        if (updatedEnrollment == null) return BadRequest();
        return Ok(updatedEnrollment);
    }
}
