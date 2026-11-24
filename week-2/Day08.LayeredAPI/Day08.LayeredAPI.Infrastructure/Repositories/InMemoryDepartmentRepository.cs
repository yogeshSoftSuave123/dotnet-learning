using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Infrastructure.Repositories;

public class InMemoryDepartmentRepository : IDepartmentRepository
{
    private static readonly List<Department> _departments = new();
    private static int _nextId = 1;
    private static bool _seeded = false;

    public InMemoryDepartmentRepository()
    {
        if (!_seeded)
        {
            SeedData();
            _seeded = true;
        }
    }

    public Task<IEnumerable<Department>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Department>>(_departments.ToList());
    }

    public Task<Department?> GetByIdAsync(int id)
    {
        var department = _departments.FirstOrDefault(d => d.Id == id);
        return Task.FromResult(department);
    }

    public Task<Department> AddAsync(Department department)
    {
        department.Id = _nextId++;
        _departments.Add(department);
        return Task.FromResult(department);
    }

    public Task UpdateAsync(Department department)
    {
        var index = _departments.FindIndex(d => d.Id == department.Id);
        if (index >= 0)
        {
            _departments[index] = department;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Department department)
    {
        _departments.Remove(department);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_departments.Any(d => d.Id == id));
    }

    private static void SeedData()
    {
        _departments.AddRange(new[]
        {
            new Department
            {
                Id = _nextId++,
                Name = "Engineering",
                Description = "Software Development and Engineering",
                Location = "Building A",
                CreatedAt = DateTime.UtcNow.AddMonths(-12)
            },
            new Department
            {
                Id = _nextId++,
                Name = "Human Resources",
                Description = "HR and People Operations",
                Location = "Building B",
                CreatedAt = DateTime.UtcNow.AddMonths(-18)
            },
            new Department
            {
                Id = _nextId++,
                Name = "Marketing",
                Description = "Marketing and Communications",
                Location = "Building C",
                CreatedAt = DateTime.UtcNow.AddMonths(-10)
            }
        });
    }
}
