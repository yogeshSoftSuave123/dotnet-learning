using System;
using System.Collections.Generic;
using System.Linq;
using Day2.DomainModels.Entities;
using Day2.DomainModels.Interfaces;

namespace Day2.DomainModels.Repositories
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly Dictionary<string, Employee> _employees;

        public InMemoryEmployeeRepository()
        {
            _employees = new Dictionary<string, Employee>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.ContainsKey(employee.EmployeeId))
                throw new InvalidOperationException($"Employee with ID {employee.EmployeeId} already exists");

            _employees[employee.EmployeeId] = employee;
        }

        public void Remove(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            if (!_employees.Remove(employeeId))
                throw new KeyNotFoundException($"Employee with ID {employeeId} not found");
        }

        public Employee? GetById(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            _employees.TryGetValue(employeeId, out var employee);
            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employees.Values.ToList();
        }

        public IEnumerable<Employee> GetByDepartment(string departmentId)
        {
            if (string.IsNullOrWhiteSpace(departmentId))
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

            return _employees.Values
                .Where(e => e.Department != null &&
                           e.Department.DepartmentId.Equals(departmentId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Update(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (!_employees.ContainsKey(employee.EmployeeId))
                throw new KeyNotFoundException($"Employee with ID {employee.EmployeeId} not found");

            _employees[employee.EmployeeId] = employee;
        }
    }
}
