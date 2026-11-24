using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2.DomainModels.Entities
{
    public class Department
    {
        public string DepartmentId { get; private set; }
        public string Name { get; set; }
        public string Location { get; set; }
        private List<Employee> _employees;

        public IReadOnlyList<Employee> Employees => _employees.AsReadOnly();

        public Department(string departmentId, string name, string location)
        {
            if (string.IsNullOrWhiteSpace(departmentId))
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            DepartmentId = departmentId;
            Name = name;
            Location = location ?? "Unknown";
            _employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.Contains(employee))
                throw new InvalidOperationException("Employee already exists in this department");

            _employees.Add(employee);
            employee.AssignToDepartment(this);
        }

        public void RemoveEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _employees.Remove(employee);
        }

        public int GetEmployeeCount()
        {
            return _employees.Count;
        }

        public decimal GetTotalSalaryExpense()
        {
            return _employees.Sum(e => e.Salary);
        }

        public override string ToString()
        {
            return $"Department: {Name} ({DepartmentId}) - Location: {Location}, Employees: {_employees.Count}";
        }
    }
}
