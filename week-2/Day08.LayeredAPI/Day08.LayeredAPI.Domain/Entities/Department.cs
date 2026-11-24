namespace Day08.LayeredAPI.Domain.Entities;

/// <summary>
/// Department entity - Domain model
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    // Domain methods
    public int GetEmployeeCount() => Employees?.Count ?? 0;
}
