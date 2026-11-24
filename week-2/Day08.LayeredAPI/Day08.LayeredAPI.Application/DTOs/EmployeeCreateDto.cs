using System.ComponentModel.DataAnnotations;

namespace Day08.LayeredAPI.Application.DTOs;

/// <summary>
/// DTO for creating new employees
/// Includes validation attributes for declarative validation
/// </summary>
public class EmployeeCreateDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Valid department must be selected")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Position is required")]
    [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
    public string Position { get; set; } = string.Empty;

    [Range(0, 10000000, ErrorMessage = "Salary must be between 0 and 10,000,000")]
    public decimal Salary { get; set; }

    public DateTime? HireDate { get; set; }
}
