using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _context;

    public EnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollment.FindAsync(id);
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollment.ToListAsync();
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment)
    {
        _context.Enrollment.Add(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollment.Update(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<Enrollment?> DeleteAsync(int id)
    {
        var entity = await _context.Enrollment.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Deleted = true;
        _context.Enrollment.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
