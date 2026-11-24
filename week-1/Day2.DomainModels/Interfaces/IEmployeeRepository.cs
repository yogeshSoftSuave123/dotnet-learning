using System.Collections.Generic;
using Day2.DomainModels.Entities;

namespace Day2.DomainModels.Interfaces
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        void Remove(string employeeId);
        Employee? GetById(string employeeId);
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> GetByDepartment(string departmentId);
        void Update(Employee employee);
    }
}
