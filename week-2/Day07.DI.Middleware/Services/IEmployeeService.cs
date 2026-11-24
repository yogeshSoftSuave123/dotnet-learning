using Day07.DI.Middleware.Models;

namespace Day07.DI.Middleware.Services;

/// <summary>
/// Business logic service for Employee operations
/// Registered as Scoped - new instance per request
/// </summary>
public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee> CreateEmployeeAsync(Employee employee);
    Task<bool> UpdateEmployeeAsync(int id, Employee employee);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department);
}
