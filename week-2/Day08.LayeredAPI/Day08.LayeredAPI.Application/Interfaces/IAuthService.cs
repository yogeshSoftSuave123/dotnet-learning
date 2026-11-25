using Day08.LayeredAPI.Application.DTOs;

namespace Day08.LayeredAPI.Application.Interfaces;

/// <summary>
/// Service for authentication operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with username and password
    /// </summary>
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);

    /// <summary>
    /// Registers a new user
    /// </summary>
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request);

    /// <summary>
    /// Checks if a username already exists
    /// </summary>
    Task<bool> UsernameExistsAsync(string username);

    /// <summary>
    /// Checks if an email already exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);
}
