using System;

namespace Day2.DomainModels.Entities
{
    public class Employee : Person
    {
        public string EmployeeId { get; private set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public Department? Department { get; set; }

        public Employee(string employeeId, string name, string email, DateTime hireDate, decimal salary)
            : base(name, email)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            if (salary < 0)
                throw new ArgumentException("Salary cannot be negative", nameof(salary));

            EmployeeId = employeeId;
            HireDate = hireDate;
            Salary = salary;
        }

        public int GetYearsOfService()
        {
            return (DateTime.Now - HireDate).Days / 365;
        }

        public override string GetDisplayInfo()
        {
            return $"Employee: {Name} ({EmployeeId}) - {Email}, Hired: {HireDate:yyyy-MM-dd}, Salary: ${Salary:N2}";
        }

        public void AssignToDepartment(Department department)
        {
            Department = department ?? throw new ArgumentNullException(nameof(department));
        }
    }
}
