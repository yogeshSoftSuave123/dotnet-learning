using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Day4.AsyncApp;

namespace Day4.Tests
{
    public class EmployeeServiceTests
    {
        // Test 1: Get all employees returns non-empty list
        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
        {
            // Arrange
            var service = new EmployeeService();

            // Act
            var result = await service.GetAllEmployeesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        // Test 2: Get employee by valid ID returns correct employee
        [Fact]
        public async Task GetEmployeeByIdAsync_ValidId_ReturnsEmployee()
        {
            // Arrange
            var service = new EmployeeService();
            var expectedId = 1;

            // Act
            var result = await service.GetEmployeeByIdAsync(expectedId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedId, result.Id);
            Assert.Equal("Alice Johnson", result.Name);
        }

        // Test 3: Get employee by invalid ID returns null
        [Fact]
        public async Task GetEmployeeByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var service = new EmployeeService();
            var invalidId = 999;

            // Act
            var result = await service.GetEmployeeByIdAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        // Test 4: Get employee with zero or negative ID throws exception
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public async Task GetEmployeeByIdAsync_ZeroOrNegativeId_ThrowsArgumentException(int invalidId)
        {
            // Arrange
            var service = new EmployeeService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetEmployeeByIdAsync(invalidId));
        }

        // Test 5: Calculate average salary for valid department
        [Fact]
        public async Task CalculateAverageSalaryAsync_ValidDepartment_ReturnsCorrectAverage()
        {
            // Arrange
            var service = new EmployeeService();
            var department = "Engineering";
            var expectedAverage = (95000 + 105000 + 88000) / 3m;  // 3 engineering employees

            // Act
            var result = await service.CalculateAverageSalaryAsync(department);

            // Assert
            Assert.Equal(expectedAverage, result);
        }

        // Test 6: Calculate average salary for non-existent department returns zero
        [Fact]
        public async Task CalculateAverageSalaryAsync_NonExistentDepartment_ReturnsZero()
        {
            // Arrange
            var service = new EmployeeService();
            var nonExistentDept = "Finance";

            // Act
            var result = await service.CalculateAverageSalaryAsync(nonExistentDept);

            // Assert
            Assert.Equal(0m, result);
        }

        // Test 7: Calculate average salary with null department throws exception
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CalculateAverageSalaryAsync_NullOrEmptyDepartment_ThrowsArgumentException(string? department)
        {
            // Arrange
            var service = new EmployeeService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CalculateAverageSalaryAsync(department!));
        }

        // Test 8: Get statistics returns valid data
        [Fact]
        public async Task GetStatisticsAsync_ReturnsValidStatistics()
        {
            // Arrange
            var service = new EmployeeService();

            // Act
            var result = await service.GetStatisticsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.TotalEmployees);
            Assert.True(result.AverageSalary > 0);
            Assert.True(result.TotalSalaryBudget > 0);
            Assert.True(result.DepartmentCount > 0);
            Assert.Equal(3, result.DepartmentCount);  // Engineering, HR, Sales
        }

        // Test 9: Cancellation token cancels operation
        [Fact]
        public async Task GetAllEmployeesAsync_CancellationToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var service = new EmployeeService();
            var cts = new CancellationTokenSource();
            cts.Cancel();  // Cancel immediately

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                service.GetAllEmployeesAsync(cts.Token));
        }

        // Test 10: Search with valid term returns matching employees
        [Fact]
        public async Task SearchEmployeesAsync_ValidTerm_ReturnsMatchingEmployees()
        {
            // Arrange
            var service = new EmployeeService();
            var searchTerm = "Alice";

            // Act
            var result = await service.SearchEmployeesAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Alice Johnson", result.First().Name);
        }

        // Test 11: Search with empty term returns empty list
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SearchEmployeesAsync_EmptyTerm_ReturnsEmptyList(string? searchTerm)
        {
            // Arrange
            var service = new EmployeeService();

            // Act
            var result = await service.SearchEmployeesAsync(searchTerm!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Test 12: Get department averages returns correct data
        [Fact]
        public async Task GetDepartmentAveragesAsync_ReturnsCorrectAverages()
        {
            // Arrange
            var service = new EmployeeService();

            // Act
            var result = await service.GetDepartmentAveragesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);  // Engineering, HR, Sales
            Assert.True(result.ContainsKey("Engineering"));
            Assert.True(result.ContainsKey("HR"));
            Assert.True(result.ContainsKey("Sales"));
        }

        // Test 13: Get employee count returns correct value
        [Fact]
        public async Task GetEmployeeCountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var service = new EmployeeService();

            // Act
            var result = await service.GetEmployeeCountAsync();

            // Assert
            Assert.Equal(5, result);
        }

        // Test 14: Search with timeout throws TimeoutException
        [Fact]
        public async Task SearchEmployeesWithTimeoutAsync_ShortTimeout_ThrowsTimeoutException()
        {
            // Arrange
            var service = new EmployeeService();
            var shortTimeout = 100;  // 100ms, but search takes 2000ms

            // Act & Assert
            await Assert.ThrowsAsync<TimeoutException>(() =>
                service.SearchEmployeesWithTimeoutAsync("test", shortTimeout));
        }

        // Test 15: Cancellation during statistics calculation
        [Fact]
        public async Task GetStatisticsAsync_CancelledMidway_ThrowsOperationCanceledException()
        {
            // Arrange
            var service = new EmployeeService();
            var cts = new CancellationTokenSource(500);  // Cancel after 500ms

            // Act & Assert
            // GetStatisticsAsync takes ~1200ms (4 delays of 300ms each)
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                service.GetStatisticsAsync(cts.Token));
        }

        // Test 16: Process employees with progress reports progress
        [Fact]
        public async Task ProcessEmployeesWithProgressAsync_ReportsProgress()
        {
            // Arrange
            var service = new EmployeeService();
            var progressValues = new System.Collections.Generic.List<int>();
            var progress = new Progress<int>(value => progressValues.Add(value));

            // Act
            var result = await service.ProcessEmployeesWithProgressAsync(
                e => e.Salary > 80000,
                progress);

            // Assert
            Assert.NotEmpty(result);
            Assert.NotEmpty(progressValues);
            Assert.Contains(100, progressValues);  // Should reach 100%
        }

        // Test 17: Boundary test - search by department
        [Fact]
        public async Task SearchEmployeesAsync_SearchByDepartment_ReturnsMatches()
        {
            // Arrange
            var service = new EmployeeService();
            var searchTerm = "Engineering";

            // Act
            var result = await service.SearchEmployeesAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, emp => Assert.Equal("Engineering", emp.Department));
        }

        // Test 18: Edge case - concurrent operations
        [Fact]
        public async Task GetEmployeeByIdAsync_ConcurrentCalls_AllSucceed()
        {
            // Arrange
            var service = new EmployeeService();
            var tasks = new System.Collections.Generic.List<Task<Employee?>>();

            // Act
            for (int i = 1; i <= 5; i++)
            {
                var id = i;
                tasks.Add(service.GetEmployeeByIdAsync(id));
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(5, results.Length);
            Assert.All(results, emp => Assert.NotNull(emp));
        }
    }
}
