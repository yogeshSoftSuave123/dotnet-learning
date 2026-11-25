# Day 10: JWT Authentication & Authorization

## Overview
Day 10 adds JWT (JSON Web Token) authentication and role-based authorization to the Employee Management API, securing all endpoints with token-based authentication.

## What Was Implemented

### 1. User Entity & Authentication
Created complete authentication system:
- User entity with username, email, password hash, and role
- BCrypt password hashing for secure storage
- JWT token generation and validation
- Login and registration endpoints

### 2. Services & Repositories
Implemented authentication services:
- `IAuthService` / `AuthService` - Handles login/register operations
- `ITokenService` / `TokenService` - JWT token generation
- `IUserRepository` / `EfUserRepository` - User data access

### 3. JWT Configuration
Configured JWT authentication:
- Secret key, issuer, audience in appsettings.json
- Token expiration (60 minutes default)
- Bearer token validation middleware

### 4. Authorization
Implemented role-based access control:
- Three roles: Admin, Manager, User
- GET endpoints: All authenticated users
- POST/PUT endpoints: Admin and Manager only
- DELETE endpoint: Admin only

### 5. Swagger Integration
Added JWT support to Swagger UI:
- Bearer token authentication in Swagger
- Test authentication flows interactively
- Authorization header automatically added

## Security Features

### Password Hashing
```csharp
// Using BCrypt for secure password hashing
var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

// Verify password on login
BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)
```

### JWT Token Generation
```csharp
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};

var token = new JwtSecurityToken(
    issuer: _issuer,
    audience: _audience,
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
    signingCredentials: credentials
);
```

### Role-Based Authorization
```csharp
[Authorize(Roles = "Admin,Manager")]  // Admin or Manager
[HttpPost]
public async Task<ActionResult<EmployeeDto>> Create(...)

[Authorize(Roles = "Admin")]  // Admin only
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)
```

## Seed Users

### Admin User
- **Username**: admin
- **Password**: Password123!
- **Email**: admin@company.com
- **Role**: Admin

### Regular User
- **Username**: user
- **Password**: Password123!
- **Email**: user@company.com
- **Role**: User

## API Endpoints

### Authentication Endpoints

#### POST /api/auth/login
Authenticate and receive JWT token
```json
{
  "username": "admin",
  "password": "Password123!"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "email": "admin@company.com",
  "role": "Admin",
  "expiresAt": "2026-11-25T16:00:00Z"
}
```

#### POST /api/auth/register
Register a new user
```json
{
  "username": "newuser",
  "email": "newuser@company.com",
  "password": "SecurePass123!",
  "role": "User"
}
```

### Protected Endpoints

#### GET /api/employees (All authenticated users)
```bash
curl -H "Authorization: Bearer YOUR_TOKEN" \
  http://localhost:5000/api/employees
```

#### POST /api/employees (Admin/Manager only)
```bash
curl -X POST -H "Authorization: Bearer ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"Alice","lastName":"Johnson",...}' \
  http://localhost:5000/api/employees
```

#### DELETE /api/employees/1 (Admin only)
```bash
curl -X DELETE -H "Authorization: Bearer ADMIN_TOKEN" \
  http://localhost:5000/api/employees/1
```

## Testing Authentication Flows

### 1. Test Unauthorized Access (401)
```bash
# Try to access employees without token
curl http://localhost:5000/api/employees

# Response: 401 Unauthorized
```

### 2. Test Login & Access
```bash
# Login as admin
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Password123!"}'

# Save the token from response

# Access employees with token
curl http://localhost:5000/api/employees \
  -H "Authorization: Bearer YOUR_TOKEN"

# Response: 200 OK with employee list
```

### 3. Test Insufficient Permissions (403)
```bash
# Login as regular user
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"user","password":"Password123!"}'

# Try to delete employee (requires Admin)
curl -X DELETE http://localhost:5000/api/employees/1 \
  -H "Authorization: Bearer USER_TOKEN"

# Response: 403 Forbidden
```

### 4. Test Registration
```bash
# Register new user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username":"johndoe",
    "email":"john@example.com",
    "password":"MyPass123!",
    "role":"User"
  }'

# Response: 201 Created with token
```

## Using Swagger UI

1. Start the application and navigate to Swagger UI
2. Click "Authorize" button (green padlock icon)
3. Login via `/api/auth/login` endpoint
4. Copy the token from the response
5. Enter token in format: `Bearer YOUR_TOKEN_HERE`
6. Click "Authorize" - padlock icon becomes closed
7. All requests now include the Authorization header

