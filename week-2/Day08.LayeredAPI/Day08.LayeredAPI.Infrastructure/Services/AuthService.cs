using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Day08.LayeredAPI.Application.DTOs;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;

namespace Day08.LayeredAPI.Infrastructure.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        // Find user by username
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
            return null;
        }

        // Check if user is active
        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: User is inactive - {Username}", request.Username);
            return null;
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password - {Username}", request.Username);
            return null;
        }

        // Update last login
        user.RecordLogin();
        await _userRepository.UpdateAsync(user);

        // Generate JWT token
        var token = _tokenService.GenerateToken(user);
        var expirationMinutes = _tokenService.GetTokenExpirationMinutes();

        _logger.LogInformation("Login successful for user: {Username}", request.Username);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        _logger.LogInformation("Registration attempt for username: {Username}", request.Username);

        // Check if username already exists
        if (await _userRepository.UsernameExistsAsync(request.Username))
        {
            _logger.LogWarning("Registration failed: Username already exists - {Username}", request.Username);
            return null;
        }

        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            _logger.LogWarning("Registration failed: Email already exists - {Email}", request.Email);
            return null;
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create new user
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = request.Role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        // Generate JWT token
        var token = _tokenService.GenerateToken(user);
        var expirationMinutes = _tokenService.GetTokenExpirationMinutes();

        _logger.LogInformation("Registration successful for user: {Username}", request.Username);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userRepository.UsernameExistsAsync(username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.EmailExistsAsync(email);
    }
}
