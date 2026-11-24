using System;
using Xunit;
using Day5.MiniProject.Models;

namespace Day5.Tests
{
    public class EmployeeTests
    {
        // Normal case: Create employee with valid data
        [Fact]
        public void Constructor_ValidData_CreatesEmployee()
        {
            // Arrange
            var id = "E001";
            var name = "John Doe";
            var email = "john@company.com";
            var dept = "Engineering";
            var salary = 75000m;
            var hireDate = DateTime.Parse("2020-01-15");
            var age = 30;

            // Act
            var employee = new Employee(id, name, email, dept, salary, hireDate, age);

            // Assert
            Assert.Equal(id, employee.EmployeeId);
            Assert.Equal(name, employee.Name);
            Assert.Equal(email, employee.Email);
            Assert.Equal(dept, employee.Department);
            Assert.Equal(salary, employee.Salary);
            Assert.Equal(hireDate, employee.HireDate);
            Assert.Equal(age, employee.Age);
            Assert.True(employee.IsActive);
        }

        // Boundary case: Minimum age (18)
        [Fact]
        public void Constructor_MinimumAge_CreatesEmployee()
        {
            // Arrange & Act
            var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, 18);

            // Assert
            Assert.Equal(18, employee.Age);
        }

        // Boundary case: Maximum age (100)
        [Fact]
        public void Constructor_MaximumAge_CreatesEmployee()
        {
            // Arrange & Act
            var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, 100);

            // Assert
            Assert.Equal(100, employee.Age);
        }

        // Invalid input: Empty employee ID
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_EmptyEmployeeId_ThrowsException(string? invalidId)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Employee(invalidId!, "John", "john@test.com", "IT", 50000, DateTime.Now, 30));
        }

        // Invalid input: Empty name
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_EmptyName_ThrowsException(string? invalidName)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Employee("E001", invalidName!, "john@test.com", "IT", 50000, DateTime.Now, 30));
        }

        // Invalid input: Negative salary
        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void Constructor_NegativeSalary_ThrowsException(decimal invalidSalary)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Employee("E001", "John", "john@test.com", "IT", invalidSalary, DateTime.Now, 30));
        }

        // Invalid input: Age below 18
        [Theory]
        [InlineData(17)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_AgeBelowMinimum_ThrowsException(int invalidAge)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, invalidAge));
        }

        // Invalid input: Age above 100
        [Theory]
        [InlineData(101)]
        [InlineData(150)]
        public void Constructor_AgeAboveMaximum_ThrowsException(int invalidAge)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, invalidAge));
        }

        // Normal case: Get years of service
        [Fact]
        public void GetYearsOfService_ReturnsCorrectYears()
        {
            // Arrange
            var hireDate = DateTime.Now.AddYears(-5);
            var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, hireDate, 30);

            // Act
            var years = employee.GetYearsOfService();

            // Assert
            Assert.InRange(years, 4, 5);  // Allow some tolerance for days
        }

        // Normal case: Calculate bonus
        [Fact]
        public void CalculateAnnualBonus_ValidPercentage_ReturnsCorrectAmount()
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 100000, DateTime.Now, 30);
            var bonusPercent = 10m;
            var expectedBonus = 10000m;

            // Act
            var bonus = employee.CalculateAnnualBonus(bonusPercent);

            // Assert
            Assert.Equal(expectedBonus, bonus);
        }

        // Boundary case: Zero bonus
        [Fact]
        public void CalculateAnnualBonus_ZeroPercent_ReturnsZero()
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 100000, DateTime.Now, 30);

            // Act
            var bonus = employee.CalculateAnnualBonus(0m);

            // Assert
            Assert.Equal(0m, bonus);
        }

        // Boundary case: 100% bonus
        [Fact]
        public void CalculateAnnualBonus_HundredPercent_ReturnsSalary()
        {
            // Arrange
            var salary = 100000m;
            var employee = new Employee("E001", "John", "john@test.com", "IT", salary, DateTime.Now, 30);

            // Act
            var bonus = employee.CalculateAnnualBonus(100m);

            // Assert
            Assert.Equal(salary, bonus);
        }

        // Invalid input: Negative bonus percentage
        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void CalculateAnnualBonus_NegativePercent_ThrowsException(decimal invalidPercent)
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 100000, DateTime.Now, 30);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                employee.CalculateAnnualBonus(invalidPercent));
        }

        // Invalid input: Bonus percentage above 100
        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        public void CalculateAnnualBonus_PercentAbove100_ThrowsException(decimal invalidPercent)
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 100000, DateTime.Now, 30);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                employee.CalculateAnnualBonus(invalidPercent));
        }

        // Normal case: Deactivate employee
        [Fact]
        public void Deactivate_SetsIsActiveToFalse()
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, 30);
            Assert.True(employee.IsActive);  // Initially active

            // Act
            employee.Deactivate();

            // Assert
            Assert.False(employee.IsActive);
        }

        // Normal case: Activate employee
        [Fact]
        public void Activate_SetsIsActiveToTrue()
        {
            // Arrange
            var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, 30);
            employee.Deactivate();
            Assert.False(employee.IsActive);

            // Act
            employee.Activate();

            // Assert
            Assert.True(employee.IsActive);
        }

        // Normal case: ToString returns formatted string
        [Fact]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var employee = new Employee("E001", "John Doe", "john@test.com", "Engineering", 75000, DateTime.Now, 30);

            // Act
            var result = employee.ToString();

            // Assert
            Assert.Contains("E001", result);
            Assert.Contains("John Doe", result);
            Assert.Contains("Engineering", result);
            Assert.Contains("75,000", result);
            Assert.Contains("Active", result);
        }
    }
}
