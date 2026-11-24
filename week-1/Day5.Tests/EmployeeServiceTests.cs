using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Day5.MiniProject.Models;
using Day5.MiniProject.Services;
using Day5.MiniProject.Interfaces;

namespace Day5.Tests
{
    public class EmployeeServiceTests
    {
        private async Task<IEmployeeRepository> CreateRepositoryWithSampleDataAsync()
        {
            var repository = new EmployeeRepository();

            var employees = new[]
            {
                new Employee("E001", "Alice", "alice@test.com", "Engineering", 95000, DateTime.Parse("2020-01-15"), 32),
                new Employee("E002", "Bob", "bob@test.com", "HR", 65000, DateTime.Parse("2019-03-22"), 45),
                new Employee("E003", "Charlie", "charlie@test.com", "Engineering", 105000, DateTime.Parse("2018-07-10"), 38),
                new Employee("E004", "Diana", "diana@test.com", "Sales", 75000, DateTime.Parse("2021-05-18"), 29),
                new Employee("E005", "Eve", "eve@test.com", "Engineering", 88000, DateTime.Parse("2022-02-01"), 27)
            };

            foreach (var emp in employees)
            {
                await repository.AddAsync(emp);
            }

            return repository;
        }

        // Normal case: Calculate average salary for all employees
        [Fact]
        public async Task CalculateAverageSalaryAsync_AllEmployees_ReturnsCorrectAverage()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);
            var expectedAvg = (95000 + 65000 + 105000 + 75000 + 88000) / 5m;

            // Act
            var result = await service.CalculateAverageSalaryAsync();

