using Day07.DI.Middleware.Models;

namespace Day07.DI.Middleware.Services;

/// <summary>
/// In-memory implementation of IEmployeeRepository
/// Demonstrates Scoped lifetime
/// </summary>
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private static readonly List<Employee> _employees = new()
    {
        new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@company.com",
            Department = "Engineering",
            Salary = 95000,
            HireDate = DateTime.Parse("2020-01-15"),
            IsActive = true
        },
        new Employee
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@company.com",
            Department = "HR",
            Salary = 85000,
            HireDate = DateTime.Parse("2019-06-10"),
            IsActive = true
        }
    };

    private static int _nextId = 3;
    private readonly ILogger<InMemoryEmployeeRepository> _logger;
    private readonly Guid _instanceId = Guid.NewGuid();

    public InMemoryEmployeeRepository(ILogger<InMemoryEmployeeRepository> logger)
    {
        _logger = logger;
        _logger.LogInformation("Repository created with instance ID: {InstanceId}", _instanceId);
    }

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        _logger.LogInformation("Getting all employees (Instance: {InstanceId})", _instanceId);
        return Task.FromResult<IEnumerable<Employee>>(_employees.ToList());
    }

    public Task<Employee?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Getting employee {EmployeeId} (Instance: {InstanceId})", id, _instanceId);
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employee);
    }

    public Task<Employee> AddAsync(Employee employee)
    {
        employee.Id = _nextId++;
        _employees.Add(employee);
        _logger.LogInformation("Added employee {EmployeeId} (Instance: {InstanceId})", employee.Id, _instanceId);
        return Task.FromResult(employee);
    }

    public Task<bool> UpdateAsync(Employee employee)
    {
        var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
        if (existing == null)
        {
            _logger.LogWarning("Employee {EmployeeId} not found for update", employee.Id);
            return Task.FromResult(false);
        }

        var index = _employees.IndexOf(existing);
        _employees[index] = employee;
        _logger.LogInformation("Updated employee {EmployeeId} (Instance: {InstanceId})", employee.Id, _instanceId);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            _logger.LogWarning("Employee {EmployeeId} not found for deletion", id);
            return Task.FromResult(false);
        }

        _employees.Remove(employee);
        _logger.LogInformation("Deleted employee {EmployeeId} (Instance: {InstanceId})", id, _instanceId);
        return Task.FromResult(true);
    }
}
