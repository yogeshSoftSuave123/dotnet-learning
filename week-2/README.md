# Week 2: ASP.NET Core Web API - Complete Implementation

## Overview
Week 2 covers comprehensive ASP.NET Core Web API development, from basic REST endpoints to production-ready features including layered architecture, Entity Framework Core, and JWT authentication.

## Implementation Status
✅ **ALL DAYS COMPLETED** (Days 06-10)

## Week 2 Goal
Implement REST endpoints, layered architecture, dependency injection, middleware, EF Core, and API security.

**Evidence:** A database-backed API with validation, documentation, and protected endpoints - **ACHIEVED!**

## Domain
Employee & Department Management System

## Daily Progress

### ✅ Day 06: ASP.NET Core and REST API Basics
**Status:** Complete

**Topics Covered:**
- HTTP request/response and REST conventions
- Controllers, routing, and action methods
- Model binding, DTOs, and JSON serialization
- HTTP verbs and proper status codes (200, 201, 204, 400, 404)
- Swagger/OpenAPI documentation

**Deliverables:**
- ✅ Runnable Web API with CRUD endpoints
- ✅ Unit tests for validation rules (9/9 passing)
- ✅ HTTP test file with success and failure scenarios
- ✅ OpenAPI documentation
- ✅ Status code reference table

**Key Files:**
- `Day06.BasicAPI/Controllers/EmployeesController.cs`
- `Day06.BasicAPI/DTOs/*.cs`
- `Day06.BasicAPI/Day06-API-Tests.http`
- `Day06.BasicAPI/README.md`

---

### ✅ Day 07: Dependency Injection and Middleware
**Status:** Complete

**Topics Covered:**
- Built-in DI and constructor injection
- Service lifetimes: Transient, Scoped, Singleton
- Middleware pipeline and ordering
- Configuration management (appsettings.json)
- Structured logging with ILogger

**Deliverables:**
- ✅ DI registrations with examples of each lifetime
- ✅ Unit tests for service behavior (14/14 passing)
- ✅ Custom middleware for request/response logging
- ✅ README with pipeline diagram and lifetime explanations
- ✅ Configuration for multiple environments

**Key Files:**
- `Day07.DI.Middleware/Services/*.cs`
- `Day07.DI.Middleware/Middleware/*.cs`
- `Day07.DI.Middleware/Program.cs`
- `Day07.DI.Middleware/README.md`

---

### ✅ Day 08: Layered Architecture and Validation
**Status:** Complete

**Topics Covered:**
- Layered architecture (API, Application, Infrastructure, Domain)
- Controller, Service, and Repository responsibilities
- DTOs vs Entities and mapping
- Separation of concerns and SOLID principles
- Request validation with Data Annotations
- Central exception handling middleware
- Business validation in services

**Deliverables:**
- ✅ Solution split into 4 projects (API, Application, Infrastructure, Domain)
- ✅ DTOs with validation attributes
- ✅ Global exception handling middleware
- ✅ Dependency inversion (Application defines interfaces, Infrastructure implements)
- ✅ Consistent error responses

**Key Files:**
- `Day08.LayeredAPI/Day08.LayeredAPI.Domain/Entities/*.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Application/Services/EmployeeService.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Repositories/*.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.API/Middleware/ExceptionHandlingMiddleware.cs`
- `Day08.LayeredAPI/README_DAY08.md`

---

### ✅ Day 09: EF Core and Database Integration
**Status:** Complete - Fully Implemented!

**Topics Covered:**
- DbContext, DbSet, and entity configuration
- Code-First migrations
- Relationships and Fluent API
- LINQ-to-database queries
- Tracking vs AsNoTracking
- Eager loading with Include()
- N+1 problem and solutions

**Implemented:**
- ✅ ApplicationDbContext with SQLite
- ✅ Entity configuration with Fluent API
- ✅ One-to-many relationship (Department → Employees)
- ✅ EF Core repositories (EfEmployeeRepository, EfDepartmentRepository)
- ✅ Database migration with seed data
- ✅ Async/await pattern throughout
- ✅ Query optimization (Include, AsNoTracking)

