# Day 10: API Security and Authentication - Implementation Guide

## Overview
Implement JWT-based authentication and role-based authorization to protect API endpoints.

## Implementation Steps

### 1. Add JWT Packages
```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.API
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

### 2. Add User Entity and Authentication Models

**Domain/Entities/User.cs**:
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";  // "Admin", "Manager", "User"
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

**Application/DTOs/AuthDtos.cs**:
```csharp
public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
```

### 3. Create Token Service

**Application/Services/TokenService.cs**:
```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public interface ITokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
```

### 4. Configure JWT Authentication

**Program.cs**:
```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Admin", "Manager"));
});

// Register services
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Middleware order matters!
app.UseAuthentication();  // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();
```

### 5. Add Configuration

**appsettings.json**:
```json
{
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-chars-long-for-security",
    "Issuer": "EmployeeManagementAPI",
    "Audience": "EmployeeManagementAPI",
    "ExpirationHours": 8
  }
}
```

**appsettings.Development.json** (for local dev):
```json
{
  "Jwt": {
    "Secret": "development-secret-key-change-in-production-min-32-chars"
  }
}
```

### 6. Create Authentication Controller

**API/Controllers/AuthController.cs**:
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Login and get JWT token
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Account is deactivated" });
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        });
    }

    /// <summary>
    /// Register new user (Admin only)
    /// </summary>
    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Username already exists" });
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            Role = "User"
        };

        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetCurrentUser), new { id = user.Id }, user);
    }

    /// <summary>
    /// Get current authenticated user
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetByIdAsync(int.Parse(userId));
        return Ok(user);
    }

    private static string HashPassword(string password)
    {
        // Use BCrypt, Argon2, or ASP.NET Core Identity for production
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
```

### 7. Protect Employee Endpoints

**API/Controllers/EmployeesController.cs**:
```csharp
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints require authentication
public class EmployeesController : ControllerBase
{
    // GET endpoints - any authenticated user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        // ...
    }

    // POST - Manager or Admin only
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] EmployeeCreateDto createDto)
    {
        // ...
    }

    // PUT - Manager or Admin only
    [HttpPut("{id}")]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] EmployeeUpdateDto updateDto)
    {
        // ...
    }

    // DELETE - Admin only
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        // ...
    }
}
```

## Testing with JWT

### Using .http file:
```http
### Login
POST https://localhost:7000/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}

### Save token and use in subsequent requests
@token = <paste-token-here>

### Get all employees (authenticated)
GET https://localhost:7000/api/employees
Authorization: Bearer {{token}}

### Create employee (401 - Unauthenticated)
POST https://localhost:7000/api/employees
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "User"
}

### Create employee (403 - Forbidden, if not Admin/Manager)
POST https://localhost:7000/api/employees
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "User",
  "email": "test@company.com",
  "departmentId": 1,
  "position": "Developer",
  "salary": 75000
}
```

## Key Concepts

### Authentication vs Authorization
- **Authentication**: Verifying WHO you are (login, JWT token)
- **Authorization**: Checking WHAT you can do (roles, policies)

### 401 vs 403
- **401 Unauthorized**: No token or invalid token (not authenticated)
- **403 Forbidden**: Valid token but insufficient permissions (authenticated but not authorized)

### Claims
- Key-value pairs stored in JWT token
- Used for authorization decisions
- Examples: UserId, Username, Email, Role

### Roles vs Policies
- **Roles**: Simple role names ("Admin", "User")
- **Policies**: Complex authorization rules (can combine multiple requirements)

## Security Best Practices

1. **Never commit secrets** - Use User Secrets or Environment Variables
```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Secret" "your-secret-key"
```

2. **Use HTTPS** in production
3. **Strong secrets** - Minimum 32 characters for JWT secret
4. **Short token expiration** - 8 hours or less
5. **Hash passwords** - Use BCrypt, Argon2, or ASP.NET Core Identity
6. **Validate all inputs** - Even for authenticated users
7. **Rate limiting** - Prevent brute force attacks

## Interview Questions

**Authentication vs Authorization?**
- Authentication: WHO are you? (identity)
- Authorization: WHAT can you do? (permissions)

**What is a claim?**
- A piece of information about the user (name, email, role)
- Stored in JWT token
- Used for authorization decisions

**401 vs 403?**
- 401: Not authenticated (no token or invalid token)
- 403: Authenticated but not authorized (insufficient permissions)

## Configuration for Production

**Use Environment Variables**:
```bash
export Jwt__Secret="production-secret-32-chars-min"
export ConnectionStrings__DefaultConnection="..."
```

**Or Azure Key Vault**:
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```
