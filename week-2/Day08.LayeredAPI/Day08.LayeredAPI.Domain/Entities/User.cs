namespace Day08.LayeredAPI.Domain.Entities;

/// <summary>
/// User entity for authentication and authorization
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // Admin, Manager, User
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public bool IsAdmin() => Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    public bool IsManager() => Role.Equals("Manager", StringComparison.OrdinalIgnoreCase);
}