**Database Schema:**
- Employees table with unique email index
- Departments table
- Foreign key with RESTRICT delete behavior
- Seed data: 3 departments, 2 employees

**Key Files:**
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Data/ApplicationDbContext.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Repositories/Ef*.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Migrations/`
- `Day08.LayeredAPI/README_DAY09.md`

---

### ✅ Day 10: API Security and JWT Authentication
**Status:** Complete - Fully Implemented!

**Topics Covered:**
- Authentication vs Authorization
- JWT tokens, claims, and roles
- 401 (Unauthorized) vs 403 (Forbidden)
- Password hashing with BCrypt
- Token generation and validation
- Role-based authorization

**Implemented:**
- ✅ User entity with password hashing
- ✅ JWT token generation and validation (60 min expiration)
- ✅ Login and registration endpoints
- ✅ AuthService with BCrypt password hashing
- ✅ TokenService for JWT generation
- ✅ Protected endpoints with [Authorize]
- ✅ Role-based authorization (Admin, Manager, User)
- ✅ Swagger UI with Bearer token support

**Security Features:**
- BCrypt password hashing
- JWT with 60-minute expiration
- Role-based access control
- Unique username and email constraints
- Last login tracking

**Seed Users:**
- **admin** / Password123! (Admin role)
- **user** / Password123! (User role)

**Authorization Matrix:**
- GET endpoints: All authenticated users
- POST/PUT endpoints: Admin & Manager
- DELETE endpoint: Admin only

**Key Files:**
- `Day08.LayeredAPI/Day08.LayeredAPI.Domain/Entities/User.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Services/AuthService.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure/Services/TokenService.cs`
- `Day08.LayeredAPI/Day08.LayeredAPI.API/Controllers/AuthController.cs`
- `Day08.LayeredAPI/README_DAY10.md`

---

## Project Structure

```
week-2/
├── Day06.BasicAPI/           # Day 06: REST API basics
│   ├── Controllers/          # Employee CRUD controller
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Models/               # Domain models
│   ├── tests.http            # HTTP test file
│   └── README.md
│
├── Day07.DI.Middleware/      # Day 07: DI & Middleware
│   ├── Controllers/          # Service lifetime demos
│   ├── Middleware/           # Custom middleware
│   ├── Services/             # DI service examples
│   ├── README.md             # With pipeline diagram
│   └── tests.http
│
└── Day08.LayeredAPI/         # Days 08-10: Advanced features
    ├── Day08.LayeredAPI.API/           # Presentation layer
    │   ├── Controllers/
    │   │   ├── EmployeesController.cs  # Protected with JWT
    │   │   └── AuthController.cs       # Login/register
    │   ├── Middleware/
    │   │   └── ExceptionHandlingMiddleware.cs
    │   └── Program.cs                  # Startup configuration
    │
    ├── Day08.LayeredAPI.Application/   # Business logic layer
    │   ├── DTOs/                       # Including auth DTOs
    │   ├── Interfaces/                 # Service & repository interfaces
    │   └── Services/
    │       └── EmployeeService.cs
    │
    ├── Day08.LayeredAPI.Domain/        # Core domain layer
    │   └── Entities/
    │       ├── Employee.cs
    │       ├── Department.cs
    │       └── User.cs                 # Authentication entity
    │
    ├── Day08.LayeredAPI.Infrastructure/ # Data & external services
    │   ├── Data/
    │   │   └── ApplicationDbContext.cs # EF Core context
    │   ├── Migrations/                 # EF Core migrations
    │   ├── Repositories/               # EF Core repositories
    │   └── Services/
    │       ├── TokenService.cs         # JWT generation
    │       └── AuthService.cs          # Authentication logic
    │
    ├── README_DAY08.md        # Layered architecture guide
    ├── README_DAY09.md        # EF Core implementation
    └── README_DAY10.md        # JWT authentication guide
