using Day08.LayeredAPI.Application.DTOs;

namespace Day08.LayeredAPI.Application.Interfaces;

/// <summary>
/// Service interface - defines business logic contract
/// Services orchestrate business operations and coordinate between repositories
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeCreateDto createDto);
    Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto updateDto);
    Task DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(int departmentId);
}
