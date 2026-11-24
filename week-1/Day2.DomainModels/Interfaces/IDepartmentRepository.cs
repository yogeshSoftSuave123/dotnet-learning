using System.Collections.Generic;
using Day2.DomainModels.Entities;

namespace Day2.DomainModels.Interfaces
{
    public interface IDepartmentRepository
    {
        void Add(Department department);
        void Remove(string departmentId);
        Department? GetById(string departmentId);
        IEnumerable<Department> GetAll();
        void Update(Department department);
    }
}
