using System.ComponentModel.DataAnnotations;

namespace Day08.LayeredAPI.Application.DTOs;

public class EmployeeUpdateDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Position { get; set; } = string.Empty;

    [Range(0, 10000000)]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }
}