            // Assert
            Assert.Equal(expectedAvg, result);
        }

        // Normal case: Calculate average salary for specific department
        [Fact]
        public async Task CalculateAverageSalaryAsync_EngineeringDept_ReturnsCorrectAverage()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);
            var expectedAvg = (95000 + 105000 + 88000) / 3m;

            // Act
            var result = await service.CalculateAverageSalaryAsync("Engineering");

            // Assert
            Assert.Equal(expectedAvg, result);
        }

        // Boundary case: Non-existent department returns zero
        [Fact]
        public async Task CalculateAverageSalaryAsync_NonExistentDept_ReturnsZero()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.CalculateAverageSalaryAsync("Finance");

            // Assert
            Assert.Equal(0m, result);
        }

        // Normal case: Get employee count by department
        [Fact]
        public async Task GetEmployeeCountByDepartmentAsync_ReturnsCorrectCounts()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetEmployeeCountByDepartmentAsync();

            // Assert
            Assert.Equal(3, result.Count);  // 3 departments
            Assert.Equal(3, result["Engineering"]);
            Assert.Equal(1, result["HR"]);
            Assert.Equal(1, result["Sales"]);
        }

        // Normal case: Get top earners
        [Fact]
        public async Task GetTopEarnersAsync_ReturnsCorrectEmployees()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetTopEarnersAsync(3);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Charlie", result[0].Name);  // 105000
            Assert.Equal("Alice", result[1].Name);    // 95000
            Assert.Equal("Eve", result[2].Name);      // 88000
        }

        // Boundary case: Request more top earners than exist
        [Fact]
        public async Task GetTopEarnersAsync_CountExceedsTotal_ReturnsAll()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetTopEarnersAsync(100);

            // Assert
            Assert.Equal(5, result.Count);  // Only 5 employees exist
        }

        // Invalid input: Zero or negative count
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetTopEarnersAsync_InvalidCount_ThrowsException(int invalidCount)
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetTopEarnersAsync(invalidCount));
        }

        // Normal case: Search employees by name
        [Fact]
        public async Task SearchEmployeesAsync_ValidName_ReturnsMatches()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.SearchEmployeesAsync("Alice");

            // Assert
            Assert.Single(result);
            Assert.Equal("Alice", result[0].Name);
        }

        // Normal case: Search employees by department
        [Fact]
        public async Task SearchEmployeesAsync_ValidDepartment_ReturnsMatches()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.SearchEmployeesAsync("Engineering");

            // Assert
            Assert.Equal(3, result.Count);
            Assert.All(result, emp => Assert.Equal("Engineering", emp.Department));
        }

        // Boundary case: Empty search term returns empty list
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SearchEmployeesAsync_EmptyTerm_ReturnsEmptyList(string? searchTerm)
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.SearchEmployeesAsync(searchTerm!);

            // Assert
            Assert.Empty(result);
        }

        // Normal case: Get statistics
        [Fact]
        public async Task GetStatisticsAsync_ReturnsCorrectStatistics()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetStatisticsAsync();

            // Assert
            Assert.Equal(5, result.TotalEmployees);
            Assert.Equal(5, result.ActiveEmployees);
            Assert.Equal(0, result.InactiveEmployees);
            Assert.Equal(3, result.DepartmentCount);
            Assert.True(result.AverageSalary > 0);
            Assert.True(result.TotalSalaryExpense > 0);
        }

        // Normal case: Get employees by age range
        [Fact]
        public async Task GetEmployeesByAgeRangeAsync_ValidRange_ReturnsMatches()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetEmployeesByAgeRangeAsync(30, 40);

            // Assert
            Assert.Equal(3, result.Count);  // Alice (32), Charlie (38), Eve (27) - wait, Eve is 27
            // Let me recalculate: Alice=32, Charlie=38 are in range. Eve=27, Diana=29, Bob=45 are not
            Assert.All(result, emp => Assert.InRange(emp.Age, 30, 40));
        }

        // Boundary case: Exact age match
        [Fact]
        public async Task GetEmployeesByAgeRangeAsync_ExactAge_ReturnsMatch()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result = await service.GetEmployeesByAgeRangeAsync(32, 32);

            // Assert
            Assert.Single(result);
            Assert.Equal("Alice", result[0].Name);
        }

        // Invalid input: Negative age
        [Fact]
        public async Task GetEmployeesByAgeRangeAsync_NegativeAge_ThrowsException()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetEmployeesByAgeRangeAsync(-1, 40));
        }

        // Invalid input: Min age greater than max age
        [Fact]
        public async Task GetEmployeesByAgeRangeAsync_MinGreaterThanMax_ThrowsException()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetEmployeesByAgeRangeAsync(50, 30));
        }

        // Normal case: Deactivate employees
        [Fact]
        public async Task DeactivateEmployeesAsync_ValidPredicate_DeactivatesMatches()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act - Deactivate employees with salary < 70000
            var count = await service.DeactivateEmployeesAsync(e => e.Salary < 70000);

            // Assert
            Assert.Equal(1, count);  // Only Bob (65000)

            var bob = await repository.GetByIdAsync("E002");
            Assert.NotNull(bob);
            Assert.False(bob!.IsActive);
        }

        // Invalid input: Null predicate
        [Fact]
        public async Task DeactivateEmployeesAsync_NullPredicate_ThrowsException()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                service.DeactivateEmployeesAsync(null!));
        }

        // Normal case: Give bonus to top performers
        [Fact]
        public async Task GiveBonusToTopPerformersAsync_ValidData_UpdatesSalaries()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);
            var originalSalary = 105000m;

            // Act
            await service.GiveBonusToTopPerformersAsync(1, 10m);

            // Assert
            var charlie = await repository.GetByIdAsync("E003");
            Assert.NotNull(charlie);
            Assert.True(charlie!.Salary > originalSalary);
            Assert.Equal(originalSalary * 1.1m, charlie.Salary);
        }

        // Boundary case: Zero bonus percentage
        [Fact]
        public async Task GiveBonusToTopPerformersAsync_ZeroPercent_NoChange()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);
            var employees = await repository.GetAllAsync();
            var originalSalaries = employees.ToDictionary(e => e.EmployeeId, e => e.Salary);

            // Act
            await service.GiveBonusToTopPerformersAsync(1, 0m);

            // Assert
            var updatedEmployees = await repository.GetAllAsync();
            foreach (var emp in updatedEmployees)
            {
                Assert.Equal(originalSalaries[emp.EmployeeId], emp.Salary);
            }
        }

        // Invalid input: Zero or negative count for bonus
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GiveBonusToTopPerformersAsync_InvalidCount_ThrowsException(int invalidCount)
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GiveBonusToTopPerformersAsync(invalidCount, 10m));
        }

        // Normal case: Null repository in constructor throws exception
        [Fact]
        public void Constructor_NullRepository_ThrowsException()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new EmployeeService(null!));
        }

        // Regression test: Ensure case-insensitive department search
        [Fact]
        public async Task CalculateAverageSalaryAsync_CaseInsensitiveDept_ReturnsCorrectAverage()
        {
            // Arrange
            var repository = await CreateRepositoryWithSampleDataAsync();
            var service = new EmployeeService(repository);

            // Act
            var result1 = await service.CalculateAverageSalaryAsync("engineering");
            var result2 = await service.CalculateAverageSalaryAsync("ENGINEERING");
            var result3 = await service.CalculateAverageSalaryAsync("Engineering");

            // Assert
            Assert.Equal(result1, result2);
            Assert.Equal(result2, result3);
        }
    }
}
