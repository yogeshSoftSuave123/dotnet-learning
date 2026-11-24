using System;
using System.Collections.Generic;
using System.Linq;

namespace Day1.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Employee Management System ===\n");

            var manager = new EmployeeManager();

            // Sample 1: Adding employees
            Console.WriteLine("Sample 1: Adding Employees");
            manager.AddEmployee(new Employee(1, "John Doe", "Engineering", 75000m));
            manager.AddEmployee(new Employee(2, "Jane Smith", "HR", 65000m));
            manager.AddEmployee(new Employee(3, "Bob Johnson", "Engineering", 80000m));
            manager.DisplayAllEmployees();
            Console.WriteLine();

            // Sample 2: Filtering by department
            Console.WriteLine("Sample 2: Filter by Department (Engineering)");
            var engineers = manager.GetEmployeesByDepartment("Engineering");
            foreach (var emp in engineers)
            {
                Console.WriteLine(emp);
            }
            Console.WriteLine();

            // Sample 3: Calculating average salary
            Console.WriteLine("Sample 3: Average Salary Calculation");
            decimal avgSalary = manager.CalculateAverageSalary();
            Console.WriteLine($"Average Salary: ${avgSalary:F2}");
            Console.WriteLine();
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }

        public Employee(int id, string name, string department, decimal salary)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Department = department ?? throw new ArgumentNullException(nameof(department));

            if (salary < 0)
                throw new ArgumentException("Salary cannot be negative", nameof(salary));

            Salary = salary;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Department: {Department}, Salary: ${Salary:F2}";
        }
    }

    public class EmployeeManager
    {
        private List<Employee> employees;

        public EmployeeManager()
        {
            employees = new List<Employee>();
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            employees.Add(employee);
            Console.WriteLine($"Added: {employee.Name}");
        }

        public List<Employee> GetEmployeesByDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            return employees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        public decimal CalculateAverageSalary()
        {
            if (employees.Count == 0)
                return 0m;

            return employees.Average(e => e.Salary);
        }

        public void DisplayAllEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                return;
            }

            Console.WriteLine($"Total Employees: {employees.Count}");
            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
        }

        public Employee? FindEmployeeById(int id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
        }
    }
}
