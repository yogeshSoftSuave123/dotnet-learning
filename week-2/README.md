# Week 2: ASP.NET Core Web API Development

## Overview
Week 2 focuses on building a production-ready ASP.NET Core Web API with database persistence, validation, security, and proper architecture.

## Week 2 Goal
Implement REST endpoints, layered architecture, dependency injection, middleware, EF Core, and API security basics.

**Evidence:** A database-backed API with validation, documentation, and protected endpoints.

## Domain
Employee & Department Management System (continuing from Week 1)

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
- ✅ Unit tests for validation rules
- ✅ HTTP test file with success and failure scenarios
- ✅ OpenAPI documentation
- ✅ Status code reference table

**Key Files:**
- `Day06.BasicAPI/Controllers/EmployeesController.cs`
- `Day06.BasicAPI/DTOs/*.cs`
- `Day06.BasicAPI/Day06-API-Tests.http`
- `Day06.BasicAPI/README.md`

**Tests:** 9/9 passing ✓

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
- ✅ Unit tests for service behavior with mocked dependencies
- ✅ Custom middleware for request/response logging
- ✅ README with pipeline diagram and lifetime explanations
- ✅ Configuration for multiple environments

**Key Files:**
- `Day07.DI.Middleware/Services/*.cs`
- `Day07.DI.Middleware/Middleware/*.cs`
- `Day07.DI.Middleware/Program.cs`
- `Day07.DI.Middleware/README.md`

**Tests:** 14/14 passing ✓

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
- `Day08.LayeredAPI/README.md`

**Build:** Successful ✓

---

### 📘 Day 09: EF Core and Database Integration
**Status:** Implementation Guide Provided

**Topics:**
- DbContext, DbSet, and entity configuration
- Relationships and migrations
- LINQ-to-database queries
- Tracking vs AsNoTracking
- Eager loading with Include()
- Filtering, sorting, and pagination
- N+1 problem and solutions

**Implementation Guide:**
- Complete guide available in `DAY09_IMPLEMENTATION_GUIDE.md`
- Includes EF Core setup, DbContext configuration
- Migration commands and best practices
- Repository implementations with EF Core
- Filtering, sorting, and pagination patterns
- Interview questions and answers

**Concepts Covered:**
- Creating and applying migrations
- Entity configuration with Fluent API
- Query optimization strategies
- Connection string management

---

### 📘 Day 10: API Security and Checkpoint (Gate 2)
**Status:** Implementation Guide Provided

**Topics:**
- Authentication vs Authorization
- JWT tokens, claims, and roles/policies
- 401 (Unauthorized) vs 403 (Forbidden)
- Secrets and secure configuration
- Token generation and validation
- Role-based and policy-based authorization

**Implementation Guide:**
- Complete guide available in `DAY10_IMPLEMENTATION_GUIDE.md`
- JWT authentication setup
- Token service implementation
- Protected endpoints with [Authorize]
- Role-based authorization examples
- Security best practices
- Testing authenticated endpoints
- Interview questions and answers

**Concepts Covered:**
- JWT token structure and validation
- Claims-based authorization
- Password hashing
- Secure configuration management
- Testing authentication flows

---

## Project Structure

```
week-2/
├── Day06.BasicAPI/           # Day 6: REST API basics
│   ├── Controllers/
│   ├── DTOs/
│   ├── Models/
│   └── README.md
├── Day06.Tests/              # Day 6 tests
├── Day07.DI.Middleware/      # Day 7: DI and middleware
│   ├── Controllers/
│   ├── Services/
│   ├── Middleware/
│   ├── Configuration/
│   └── README.md
├── Day07.Tests/              # Day 7 tests
├── Day08.LayeredAPI/         # Day 8: Layered architecture (TBD)
├── Day09.EFCore/             # Day 9: Database integration (TBD)
├── Day10.Security/           # Day 10: API security (TBD)
└── Week2Assessment.sln       # Main solution
```

## Running the Projects

### Prerequisites
- .NET 8.0 SDK
- IDE (Visual Studio, VS Code, or Rider)
- SQL Server or SQLite (for Day 09)
- Postman or REST Client extension (for testing)

### Build All Projects
```bash
cd week-2
dotnet restore
dotnet build
```

### Run Specific Day
```bash
# Day 06
cd Day06.BasicAPI
dotnet run

# Day 07
cd Day07.DI.Middleware
dotnet run
```

### Run All Tests
```bash
dotnet test
```

## Week 2 Learning Outcomes

By the end of Week 2, you will be able to:

✅ Create and modify REST endpoints in ASP.NET Core
✅ Explain request flow, middleware, DI, and service lifetimes
✅ Implement layered architecture with proper separation of concerns
📘 Integrate EF Core for database persistence (guide provided)
📘 Handle validation, errors, authentication, and authorization (guide provided)
✅ Write unit tests alongside new features

## Assessment Criteria (Gate 2)

| Criterion | Evidence | Weight |
|-----------|----------|--------|
| API and Architecture | REST conventions, layers, DI, validation | 25% |
| Data and Security | Persistence, safe queries, authorization | 15% |
| Testing and Debugging | Useful tests and failure diagnosis | 15% |

**Pass Criteria:** Working core endpoints and satisfactory code walkthrough

## Submission Checklist

- ☐ Source code with clear project structure
- ☐ README with setup and run instructions
- ☐ OpenAPI documentation and example requests
- ☐ Database setup or migration instructions
- ☐ Automated test suite and results
- ☐ Build/test evidence and known limitations

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
- [Dot Net Tutorials - ASP.NET Core Web API](https://dotnettutorials.net/course/asp-net-core-web-api-tutorials/)

## Status Summary

| Day | Topic | Status | Tests |
|-----|-------|--------|-------|
| 06 | REST API Basics | ✅ Complete | 9/9 ✓ |
| 07 | DI & Middleware | ✅ Complete | 14/14 ✓ |
| 08 | Layered Architecture | ✅ Complete | Build ✓ |
| 09 | EF Core Integration | 📘 Guide Provided | - |
| 10 | Security & Gate 2 | 📘 Guide Provided | - |

**Completion:** 60% implemented (Days 06-08), 40% documented with implementation guides (Days 09-10)

## Implementation Guides

Days 09 and 10 include comprehensive implementation guides with:
- Complete code examples and patterns
- Step-by-step setup instructions
- Interview questions with answers
- Best practices and security considerations
- Testing strategies

See `DAY09_IMPLEMENTATION_GUIDE.md` and `DAY10_IMPLEMENTATION_GUIDE.md` for details.

## Next Steps

1. ✅ Day 06-08: Fully implemented and tested
2. 📘 Day 09: Follow implementation guide to add EF Core
3. 📘 Day 10: Follow implementation guide to add JWT auth
4. Gate 2 Assessment: Code walkthrough and demos
5. ✅ Pushed to GitHub: https://github.com/yogeshSoftSuave123/dotnet-learning.git

## Notes

- All projects target .NET 8.0
- Uses in-memory data for Days 06-08 (database added in Day 09)
- JWT authentication added in Day 10
- Each day builds upon previous days' concepts
- Tests focus on business logic and validation, not infrastructure
