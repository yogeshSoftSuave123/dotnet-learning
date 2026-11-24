using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Day5.MiniProject.Interfaces;
using Day5.MiniProject.Models;

namespace Day5.MiniProject.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<string, Employee> _employees;

        public EmployeeRepository()
        {
            _employees = new Dictionary<string, Employee>(StringComparer.OrdinalIgnoreCase);
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            await Task.Delay(50, cancellationToken);  // Simulate async operation

            if (_employees.ContainsKey(employee.EmployeeId))
                throw new InvalidOperationException($"Employee {employee.EmployeeId} already exists");

            _employees[employee.EmployeeId] = employee;
        }

        public async Task<Employee?> GetByIdAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            await Task.Delay(30, cancellationToken);

            _employees.TryGetValue(employeeId, out var employee);
            return employee;
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(50, cancellationToken);
            return _employees.Values.ToList();
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            await Task.Delay(50, cancellationToken);

            if (!_employees.ContainsKey(employee.EmployeeId))
                throw new KeyNotFoundException($"Employee {employee.EmployeeId} not found");

            _employees[employee.EmployeeId] = employee;
        }

        public async Task DeleteAsync(string employeeId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            await Task.Delay(50, cancellationToken);

            if (!_employees.Remove(employeeId))
                throw new KeyNotFoundException($"Employee {employeeId} not found");
        }

        public async Task<List<Employee>> FindByDepartmentAsync(string department, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            await Task.Delay(50, cancellationToken);

            return _employees.Values
                .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
