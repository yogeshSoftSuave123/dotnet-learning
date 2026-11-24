using System;
using System.Collections.Generic;
using System.Linq;
using Day2.DomainModels.Entities;
using Day2.DomainModels.Interfaces;

namespace Day2.DomainModels.Repositories
{
    public class InMemoryDepartmentRepository : IDepartmentRepository
    {
        private readonly Dictionary<string, Department> _departments;

        public InMemoryDepartmentRepository()
        {
            _departments = new Dictionary<string, Department>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            if (_departments.ContainsKey(department.DepartmentId))
                throw new InvalidOperationException($"Department with ID {department.DepartmentId} already exists");

            _departments[department.DepartmentId] = department;
        }

        public void Remove(string departmentId)
        {
            if (string.IsNullOrWhiteSpace(departmentId))
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

            if (!_departments.Remove(departmentId))
                throw new KeyNotFoundException($"Department with ID {departmentId} not found");
        }

        public Department? GetById(string departmentId)
        {
            if (string.IsNullOrWhiteSpace(departmentId))
                throw new ArgumentException("Department ID cannot be empty", nameof(departmentId));

            _departments.TryGetValue(departmentId, out var department);
            return department;
        }

        public IEnumerable<Department> GetAll()
        {
            return _departments.Values.ToList();
        }

        public void Update(Department department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            if (!_departments.ContainsKey(department.DepartmentId))
                throw new KeyNotFoundException($"Department with ID {department.DepartmentId} not found");

            _departments[department.DepartmentId] = department;
        }
    }
}
