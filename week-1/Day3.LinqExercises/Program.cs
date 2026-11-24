using System;
using System.Collections.Generic;
using System.Linq;

namespace Day3.LinqExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== LINQ Exercises - Employee Management ===\n");

            var exercises = new LinqExercises();
            exercises.RunAllExercises();
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public int Age { get; set; }
        public string City { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"[{Id}] {Name,-20} | {Department,-15} | ${Salary,8:N0} | {HireDate:yyyy-MM-dd} | Age: {Age}";
        }
    }

    public class LinqExercises
    {
        private List<Employee> employees;

        public LinqExercises()
        {
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice Johnson", Department = "Engineering", Salary = 95000, HireDate = DateTime.Parse("2020-01-15"), Age = 32, City = "Seattle" },
                new Employee { Id = 2, Name = "Bob Smith", Department = "HR", Salary = 65000, HireDate = DateTime.Parse("2019-03-22"), Age = 45, City = "Portland" },
                new Employee { Id = 3, Name = "Charlie Brown", Department = "Engineering", Salary = 105000, HireDate = DateTime.Parse("2018-07-10"), Age = 38, City = "Seattle" },
                new Employee { Id = 4, Name = "Diana Prince", Department = "Sales", Salary = 75000, HireDate = DateTime.Parse("2021-05-18"), Age = 29, City = "Austin" },
                new Employee { Id = 5, Name = "Eve Davis", Department = "Engineering", Salary = 88000, HireDate = DateTime.Parse("2022-02-01"), Age = 27, City = "Seattle" },
                new Employee { Id = 6, Name = "Frank Miller", Department = "Marketing", Salary = 70000, HireDate = DateTime.Parse("2020-11-30"), Age = 35, City = "Portland" },
                new Employee { Id = 7, Name = "Grace Hopper", Department = "Engineering", Salary = 120000, HireDate = DateTime.Parse("2017-04-12"), Age = 42, City = "Seattle" },
                new Employee { Id = 8, Name = "Henry Ford", Department = "Sales", Salary = 82000, HireDate = DateTime.Parse("2019-09-05"), Age = 40, City = "Austin" },
                new Employee { Id = 9, Name = "Ivy Chen", Department = "HR", Salary = 72000, HireDate = DateTime.Parse("2021-01-20"), Age = 31, City = "Portland" },
                new Employee { Id = 10, Name = "Jack Wilson", Department = "Marketing", Salary = 68000, HireDate = DateTime.Parse("2022-06-15"), Age = 26, City = "Austin" }
            };
        }

        public void RunAllExercises()
        {
            Query1_FilterBySalary();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query2_SelectNameAndDepartment();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query3_GroupByDepartment();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query4_OrderBySalaryDesc();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query5_AggregateOperations();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query6_ComplexFiltering();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query7_JoinAndGroupBy();
            Console.WriteLine("\n" + new string('-', 100) + "\n");

            Query8_DeferredExecution();
            Console.WriteLine("\n" + new string('-', 100) + "\n");
        }

        // Query 1: Filter employees by salary > 80000
        public void Query1_FilterBySalary()
        {
            Console.WriteLine("Query 1: Filter Employees with Salary > $80,000");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) - Linear scan through collection");

            var highEarners = employees.Where(e => e.Salary > 80000).ToList();

            Console.WriteLine($"\nOutput: {highEarners.Count} employees found");
            foreach (var emp in highEarners)
            {
                Console.WriteLine(emp);
            }
        }

        // Query 2: Select (Project) specific fields
        public void Query2_SelectNameAndDepartment()
        {
            Console.WriteLine("Query 2: Select (Project) Name and Department");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) - Transform each element");

            var projection = employees.Select(e => new
            {
                e.Name,
                e.Department,
                YearsOfService = (DateTime.Now - e.HireDate).Days / 365
            }).ToList();

            Console.WriteLine($"\nOutput: {projection.Count} records");
            foreach (var item in projection)
            {
                Console.WriteLine($"{item.Name,-20} | {item.Department,-15} | Years: {item.YearsOfService}");
            }
        }

        // Query 3: Group by department
        public void Query3_GroupByDepartment()
        {
            Console.WriteLine("Query 3: Group Employees by Department");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) - Group operation");

            var grouped = employees.GroupBy(e => e.Department).ToList();

            Console.WriteLine($"\nOutput: {grouped.Count} departments");
            foreach (var group in grouped)
            {
                Console.WriteLine($"\nDepartment: {group.Key}");
                Console.WriteLine($"Employee Count: {group.Count()}");
                Console.WriteLine($"Average Salary: ${group.Average(e => e.Salary):N2}");
                foreach (var emp in group)
                {
                    Console.WriteLine($"  - {emp.Name}: ${emp.Salary:N0}");
                }
            }
        }

        // Query 4: Order by salary descending
        public void Query4_OrderBySalaryDesc()
        {
            Console.WriteLine("Query 4: Order Employees by Salary (Descending)");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n log n) - Sorting operation");

            var sorted = employees.OrderByDescending(e => e.Salary)
                                 .ThenBy(e => e.Name)
                                 .ToList();

            Console.WriteLine($"\nOutput: {sorted.Count} employees (sorted)");
            foreach (var emp in sorted)
            {
                Console.WriteLine(emp);
            }
        }

        // Query 5: Aggregate operations (Sum, Average, Min, Max, Count)
        public void Query5_AggregateOperations()
        {
            Console.WriteLine("Query 5: Aggregate Operations");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) for each aggregate");

            Console.WriteLine("\nOutput:");
            Console.WriteLine($"Total Employees: {employees.Count()}");
            Console.WriteLine($"Total Salary Budget: ${employees.Sum(e => e.Salary):N2}");
            Console.WriteLine($"Average Salary: ${employees.Average(e => e.Salary):N2}");
            Console.WriteLine($"Highest Salary: ${employees.Max(e => e.Salary):N2}");
            Console.WriteLine($"Lowest Salary: ${employees.Min(e => e.Salary):N2}");
            Console.WriteLine($"Average Age: {employees.Average(e => e.Age):F1} years");
        }

        // Query 6: Complex filtering with multiple conditions
        public void Query6_ComplexFiltering()
        {
            Console.WriteLine("Query 6: Complex Filtering (Multiple Conditions)");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Filter: Department = 'Engineering' OR 'Sales', Salary > 75000, Age < 40");
            Console.WriteLine("Expected Complexity: O(n) - Single pass with multiple predicates");

            var filtered = employees.Where(e =>
                (e.Department == "Engineering" || e.Department == "Sales") &&
                e.Salary > 75000 &&
                e.Age < 40
            ).ToList();

            Console.WriteLine($"\nOutput: {filtered.Count} employees found");
            foreach (var emp in filtered)
            {
                Console.WriteLine(emp);
            }
        }

        // Query 7: Combining GroupBy with complex aggregation
        public void Query7_JoinAndGroupBy()
        {
            Console.WriteLine("Query 7: Group by City and Calculate Statistics");
            Console.WriteLine("Input: List of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) - Grouping and aggregation");

            var cityStats = employees.GroupBy(e => e.City)
                .Select(g => new
                {
                    City = g.Key,
                    EmployeeCount = g.Count(),
                    TotalSalary = g.Sum(e => e.Salary),
                    AverageSalary = g.Average(e => e.Salary),
                    Departments = g.Select(e => e.Department).Distinct().Count()
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ToList();

            Console.WriteLine($"\nOutput: {cityStats.Count} cities");
            foreach (var stat in cityStats)
            {
                Console.WriteLine($"\nCity: {stat.City}");
                Console.WriteLine($"  Employees: {stat.EmployeeCount}");
                Console.WriteLine($"  Total Salary Budget: ${stat.TotalSalary:N2}");
                Console.WriteLine($"  Average Salary: ${stat.AverageSalary:N2}");
                Console.WriteLine($"  Number of Departments: {stat.Departments}");
            }
        }

        // Query 8: Demonstrating deferred execution
        public void Query8_DeferredExecution()
        {
            Console.WriteLine("Query 8: Deferred Execution Demonstration");
            Console.WriteLine("Input: Initial list of 10 employees");
            Console.WriteLine("Expected Complexity: O(n) - Executed only when enumerated");

            // Define query (not executed yet)
            var query = employees.Where(e => e.Department == "Engineering")
                                .Select(e => e.Name);

            Console.WriteLine("\nQuery defined (not executed yet)");
            Console.WriteLine($"Initial Engineering employees: {employees.Count(e => e.Department == "Engineering")}");

            // Add new employee
            employees.Add(new Employee
            {
                Id = 11,
                Name = "Karen Lee",
                Department = "Engineering",
                Salary = 92000,
                HireDate = DateTime.Now,
                Age = 30,
                City = "Seattle"
            });

            Console.WriteLine($"After adding new employee: {employees.Count(e => e.Department == "Engineering")}");

            // Execute query now (includes the newly added employee)
            var result = query.ToList();

            Console.WriteLine($"\nOutput: {result.Count} Engineering employees (includes newly added)");
            foreach (var name in result)
            {
                Console.WriteLine($"  - {name}");
            }

            Console.WriteLine("\nNote: Query was deferred and executed after adding new employee!");
        }

        // Unit-testable methods

        public List<Employee> FilterByDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            return employees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        public Dictionary<string, int> GroupAndCountByDepartment()
        {
            return employees.GroupBy(e => e.Department)
                           .ToDictionary(g => g.Key, g => g.Count());
        }

        public decimal CalculateAverageSalaryByDepartment(string department)
        {
            var deptEmployees = employees.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

            if (!deptEmployees.Any())
                return 0m;

            return deptEmployees.Average(e => e.Salary);
        }

        public List<Employee> SearchByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Employee>();

            return employees.Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        public List<Employee> GetTopEarners(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be positive", nameof(count));

            return employees.OrderByDescending(e => e.Salary)
                           .Take(count)
                           .ToList();
        }
    }
}
