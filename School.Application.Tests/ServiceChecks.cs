using School.Domain.Entities;
using School.Domain.Interfaces;

sealed class FakeCourseRepository : ICourseRepository
{
    public List<Course> Items { get; } = [];
    public Task<Course?> GetByIdAsync(int? id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Course>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Course> AddAsync(Course entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Course?> UpdateAsync(Course entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<Course?>(null); Items[index] = entity; return Task.FromResult<Course?>(entity); }
    public Task<Course?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeSchoolClassRepository : ISchoolClassRepository
{
    public List<SchoolClass> Items { get; } = [];
    public Task<SchoolClass?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<SchoolClass>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<SchoolClass> AddAsync(SchoolClass entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<SchoolClass?> UpdateAsync(SchoolClass entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<SchoolClass?>(null); Items[index] = entity; return Task.FromResult<SchoolClass?>(entity); }
    public Task<SchoolClass?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeUserRepository : IUserRepository
{
    public List<User> Items { get; } = [];
    public Task<bool> UserExistsAsync() => Task.FromResult(Items.Any(user => !user.Deleted));
    public Task<User?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<User>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<User> AddAsync(User entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<User?> UpdateAsync(User entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<User?>(null); Items[index] = entity; return Task.FromResult<User?>(entity); }
    public Task<User?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeEnrollmentRepository : IEnrollmentRepository
{
    public List<Enrollment> Items { get; } = [];
    public Task<Enrollment?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Enrollment>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Enrollment> AddAsync(Enrollment entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Enrollment> UpdateAsync(Enrollment entity) => Task.FromResult(entity);
    public Task<Enrollment?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}

sealed class FakeGradeRepository : IGradeRepository
{
    public List<Grade> Items { get; } = [];
    public Task<Grade?> GetByIdAsync(int id) => Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
    public Task<List<Grade>> GetAllAsync() => Task.FromResult(Items.ToList());
    public Task<Grade> AddAsync(Grade entity) { entity.Id = Items.Count + 1; Items.Add(entity); return Task.FromResult(entity); }
    public Task<Grade?> UpdateAsync(Grade entity) { var index = Items.FindIndex(x => x.Id == entity.Id); if (index < 0) return Task.FromResult<Grade?>(null); Items[index] = entity; return Task.FromResult<Grade?>(entity); }
    public Task<Grade?> DeleteAsync(int id) { var entity = Items.FirstOrDefault(x => x.Id == id); if (entity is not null) entity.Deleted = true; return Task.FromResult(entity); }
}
