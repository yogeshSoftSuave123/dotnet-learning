using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IEmployeeRepository
/// In Day 09, this will be replaced with EF Core implementation
/// </summary>
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private static readonly List<Employee> _employees = new();
    private static int _nextId = 1;
    private static bool _seeded = false;

    public InMemoryEmployeeRepository()
    {
        if (!_seeded)
        {
            SeedData();
            _seeded = true;
        }
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Employee>>(_employees.ToList());
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employee);
    }

    public Task<Employee?> GetByEmailAsync(string email)
    {
        var employee = _employees.FirstOrDefault(e =>
            e.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(employee);
    }

    public Task<Employee> AddAsync(Employee employee)
    {
        employee.Id = _nextId++;
        _employees.Add(employee);
        return Task.FromResult(employee);
    }

    public Task UpdateAsync(Employee employee)
    {
        var index = _employees.FindIndex(e => e.Id == employee.Id);
        if (index >= 0)
        {
            _employees[index] = employee;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee employee)
    {
        _employees.Remove(employee);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_employees.Any(e => e.Id == id));
    }

    private static void SeedData()
    {
        _employees.AddRange(new[]
        {
            new Employee
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                PhoneNumber = "+1234567890",
                DepartmentId = 1,
                Position = "Senior Developer",
                Salary = 95000,
                HireDate = DateTime.Parse("2020-01-15"),
                IsActive = true
            },
            new Employee
            {
                Id = _nextId++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@company.com",
                PhoneNumber = "+1234567891",
                DepartmentId = 2,
                Position = "HR Manager",
                Salary = 85000,
                HireDate = DateTime.Parse("2019-06-10"),
                IsActive = true
            }
        });
    }
}