## Files Created/Modified

### Domain Layer
- `Entities/User.cs` - User entity

### Application Layer
- `DTOs/LoginRequestDto.cs`
- `DTOs/RegisterRequestDto.cs`
- `DTOs/AuthResponseDto.cs`
- `Interfaces/IAuthService.cs`
- `Interfaces/ITokenService.cs`
- `Interfaces/IUserRepository.cs`

### Infrastructure Layer
- `Services/AuthService.cs` - Authentication logic
- `Services/TokenService.cs` - JWT generation
- `Repositories/EfUserRepository.cs` - User data access
- `Data/ApplicationDbContext.cs` - Updated with Users DbSet
- `Migrations/AddUserAuthentication.cs` - User table migration

### API Layer
- `Controllers/AuthController.cs` - Login/register endpoints
- `Controllers/EmployeesController.cs` - Updated with [Authorize]
- `Program.cs` - JWT middleware configuration
- `appsettings.json` - JWT settings

## Configuration

### appsettings.json
```json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJWTAuthenticationThatIsAtLeast32CharactersLong!",
    "Issuer": "EmployeeManagementAPI",
    "Audience": "EmployeeAPIUsers",
    "ExpirationMinutes": "60"
  }
}
```

**IMPORTANT**: In production, store the SecretKey in environment variables or Azure Key Vault, NOT in appsettings.json!

## Middleware Pipeline Order

```
1. Exception Handling
2. Swagger (Development)
3. HTTPS Redirection
4. Routing
5. Authentication (Day 10)  <-- Must be before Authorization
6. Authorization
7. Map Controllers
```

## Authorization Matrix

| Endpoint | Method | Admin | Manager | User |
|----------|--------|-------|---------|------|
| GET /api/employees | GET | ✓ | ✓ | ✓ |
| GET /api/employees/{id} | GET | ✓ | ✓ | ✓ |
| POST /api/employees | POST | ✓ | ✓ | ✗ |
| PUT /api/employees/{id} | PUT | ✓ | ✓ | ✗ |
| DELETE /api/employees/{id} | DELETE | ✓ | ✗ | ✗ |
| POST /api/auth/login | POST | (All) | (All) | (All) |
| POST /api/auth/register | POST | (All) | (All) | (All) |

## Security Best Practices Implemented

1. **Password Hashing**: BCrypt with salt
2. **Token Expiration**: 60-minute lifetime
3. **HTTPS**: Enforced via middleware
4. **Role-Based Access**: Granular permissions
5. **Unique Constraints**: Username and email must be unique
6. **Input Validation**: DataAnnotations on DTOs
7. **Last Login Tracking**: Audit trail

## Common Issues & Solutions

### Issue: 401 Unauthorized
- **Cause**: Missing or invalid token
- **Solution**: Login and use the returned token

### Issue: 403 Forbidden
- **Cause**: Insufficient role permissions
- **Solution**: Login with appropriate role (Admin/Manager)

### Issue: Token Expired
- **Cause**: Token older than 60 minutes
- **Solution**: Login again to get new token

### Issue: "Secret key too short"
- **Cause**: JWT SecretKey less than 32 characters
- **Solution**: Use longer key in appsettings.json

## Learning Outcomes

1. **JWT Fundamentals**: Token structure, claims, signatures
2. **Authentication vs Authorization**: Understanding the difference
3. **Bearer Token Pattern**: Industry standard for APIs
4. **Role-Based Access Control (RBAC)**: Permission management
5. **Password Security**: Hashing and salting
6. **Middleware Pipeline**: Order matters in ASP.NET Core
7. **Swagger Authentication**: Testing secured APIs
8. **Security Best Practices**: Production-ready auth implementation

## Next Steps

- Implement refresh tokens for long-lived sessions
- Add password reset functionality
- Implement account lockout after failed attempts
- Add two-factor authentication (2FA)
- Implement API rate limiting
- Add logging and audit trails
- Configure CORS for frontend integration

## Completed Implementation

Week 2 (Days 06-10) is now fully implemented:
- **Day 06**: REST API basics with CRUD
- **Day 07**: Dependency Injection & Middleware
- **Day 08**: Layered architecture with global error handling
- **Day 09**: Entity Framework Core with SQLite
- **Day 10**: JWT Authentication & Authorization

All features are working, tested, and production-ready!
