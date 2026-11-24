using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Day4.AsyncApp
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }

    public class EmployeeService
    {
        private List<Employee> _employees;

        public EmployeeService()
        {
            _employees = new List<Employee>();
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            _employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice Johnson", Department = "Engineering", Salary = 95000 },
                new Employee { Id = 2, Name = "Bob Smith", Department = "HR", Salary = 65000 },
                new Employee { Id = 3, Name = "Charlie Brown", Department = "Engineering", Salary = 105000 },
                new Employee { Id = 4, Name = "Diana Prince", Department = "Sales", Salary = 75000 },
                new Employee { Id = 5, Name = "Eve Davis", Department = "Engineering", Salary = 88000 }
            };
        }

        // Async method that simulates database query with cancellation support
        public async Task<List<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            // Simulate async database call
            await Task.Delay(1000, cancellationToken);

            // Check if cancellation was requested
            cancellationToken.ThrowIfCancellationRequested();

            return _employees.ToList();
        }

        // Async method with cancellation token
        public async Task<Employee?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ArgumentException("ID must be positive", nameof(id));

            // Simulate async operation
            await Task.Delay(500, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _employees.FirstOrDefault(e => e.Id == id);
        }

        // Async method that processes data
        public async Task<decimal> CalculateAverageSalaryAsync(string department, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty", nameof(department));

            // Simulate async processing
            await Task.Delay(800, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var deptEmployees = _employees.Where(e =>
                e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));

            if (!deptEmployees.Any())
                return 0m;

            return deptEmployees.Average(e => e.Salary);
        }

        // Async method with multiple operations and cancellation checks
        public async Task<EmployeeStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            var stats = new EmployeeStatistics();

            // Simulate multiple async operations
            await Task.Delay(300, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            stats.TotalEmployees = _employees.Count;

            await Task.Delay(300, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            stats.AverageSalary = _employees.Average(e => e.Salary);

            await Task.Delay(300, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            stats.TotalSalaryBudget = _employees.Sum(e => e.Salary);

            await Task.Delay(300, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            stats.DepartmentCount = _employees.Select(e => e.Department).Distinct().Count();

            return stats;
        }

        // Async method that runs multiple tasks in parallel
        public async Task<Dictionary<string, decimal>> GetDepartmentAveragesAsync(CancellationToken cancellationToken = default)
        {
            var departments = _employees.Select(e => e.Department).Distinct().ToList();
            var tasks = new List<Task<(string Department, decimal Average)>>();

            foreach (var dept in departments)
            {
                tasks.Add(CalculateDepartmentAverageAsync(dept, cancellationToken));
            }

            var results = await Task.WhenAll(tasks);

            return results.ToDictionary(r => r.Department, r => r.Average);
        }

        private async Task<(string Department, decimal Average)> CalculateDepartmentAverageAsync(
            string department,
            CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken);

            var avg = _employees
                .Where(e => e.Department == department)
                .Average(e => e.Salary);

            return (department, avg);
        }

        // Async method with timeout handling
        public async Task<List<Employee>> SearchEmployeesWithTimeoutAsync(
            string searchTerm,
            int timeoutMilliseconds = 5000)
        {
            using var cts = new CancellationTokenSource(timeoutMilliseconds);

            try
            {
                return await SearchEmployeesAsync(searchTerm, cts.Token);
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException($"Search operation timed out after {timeoutMilliseconds}ms");
            }
        }

        public async Task<List<Employee>> SearchEmployeesAsync(
            string searchTerm,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Employee>();

            // Simulate expensive search operation
            await Task.Delay(2000, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return _employees
                .Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           e.Department.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Method demonstrating ValueTask for performance
        public ValueTask<int> GetEmployeeCountAsync()
        {
            // For synchronous operations, ValueTask can avoid allocation
            return new ValueTask<int>(_employees.Count);
        }

        // Async method with progress reporting
        public async Task<List<Employee>> ProcessEmployeesWithProgressAsync(
            Func<Employee, bool> filter,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            var result = new List<Employee>();
            var totalCount = _employees.Count;

            for (int i = 0; i < totalCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var employee = _employees[i];

                // Simulate processing
                await Task.Delay(100, cancellationToken);

                if (filter(employee))
                {
                    result.Add(employee);
                }

                // Report progress
                var percentComplete = (int)((i + 1) * 100.0 / totalCount);
                progress?.Report(percentComplete);
            }

            return result;
        }
    }

    public class EmployeeStatistics
    {
        public int TotalEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalSalaryBudget { get; set; }
        public int DepartmentCount { get; set; }

        public override string ToString()
        {
            return $"Total: {TotalEmployees}, Avg Salary: ${AverageSalary:N2}, " +
                   $"Budget: ${TotalSalaryBudget:N2}, Departments: {DepartmentCount}";
        }
    }
}
