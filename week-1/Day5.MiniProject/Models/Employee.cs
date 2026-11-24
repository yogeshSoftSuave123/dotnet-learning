using System;

namespace Day5.MiniProject.Models
{
    public class Employee
    {
        public string EmployeeId { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }

        public Employee(string employeeId, string name, string email, string department,
            decimal salary, DateTime hireDate, int age)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("Employee ID cannot be empty", nameof(employeeId));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (salary < 0)
                throw new ArgumentException("Salary cannot be negative", nameof(salary));

            if (age < 18 || age > 100)
                throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 18 and 100");

            EmployeeId = employeeId;
            Name = name;
            Email = email;
            Department = department;
            Salary = salary;
            HireDate = hireDate;
            Age = age;
            IsActive = true;
        }

        public int GetYearsOfService()
        {
            return (DateTime.Now - HireDate).Days / 365;
        }

        public decimal CalculateAnnualBonus(decimal bonusPercentage)
        {
            if (bonusPercentage < 0 || bonusPercentage > 100)
                throw new ArgumentOutOfRangeException(nameof(bonusPercentage),
                    "Bonus percentage must be between 0 and 100");

            return Salary * (bonusPercentage / 100);
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public override string ToString()
        {
            var status = IsActive ? "Active" : "Inactive";
            return $"[{EmployeeId}] {Name} | {Department} | ${Salary:N0} | {status}";
        }
    }
}
