using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Day5.MiniProject.Models;

namespace Day5.MiniProject.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Employee?> GetByIdAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);
        Task DeleteAsync(string employeeId, CancellationToken cancellationToken = default);
        Task<List<Employee>> FindByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    }
}
