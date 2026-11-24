using Day07.DI.Middleware.Models;

namespace Day07.DI.Middleware.Services;

/// <summary>
/// Scoped service layer demonstrating dependency injection
/// Shows how scoped service depends on other services
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IGuidGeneratorService _guidGenerator;
    private readonly Guid _instanceId;

    public EmployeeService(
        IEmployeeRepository repository,
        ILogger<EmployeeService> logger,
        IGuidGeneratorService guidGenerator)
    {
        _repository = repository;
        _logger = logger;
        _guidGenerator = guidGenerator;
        _instanceId = _guidGenerator.GenerateGuid();
        _logger.LogInformation("EmployeeService created as SCOPED with instance ID: {InstanceId}", _instanceId);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Service {InstanceId}: Getting all employees", _instanceId);
        return await _repository.GetAllAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        _logger.LogInformation("Service {InstanceId}: Getting employee {EmployeeId}", _instanceId, id);
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Employee> CreateEmployeeAsync(Employee employee)
    {
        // Business logic validation
        if (string.IsNullOrWhiteSpace(employee.Email))
        {
            _logger.LogWarning("Attempted to create employee without email");
            throw new ArgumentException("Email is required", nameof(employee));
        }

        _logger.LogInformation("Service {InstanceId}: Creating new employee {Email}", _instanceId, employee.Email);
        return await _repository.AddAsync(employee);
    }

    public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Service {InstanceId}: Employee {EmployeeId} not found for update", _instanceId, id);
            return false;
        }

        employee.Id = id;
        _logger.LogInformation("Service {InstanceId}: Updating employee {EmployeeId}", _instanceId, id);
        return await _repository.UpdateAsync(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        _logger.LogInformation("Service {InstanceId}: Deleting employee {EmployeeId}", _instanceId, id);
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department)
    {
        _logger.LogInformation("Service {InstanceId}: Getting employees in department {Department}", _instanceId, department);
        var allEmployees = await _repository.GetAllAsync();
        return allEmployees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
    }
}
