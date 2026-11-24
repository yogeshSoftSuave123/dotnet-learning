using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Application.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Department department);
    Task<bool> ExistsAsync(int id);
}
