# Day 07: Dependency Injection and Middleware

## Overview
This project demonstrates two central mechanisms in ASP.NET Core applications:
1. **Dependency Injection (DI)** - Service registration and lifetimes
2. **Middleware Pipeline** - Request processing and ordering
3. **Configuration Management** - Environment-specific settings
4. **Structured Logging** - ILogger with scopes

## Key Concepts

### Dependency Injection (DI)

ASP.NET Core has built-in DI container that manages service lifetimes and resolves dependencies automatically.

#### Service Lifetimes

| Lifetime | Description | Use Case | Example |
|----------|-------------|----------|---------|
| **Transient** | New instance every time | Lightweight, stateless operations | `IGuidGeneratorService` |
| **Scoped** | One instance per request | Request-specific work, repositories | `IEmployeeRepository`, `IEmployeeService` |
| **Singleton** | One instance for app lifetime | Shared state, caching | `IRequestCounterService` |

#### Registration in Program.cs

```csharp
// Transient
builder.Services.AddTransient<IGuidGeneratorService, GuidGeneratorService>();

// Scoped
builder.Services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Singleton
builder.Services.AddSingleton<IRequestCounterService, RequestCounterService>();
```

### Middleware Pipeline

Middleware components execute in a specific order. Each middleware can:
1. Process the request before calling the next middleware
2. Call the next middleware
3. Process the response after the next middleware returns

#### Middleware Pipeline Diagram

```
┌──────────────────────────────────────────────────────────────┐
│                      HTTP Request                             │
└───────────────────────────┬──────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  1. Swagger (Development Only)                                 │
│     - API Documentation UI                                     │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  2. HTTPS Redirection                                          │
│     - Redirects HTTP to HTTPS                                  │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  3. Request Counter (Custom Middleware)                        │
│     - Increments singleton counter                             │
│     - Adds X-Request-Count header                              │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  4. Request Logging (Custom Middleware)                        │
│     - Logs request method, path, timing                        │
│     - Logs response status code                                │
│     - Structured logging with scopes                           │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  5. Routing                                                    │
│     - Matches request to endpoint                              │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  6. Authorization                                              │
│     - Checks permissions (not yet implemented)                 │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌────────────────────────────────────────────────────────────────┐
│  7. Endpoints (Controllers)                                    │
│     - Controller action executes                               │
│     - Services injected via constructor                        │
└───────────────────────────┬────────────────────────────────────┘
                            │
                            ▼
┌──────────────────────────────────────────────────────────────┐
│                      HTTP Response                            │
└──────────────────────────────────────────────────────────────┘
```

**Why Order Matters:**
- **Request Counter must be early** to count all requests, even failed ones
- **Logging should be early** to capture complete request/response cycle
- **Routing must come before Authorization** to know which endpoint requires auth
- **Authorization must come before Endpoints** to protect actions
- Exceptions bubble up through the pipeline in reverse order

### Configuration Management

Environment-specific configuration using `appsettings.json` and `appsettings.{Environment}.json`:

```csharp
// Bind configuration to strongly-typed class
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.SectionName));

// Inject into services
public class MyService
{
    private readonly AppSettings _settings;

    public MyService(IOptions<AppSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

### Structured Logging with ILogger

```csharp
// Logger injection
public class EmployeeService
{
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(ILogger<EmployeeService> logger)
    {
        _logger = logger;
    }

    public async Task DoWork()
    {
        // Structured logging with named parameters
        _logger.LogInformation("Processing employee {EmployeeId}", employeeId);

        // Log scopes for related log entries
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["RequestId"] = requestId,
            ["UserId"] = userId
        }))
        {
            // All logs in this block include scope properties
            _logger.LogInformation("Started processing");
            _logger.LogInformation("Finished processing");
        }
    }
}
```

## Project Structure

```
Day07.DI.Middleware/
├── Configuration/
│   └── AppSettings.cs              # Configuration classes
├── Controllers/
│   ├── EmployeesController.cs      # Employee CRUD endpoints
│   └── DiagnosticsController.cs    # App diagnostics endpoints
├── Middleware/
│   ├── RequestLoggingMiddleware.cs # Custom logging middleware
│   └── RequestCounterMiddleware.cs # Custom counter middleware
├── Models/
│   └── Employee.cs                 # Domain model
├── Services/
│   ├── IEmployeeRepository.cs      # Repository interface (Scoped)
│   ├── InMemoryEmployeeRepository.cs
│   ├── IEmployeeService.cs         # Service interface (Scoped)
│   ├── EmployeeService.cs
│   ├── IGuidGeneratorService.cs    # Transient service interface
│   ├── GuidGeneratorService.cs
│   ├── IRequestCounterService.cs   # Singleton service interface
│   └── RequestCounterService.cs
├── Program.cs                      # DI and middleware configuration
├── appsettings.json                # Production configuration
└── appsettings.Development.json    # Development configuration

