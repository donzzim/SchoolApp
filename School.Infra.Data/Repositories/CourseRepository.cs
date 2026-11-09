using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Course> GetByIdAsync(int id)
    {
        return await _context.Course.FindAsync(id);
    }

    public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Course.ToListAsync();
    }

    public async Task<Course> AddAsync(Course course)
    {
        _context.Course.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> UpdateAsync(Course course)
    {
        _context.Course.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> DeleteAsync(int id)
    {
        var course = await _context.Course.FindAsync(id);
        if (course == null)
        {
            return null;
        }

        course.Deleted = true;
        _context.Course.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }
}