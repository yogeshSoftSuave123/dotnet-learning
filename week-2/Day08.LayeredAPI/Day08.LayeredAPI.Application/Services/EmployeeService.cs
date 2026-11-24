using Day08.LayeredAPI.Application.DTOs;
using Day08.LayeredAPI.Application.Exceptions;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Day08.LayeredAPI.Application.Services;

/// <summary>
/// Employee service implementation
/// Contains business logic and validation
/// Maps between entities and DTOs
/// </summary>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        _logger.LogInformation("Getting all employees");
        var employees = await _employeeRepository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        _logger.LogInformation("Getting employee {EmployeeId}", id);
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            _logger.LogWarning("Employee {EmployeeId} not found", id);
            throw new NotFoundException(nameof(Employee), id);
        }

        return MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto createDto)
    {
        _logger.LogInformation("Creating employee with email {Email}", createDto.Email);

        // Business validation - check for duplicate email
        var existingEmployee = await _employeeRepository.GetByEmailAsync(createDto.Email);
        if (existingEmployee != null)
        {
            throw new ValidationException(nameof(createDto.Email),
                "An employee with this email already exists");
        }

        // Validate department exists
        var departmentExists = await _departmentRepository.ExistsAsync(createDto.DepartmentId);
        if (!departmentExists)
        {
            throw new ValidationException(nameof(createDto.DepartmentId),
                $"Department with ID {createDto.DepartmentId} does not exist");
        }

        // Map DTO to Entity
        var employee = new Employee
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PhoneNumber = createDto.PhoneNumber,
            DepartmentId = createDto.DepartmentId,
            Position = createDto.Position,
            Salary = createDto.Salary,
            HireDate = createDto.HireDate ?? DateTime.UtcNow,
            IsActive = true
        };

        var created = await _employeeRepository.AddAsync(employee);
        _logger.LogInformation("Created employee {EmployeeId}", created.Id);

        return MapToDto(created);
    }

    public async Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto updateDto)
    {
        _logger.LogInformation("Updating employee {EmployeeId}", id);

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        // Check for duplicate email (excluding current employee)
        var employeeWithEmail = await _employeeRepository.GetByEmailAsync(updateDto.Email);
        if (employeeWithEmail != null && employeeWithEmail.Id != id)
        {
            throw new ValidationException(nameof(updateDto.Email),
                "An employee with this email already exists");
        }

        // Validate department exists
        var departmentExists = await _departmentRepository.ExistsAsync(updateDto.DepartmentId);
        if (!departmentExists)
        {
            throw new ValidationException(nameof(updateDto.DepartmentId),
                $"Department with ID {updateDto.DepartmentId} does not exist");
        }

        // Update entity
        employee.FirstName = updateDto.FirstName;
        employee.LastName = updateDto.LastName;
        employee.Email = updateDto.Email;
        employee.PhoneNumber = updateDto.PhoneNumber;
        employee.DepartmentId = updateDto.DepartmentId;
        employee.Position = updateDto.Position;
        employee.Salary = updateDto.Salary;
        employee.IsActive = updateDto.IsActive;

        await _employeeRepository.UpdateAsync(employee);
        _logger.LogInformation("Updated employee {EmployeeId}", id);

        return MapToDto(employee);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        _logger.LogInformation("Deleting employee {EmployeeId}", id);

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), id);
        }

        await _employeeRepository.DeleteAsync(employee);
        _logger.LogInformation("Deleted employee {EmployeeId}", id);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId)
    {
        _logger.LogInformation("Getting employees for department {DepartmentId}", departmentId);

        // Validate department exists
        var departmentExists = await _departmentRepository.ExistsAsync(departmentId);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), departmentId);
        }

        var employees = await _employeeRepository.GetAllAsync();
        return employees
            .Where(e => e.DepartmentId == departmentId)
            .Select(MapToDto);
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = employee.GetFullName(),
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.Name ?? "Unknown",
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        };
    }
}
