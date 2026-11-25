using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Application.Interfaces;

/// <summary>
/// Service for JWT token generation and validation
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token for the given user
    /// </summary>
    string GenerateToken(User user);

    /// <summary>
    /// Gets the token expiration time in minutes
    /// </summary>
    int GetTokenExpirationMinutes();
}
