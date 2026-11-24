using Microsoft.Extensions.Logging;
using Moq;
using Day07.DI.Middleware.Models;
using Day07.DI.Middleware.Services;

namespace Day07.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly Mock<ILogger<EmployeeService>> _mockLogger;
    private readonly Mock<IGuidGeneratorService> _mockGuidGenerator;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeeRepository>();
        _mockLogger = new Mock<ILogger<EmployeeService>>();
        _mockGuidGenerator = new Mock<IGuidGeneratorService>();

        _mockGuidGenerator.Setup(g => g.GenerateGuid()).Returns(Guid.NewGuid());

        _service = new EmployeeService(_mockRepository.Object, _mockLogger.Object, _mockGuidGenerator.Object);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
    {
        // Arrange
        var expectedEmployees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(expectedEmployees);

        // Act
        var result = await _service.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ValidId_ReturnsEmployee()
    {
        // Arrange
        var expectedEmployee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expectedEmployee);

        // Act
        var result = await _service.GetEmployeeByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.GetEmployeeByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ValidEmployee_CreatesSuccessfully()
    {
        // Arrange
        var newEmployee = new Employee
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice@test.com",
            Department = "Marketing",
            Salary = 80000
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee e) => { e.Id = 3; return e; });

        // Act
        var result = await _service.CreateEmployeeAsync(newEmployee);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeAsync_MissingEmail_ThrowsArgumentException()
    {
        // Arrange
        var invalidEmployee = new Employee
        {
            FirstName = "Bob",
            LastName = "Wilson",
            Email = "",  // Missing email
            Department = "Sales",
            Salary = 70000
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.CreateEmployeeAsync(invalidEmployee));
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ExistingEmployee_UpdatesSuccessfully()
    {
        // Arrange
        var existingEmployee = new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com" };
        var updatedEmployee = new Employee { Id = 1, FirstName = "John", LastName = "Updated", Email = "john.updated@test.com" };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingEmployee);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Employee>())).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateEmployeeAsync(1, updatedEmployee);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<Employee>(e => e.Id == 1)), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_NonExistentEmployee_ReturnsFalse()
    {
        // Arrange
        var updatedEmployee = new Employee { Id = 999, FirstName = "Nobody", LastName = "Here", Email = "nobody@test.com" };
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.UpdateEmployeeAsync(999, updatedEmployee);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ExistingEmployee_DeletesSuccessfully()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteEmployeeAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetEmployeesByDepartmentAsync_ValidDepartment_ReturnsMatchingEmployees()
    {
        // Arrange
        var allEmployees = new List<Employee>
        {
            new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", Department = "Engineering" },
            new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Department = "HR" },
            new Employee { Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob@test.com", Department = "Engineering" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(allEmployees);

        // Act
        var result = await _service.GetEmployeesByDepartmentAsync("Engineering");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, e => Assert.Equal("Engineering", e.Department));
    }
}
