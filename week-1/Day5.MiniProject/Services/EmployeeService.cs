using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Day5.MiniProject.Interfaces;
using Day5.MiniProject.Models;

namespace Day5.MiniProject.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<decimal> CalculateAverageSalaryAsync(
            string? department = null,
            CancellationToken cancellationToken = default)
        {
            var employees = await _repository.GetAllAsync(cancellationToken);

            if (department != null)
            {
                employees = employees
                    .Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (employees.Count == 0)
                return 0m;

            return employees.Average(e => e.Salary);
        }

        public async Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync(
            CancellationToken cancellationToken = default)
        {
            var employees = await _repository.GetAllAsync(cancellationToken);

            return employees
                .GroupBy(e => e.Department)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<List<Employee>> GetTopEarnersAsync(
            int count,
            CancellationToken cancellationToken = default)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be positive", nameof(count));

            var employees = await _repository.GetAllAsync(cancellationToken);

            return employees
                .OrderByDescending(e => e.Salary)
                .Take(count)
                .ToList();
        }

        public async Task<List<Employee>> SearchEmployeesAsync(
            string searchTerm,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Employee>();

            var employees = await _repository.GetAllAsync(cancellationToken);

            return employees
                .Where(e =>
                    e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    e.Department.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<EmployeeStatistics> GetStatisticsAsync(
            CancellationToken cancellationToken = default)
        {
            var employees = await _repository.GetAllAsync(cancellationToken);

            if (employees.Count == 0)
            {
                return new EmployeeStatistics
                {
                    TotalEmployees = 0,
                    ActiveEmployees = 0,
                    InactiveEmployees = 0,
                    AverageSalary = 0,
                    TotalSalaryExpense = 0,
                    DepartmentCount = 0,
                    AverageAge = 0
                };
            }

            return new EmployeeStatistics
            {
                TotalEmployees = employees.Count,
                ActiveEmployees = employees.Count(e => e.IsActive),
                InactiveEmployees = employees.Count(e => !e.IsActive),
                AverageSalary = employees.Average(e => e.Salary),
                TotalSalaryExpense = employees.Sum(e => e.Salary),
                DepartmentCount = employees.Select(e => e.Department).Distinct().Count(),
                AverageAge = employees.Average(e => e.Age)
            };
        }

        public async Task<List<Employee>> GetEmployeesByAgeRangeAsync(
            int minAge,
            int maxAge,
            CancellationToken cancellationToken = default)
        {
            if (minAge < 0 || maxAge < 0)
                throw new ArgumentException("Ages must be non-negative");

            if (minAge > maxAge)
                throw new ArgumentException("Min age cannot be greater than max age");

            var employees = await _repository.GetAllAsync(cancellationToken);

            return employees
                .Where(e => e.Age >= minAge && e.Age <= maxAge)
                .OrderBy(e => e.Age)
                .ToList();
        }

        public async Task<bool> GiveBonusToTopPerformersAsync(
            int topCount,
            decimal bonusPercentage,
            CancellationToken cancellationToken = default)
        {
            if (topCount <= 0)
                throw new ArgumentException("Count must be positive", nameof(topCount));

            var topEarners = await GetTopEarnersAsync(topCount, cancellationToken);

            foreach (var employee in topEarners)
            {
                var bonus = employee.CalculateAnnualBonus(bonusPercentage);
                employee.Salary += bonus;
                await _repository.UpdateAsync(employee, cancellationToken);
            }

            return true;
        }

        public async Task<int> DeactivateEmployeesAsync(
            Func<Employee, bool> predicate,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var employees = await _repository.GetAllAsync(cancellationToken);
            var toDeactivate = employees.Where(predicate).ToList();

            foreach (var employee in toDeactivate)
            {
                employee.Deactivate();
                await _repository.UpdateAsync(employee, cancellationToken);
            }

            return toDeactivate.Count;
        }
    }

    public class EmployeeStatistics
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalSalaryExpense { get; set; }
        public int DepartmentCount { get; set; }
        public double AverageAge { get; set; }

        public override string ToString()
        {
            return $"Total: {TotalEmployees} ({ActiveEmployees} active, {InactiveEmployees} inactive)\n" +
                   $"Average Salary: ${AverageSalary:N2}\n" +
                   $"Total Expense: ${TotalSalaryExpense:N2}\n" +
                   $"Departments: {DepartmentCount}\n" +
                   $"Average Age: {AverageAge:F1} years";
        }
    }
}
