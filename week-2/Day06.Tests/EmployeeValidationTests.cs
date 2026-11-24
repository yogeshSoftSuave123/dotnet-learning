using System.ComponentModel.DataAnnotations;
using Day06.BasicAPI.DTOs;

namespace Day06.Tests;

public class EmployeeValidationTests
{
    [Fact]
    public void EmployeeCreateDto_ValidData_PassesValidation()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@company.com",
            PhoneNumber = "+1234567890",
            Department = "Engineering",
            Position = "Developer",
            Salary = 75000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void EmployeeCreateDto_MissingFirstName_FailsValidation()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john.doe@company.com",
            Department = "Engineering",
            Position = "Developer",
            Salary = 75000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("FirstName"));
    }

    [Fact]
    public void EmployeeCreateDto_InvalidEmail_FailsValidation()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            Department = "Engineering",
            Position = "Developer",
            Salary = 75000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Email"));
    }

    [Fact]
    public void EmployeeCreateDto_NegativeSalary_FailsValidation()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@company.com",
            Department = "Engineering",
            Position = "Developer",
            Salary = -1000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("Salary"));
    }

    [Fact]
    public void EmployeeCreateDto_FirstNameTooShort_FailsValidation()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "J",
            LastName = "Doe",
            Email = "john.doe@company.com",
            Department = "Engineering",
            Position = "Developer",
            Salary = 75000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("FirstName"));
    }

    [Theory]
    [InlineData("john@company.com")]
    [InlineData("john.doe@company.co.uk")]
    [InlineData("john+test@company.com")]
    public void EmployeeCreateDto_ValidEmails_PassValidation(string email)
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Department = "Engineering",
            Position = "Developer",
            Salary = 75000
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.Empty(validationResults);
    }

    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}
