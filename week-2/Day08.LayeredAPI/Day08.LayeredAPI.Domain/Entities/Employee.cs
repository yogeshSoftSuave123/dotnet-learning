namespace Day08.LayeredAPI.Domain.Entities;

/// <summary>
/// Employee entity - Domain model
/// Contains business logic and domain rules
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public string Position { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation property
    public Department? Department { get; set; }

    // Domain methods
    public string GetFullName() => $"{FirstName} {LastName}";

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateSalary(decimal newSalary)
    {
        if (newSalary < 0)
            throw new ArgumentException("Salary cannot be negative", nameof(newSalary));

        Salary = newSalary;
    }
}