Day07.Tests/
├── EmployeeServiceTests.cs         # Service business logic tests
└── ServiceLifetimeTests.cs         # DI lifetime behavior tests
```

## Running the Application

```bash
cd Day07.DI.Middleware
dotnet restore
dotnet build
dotnet run
```

Access the API:
- Swagger UI: https://localhost:7000
- Diagnostics: https://localhost:7000/api/diagnostics/info
- Employees: https://localhost:7000/api/employees

## Running Tests

```bash
cd ../Day07.Tests
dotnet test
```

## API Endpoints

### Employee Management

- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `GET /api/employees/department/{department}` - Get employees by department
- `POST /api/employees` - Create new employee
- `PUT /api/employees/{id}` - Update employee
- `DELETE /api/employees/{id}` - Delete employee

### Diagnostics

- `GET /api/diagnostics/info` - Get application info, request count, uptime
- `GET /api/diagnostics/config` - Get current configuration (non-sensitive)

The diagnostics endpoint demonstrates:
- Singleton service state (request counter maintains state across requests)
- Configuration injection via IOptions
- Application-wide metrics

## Service Lifetime Examples

### Transient Example
```csharp
// IGuidGeneratorService - new instance every time
public EmployeesController(
    IGuidGeneratorService gen1,
    IGuidGeneratorService gen2)  // gen1 and gen2 are DIFFERENT instances
{
    // Each has different ServiceInstanceId
}
```

### Scoped Example
```csharp
// IEmployeeRepository - same instance within a request
public class EmployeeService
{
    private readonly IEmployeeRepository _repo;

    // Same repository instance used throughout this request
    public EmployeeService(IEmployeeRepository repo)
    {
        _repo = repo;
    }
}
```

### Singleton Example
```csharp
// IRequestCounterService - same instance across all requests
// State persists for entire application lifetime
public class RequestCounterService
{
    private int _count = 0;  // Shared across ALL requests

    public void Increment() => _count++;
    public int GetCount() => _count;
}
```

## Captive Dependency Problem

**What is it?**
A captive dependency occurs when a service with a longer lifetime (Singleton) depends on a service with a shorter lifetime (Scoped or Transient).

**Example of WRONG code:**
```csharp
// WRONG: Singleton capturing Scoped dependency
public class MySingletonService
{
    private readonly IEmployeeRepository _repo;  // Scoped!

    public MySingletonService(IEmployeeRepository repo)
    {
        _repo = repo;  // This repo instance is now "captured" for app lifetime
    }
}
```

**Problem:** The scoped repository is kept alive for the entire application lifetime instead of being recreated per request. This can lead to:
- Memory leaks
- Stale data
- Thread safety issues

**Solution:** Only inject services with equal or longer lifetimes:
- Singleton can depend on: Singleton
- Scoped can depend on: Singleton, Scoped
- Transient can depend on: Singleton, Scoped, Transient

## Interview Questions & Answers

### Why use DI?
1. **Loose coupling**: Components depend on abstractions (interfaces), not concrete implementations
2. **Testability**: Easy to substitute mock implementations in tests
3. **Maintainability**: Change implementations without modifying consumers
4. **Lifetime management**: Container manages object creation and disposal
5. **Configuration**: Change behavior via configuration without code changes

### What is a captive dependency?
A captive dependency is when a longer-lived service (Singleton) captures a shorter-lived service (Scoped/Transient) in its constructor. This prevents the short-lived service from being disposed properly and can cause:
- Memory leaks
- Stale data (e.g., using old DbContext)
- Thread safety issues

**Prevention:** Only inject services with equal or longer lifetimes.

### Why does middleware order matter?
1. **Request flows top-to-bottom** through middleware
2. **Response flows bottom-to-top** (each middleware can modify response)
3. **Early middleware affects all downstream middleware**
4. **Late middleware might never execute** if early middleware short-circuits

Examples:
- **Authentication before Authorization**: Must know WHO the user is before checking WHAT they can do
- **Logging early**: Captures all requests, even those that fail in later middleware
- **Exception handling first**: Catches all exceptions from downstream middleware

## Configuration Files

### appsettings.json (Production)
```json
{
  "AppSettings": {
    "ApplicationName": "Employee Management API",
    "Environment": "Production",
    "Features": {
      "EnableDetailedErrors": false,
      "EnableRequestLogging": true
    }
  }
}
```

### appsettings.Development.json (Development)
```json
{
  "AppSettings": {
    "ApplicationName": "Employee Management API (Dev)",
    "Environment": "Development",
    "Features": {
      "EnableDetailedErrors": true,
      "EnableRequestLogging": true
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

## Key Takeaways

1. **DI Container** manages service creation and disposal automatically
2. **Service Lifetimes** (Transient, Scoped, Singleton) control instance creation
3. **Constructor Injection** is the preferred DI pattern
4. **Middleware Pipeline** processes requests in order - order matters!
5. **Custom Middleware** can intercept requests and responses
6. **ILogger** provides structured, performant logging
7. **Configuration** can be environment-specific and strongly-typed

## Next Steps (Day 08)
- Layered architecture with separate projects
- Request validation with Data Annotations
- DTOs vs Entities separation
- Central exception handling
- AutoMapper or manual mapping
