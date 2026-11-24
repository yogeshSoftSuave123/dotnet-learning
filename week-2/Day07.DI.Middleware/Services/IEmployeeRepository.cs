using Day07.DI.Middleware.Models;

namespace Day07.DI.Middleware.Services;

/// <summary>
/// Repository interface for Employee data access
/// Registered as Scoped - new instance per request
/// </summary>
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> AddAsync(Employee employee);
    Task<bool> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
}
