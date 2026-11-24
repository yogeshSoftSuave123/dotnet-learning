using System;
using System.Threading.Tasks;
using Day5.MiniProject.Models;
using Day5.MiniProject.Services;
using Day5.MiniProject.Interfaces;

namespace Day5.MiniProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Employee Management System - Week 1 Capstone ===\n");

            IEmployeeRepository repository = new EmployeeRepository();
            var service = new EmployeeService(repository);

            try
            {
                // Initialize sample data
                await InitializeSampleDataAsync(repository);

                // Demonstrate various features
                await DisplayAllEmployeesAsync(repository);
                await DisplayStatisticsAsync(service);
                await DemonstrateLinqQueriesAsync(service);
                await DemonstrateAsyncOperationsAsync(service);

                Console.WriteLine("\n=== All operations completed successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERROR: {ex.Message}");
            }
        }

        static async Task InitializeSampleDataAsync(IEmployeeRepository repository)
        {
            Console.WriteLine("Initializing sample data...\n");

            var employees = new[]
            {
                new Employee("E001", "Alice Johnson", "alice@company.com", "Engineering",
                    95000m, DateTime.Parse("2020-01-15"), 32),
                new Employee("E002", "Bob Smith", "bob@company.com", "HR",
                    65000m, DateTime.Parse("2019-03-22"), 45),
                new Employee("E003", "Charlie Brown", "charlie@company.com", "Engineering",
                    105000m, DateTime.Parse("2018-07-10"), 38),
                new Employee("E004", "Diana Prince", "diana@company.com", "Sales",
                    75000m, DateTime.Parse("2021-05-18"), 29),
                new Employee("E005", "Eve Davis", "eve@company.com", "Engineering",
                    88000m, DateTime.Parse("2022-02-01"), 27),
                new Employee("E006", "Frank Miller", "frank@company.com", "Marketing",
                    70000m, DateTime.Parse("2020-11-30"), 35),
                new Employee("E007", "Grace Hopper", "grace@company.com", "Engineering",
                    120000m, DateTime.Parse("2017-04-12"), 42),
                new Employee("E008", "Henry Ford", "henry@company.com", "Sales",
                    82000m, DateTime.Parse("2019-09-05"), 40)
            };

            foreach (var emp in employees)
            {
                await repository.AddAsync(emp);
            }

            Console.WriteLine($"Added {employees.Length} employees\n");
        }

        static async Task DisplayAllEmployeesAsync(IEmployeeRepository repository)
        {
            Console.WriteLine("=== All Employees ===");
            var employees = await repository.GetAllAsync();

            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
            Console.WriteLine();
        }

        static async Task DisplayStatisticsAsync(EmployeeService service)
        {
            Console.WriteLine("=== Employee Statistics ===");
            var stats = await service.GetStatisticsAsync();
            Console.WriteLine(stats);
            Console.WriteLine();
        }

        static async Task DemonstrateLinqQueriesAsync(EmployeeService service)
        {
            Console.WriteLine("=== LINQ Query Demonstrations ===\n");

            // 1. Top earners
            Console.WriteLine("Top 3 Earners:");
            var topEarners = await service.GetTopEarnersAsync(3);
            foreach (var emp in topEarners)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }
            Console.WriteLine();

            // 2. Department counts
            Console.WriteLine("Employee Count by Department:");
            var deptCounts = await service.GetEmployeeCountByDepartmentAsync();
            foreach (var kvp in deptCounts)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} employees");
            }
            Console.WriteLine();

            // 3. Average salary by department
            Console.WriteLine("Average Salary by Department:");
            foreach (var dept in new[] { "Engineering", "HR", "Sales", "Marketing" })
            {
                var avg = await service.CalculateAverageSalaryAsync(dept);
                if (avg > 0)
                    Console.WriteLine($"  {dept}: ${avg:N2}");
            }
            Console.WriteLine();

            // 4. Search employees
            Console.WriteLine("Search Results for 'Alice':");
            var searchResults = await service.SearchEmployeesAsync("Alice");
            foreach (var emp in searchResults)
            {
                Console.WriteLine($"  {emp}");
            }
            Console.WriteLine();

            // 5. Age range query
            Console.WriteLine("Employees aged 30-40:");
            var ageRange = await service.GetEmployeesByAgeRangeAsync(30, 40);
            foreach (var emp in ageRange)
            {
                Console.WriteLine($"  {emp.Name} (Age {emp.Age})");
            }
            Console.WriteLine();
        }

        static async Task DemonstrateAsyncOperationsAsync(EmployeeService service)
        {
            Console.WriteLine("=== Async Operations Demonstration ===\n");

            // Demonstrate cancellation token
            Console.WriteLine("Testing cancellation (this should complete normally):");
            try
            {
                using var cts = new System.Threading.CancellationTokenSource(5000); // 5 second timeout
                var stats = await service.GetStatisticsAsync(cts.Token);
                Console.WriteLine($"Statistics retrieved: {stats.TotalEmployees} employees\n");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled\n");
            }

            // Parallel async operations
            Console.WriteLine("Running multiple async queries in parallel:");
            var task1 = service.GetTopEarnersAsync(5);
            var task2 = service.GetEmployeeCountByDepartmentAsync();
            var task3 = service.GetStatisticsAsync();

            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine($"  Top earners: {task1.Result.Count}");
            Console.WriteLine($"  Departments: {task2.Result.Count}");
            Console.WriteLine($"  Total employees: {task3.Result.TotalEmployees}");
            Console.WriteLine();
        }
    }
}
