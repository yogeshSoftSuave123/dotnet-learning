using Microsoft.AspNetCore.Mvc;
using Day08.LayeredAPI.Application.DTOs;
using Day08.LayeredAPI.Application.Interfaces;

namespace Day08.LayeredAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(request);
        if (response == null)
            return Unauthorized(new { message = "Invalid username or password" });

        _logger.LogInformation("User logged in successfully: {Username}", request.Username);
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user and returns a JWT token
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if username exists
        if (await _authService.UsernameExistsAsync(request.Username))
            return BadRequest(new { message = "Username already exists" });

        // Check if email exists
        if (await _authService.EmailExistsAsync(request.Email))
            return BadRequest(new { message = "Email already exists" });

        var response = await _authService.RegisterAsync(request);
        if (response == null)
            return BadRequest(new { message = "Registration failed" });

        _logger.LogInformation("User registered successfully: {Username}", request.Username);
        return CreatedAtAction(nameof(Register), new { username = response.Username }, response);
    }
}
