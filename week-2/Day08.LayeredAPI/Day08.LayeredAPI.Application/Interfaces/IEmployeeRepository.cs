using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Application.Interfaces;

/// <summary>
/// Repository interface - defines data access contract
/// Application layer defines the interface, Infrastructure implements it
/// This follows Dependency Inversion Principle (SOLID)
/// </summary>
public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<bool> ExistsAsync(int id);
}