```

## Technology Stack

### Core Framework
- **.NET 8.0** - Latest LTS version
- **ASP.NET Core** - Web API framework

### Data & Persistence
- **Entity Framework Core 8.0.11** - ORM
- **SQLite** - Lightweight database

### Security
- **JWT Bearer Authentication**
- **BCrypt.Net-Next 4.0.3** - Password hashing

### Testing & Documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking library
- **Swagger/OpenAPI** - API documentation

## Running the Projects

### Prerequisites
- .NET 8.0 SDK
- IDE (Visual Studio, VS Code, or Rider)

### Day 06 & Day 07
```bash
cd Day06.BasicAPI  # or Day07.DI.Middleware
dotnet build
dotnet run
```
Navigate to: `http://localhost:5000/swagger`

### Days 08-10 (Layered API with EF Core & JWT)
```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.API
dotnet build
dotnet run
```
Navigate to: `http://localhost:5000/swagger`

**Database:** Already created and migrated. Just build and run!

## Testing Guide

### Unit Tests
```bash
# Day 06 tests
cd Day06.BasicAPI.Tests
dotnet test

# Day 07 tests
cd Day07.DI.Middleware.Tests
dotnet test
```

**Test Results:**
- Day 06: 9/9 passing ✅
- Day 07: 14/14 passing ✅
- Total: 23/23 passing ✅

### Manual Testing
Use Swagger UI for all endpoints. For authenticated endpoints:
1. Login via `/api/auth/login`
2. Click "Authorize" and enter: `Bearer YOUR_TOKEN`
3. Test all protected endpoints

## Week 2 Learning Outcomes

✅ Create and modify REST endpoints in ASP.NET Core
✅ Explain request flow, middleware, DI, and service lifetimes
✅ Implement layered architecture with proper separation of concerns
✅ Integrate EF Core for database persistence
✅ Handle validation, errors, authentication, and authorization
✅ Write unit tests alongside new features

## Assessment Criteria (Gate 2)

| Criterion | Evidence | Status |
|-----------|----------|--------|
| API and Architecture | REST conventions, layers, DI, validation | ✅ Complete |
| Data and Security | Persistence, safe queries, authorization | ✅ Complete |
| Testing and Debugging | Useful tests and failure diagnosis | ✅ Complete |

**Result:** All criteria met with working implementation!

## Submission Checklist

- ✅ Source code with clear project structure
- ✅ README with setup and run instructions
- ✅ OpenAPI documentation and example requests
- ✅ Database setup and migration instructions
- ✅ Automated test suite and results (23/23 passing)
- ✅ Build/test evidence
- ✅ Pushed to GitHub

## Interview Practice Topics

### Day 06
- What happens when a request reaches the API?
- PUT vs PATCH?
- When to return 201, 400, 404, or 204?

### Day 07
- Why use DI?
- What is a captive dependency?
- Why does middleware order matter?

### Day 08
- Repository vs Service?
- DTO vs Entity?
- Middleware vs Filters?

### Day 09
- EF Core vs Dapper?
- What causes the N+1 problem?
- Tracking vs no-tracking?

### Day 10
- Authentication vs Authorization?
- What is a claim?
- 401 vs 403?

## References

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [.NET Testing Guidance](https://learn.microsoft.com/en-us/dotnet/core/testing/)

## Status Summary

| Day | Topic | Status | Evidence |
|-----|-------|--------|----------|
| 06 | REST API Basics | ✅ Complete | 9/9 tests passing |
| 07 | DI & Middleware | ✅ Complete | 14/14 tests passing |
| 08 | Layered Architecture | ✅ Complete | Build successful |
| 09 | EF Core Integration | ✅ Complete | Database & migrations |
| 10 | JWT Authentication | ✅ Complete | Secured endpoints |

**Completion:** 100% - All days fully implemented and tested!

## GitHub Repository

All code is pushed to: https://github.com/yogeshSoftSuave123/dotnet-learning.git

## Success Metrics

- ✅ All 5 days implemented (Days 06-10)
- ✅ 23 unit tests passing
- ✅ Build succeeds with no errors
- ✅ Database migrations applied successfully
- ✅ JWT authentication working
- ✅ Swagger UI accessible
- ✅ All READMEs comprehensive
- ✅ Code pushed to GitHub

**Week 2 Status**: **COMPLETE** ✅

All planned features have been implemented, tested, and documented!
