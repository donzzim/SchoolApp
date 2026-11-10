using Microsoft.EntityFrameworkCore;
using School.Domain.Entities;
using School.Domain.Interfaces;
using School.Infra.Data.Context;

namespace School.Infra.Data.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly ApplicationDbContext _context;

    public GradeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Grade?> GetByIdAsync(int id)
    {
        return await _context.Grade.FindAsync(id);
    }

    public async Task<List<Grade>> GetAllAsync()
    {
        return await _context.Grade.ToListAsync();
    }

    public async Task<Grade> AddAsync(Grade grade)
    {
        _context.Grade.Add(grade);
        await _context.SaveChangesAsync();
        return grade;
    }

    public async Task<Grade> UpdateAsync(Grade grade)
    {
        _context.Grade.Update(grade);
        await _context.SaveChangesAsync();
        return grade;
    }

    public async Task<Grade?> DeleteAsync(int id)
    {
        var entity = await _context.Grade.FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        entity.Deleted = true;
        _context.Grade.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
