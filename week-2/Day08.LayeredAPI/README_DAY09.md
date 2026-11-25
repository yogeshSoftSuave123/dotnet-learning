# Day 09: Entity Framework Core Integration

## Overview
Day 09 adds Entity Framework Core (EF Core) to the Day 08 layered architecture, replacing in-memory data storage with SQLite database persistence.

## What Was Implemented

### 1. Database Context
Created `ApplicationDbContext` in Infrastructure layer with:
- DbSet properties for Employees and Departments
- Fluent API configuration for entity relationships
- Entity constraints (required fields, max lengths, unique indexes)
- One-to-many relationship: Department → Employees
- Seed data for 3 departments and 2 initial employees

### 2. EF Core Repositories
Implemented repository pattern with EF Core:
- `EfEmployeeRepository` - Database operations for employees
- `EfDepartmentRepository` - Database operations for departments

Both repositories include:
- Eager loading with `.Include()` to avoid N+1 queries
- `.AsNoTracking()` for read-only queries (better performance)
- Async/await pattern for all database operations

### 3. Configuration
- Added connection string in `appsettings.json`
- Registered `ApplicationDbContext` with SQLite provider
- Updated DI registration to use EF Core repositories instead of in-memory

### 4. Database Migration
Created and applied initial migration with:
- Tables: Departments, Employees
- Relationships: Foreign key with RESTRICT delete behavior
- Indexes: Unique index on Employee.Email
- Seed data automatically inserted

## Key EF Core Concepts Applied

### Entity Configuration
```csharp
modelBuilder.Entity<Employee>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
    entity.HasIndex(e => e.Email).IsUnique();
    entity.HasOne(e => e.Department)
          .WithMany(d => d.Employees)
          .HasForeignKey(e => e.DepartmentId)
          .OnDelete(DeleteBehavior.Restrict);
});
```

### Query Optimization
```csharp
// Eager loading to avoid N+1 queries
return await _context.Employees
    .Include(e => e.Department)
    .AsNoTracking()  // Read-only for better performance
    .ToListAsync();
```

## Database Schema

### Employees Table
- Id (Primary Key, Auto-increment)
- FirstName (Required, Max 50)
- LastName (Required, Max 50)
- Email (Required, Max 100, Unique Index)
- PhoneNumber
- DepartmentId (Foreign Key to Departments)
- Position (Required)
- Salary (Decimal 18,2)
- HireDate
- IsActive

### Departments Table
- Id (Primary Key, Auto-increment)
- Name (Required, Max 100)
- Description
- Location (Required, Max 200)
- CreatedAt

### Relationship
- One Department has many Employees
- Each Employee belongs to one Department
- Delete behavior: RESTRICT (cannot delete department with employees)

## Files Modified/Created

### Infrastructure Layer
- `Data/ApplicationDbContext.cs` - EF Core DbContext
- `Repositories/EfEmployeeRepository.cs` - EF Core employee repository
- `Repositories/EfDepartmentRepository.cs` - EF Core department repository
- `Migrations/` - EF Core migration files

### API Layer
- `Program.cs` - Updated to register DbContext and EF repositories
- `appsettings.json` - Added connection string

## Running the Application

### Database is Ready
The database has been created and seeded with initial data:
- 3 Departments: Engineering, Human Resources, Marketing
- 2 Employees: John Doe (Engineering), Jane Smith (HR)

### Build and Run
```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.API
dotnet build
dotnet run
```

### Access Swagger UI
Navigate to: http://localhost:5000/swagger (or the displayed port)

## EF Core Commands Reference

### Create a New Migration
```bash
dotnet ef migrations add MigrationName --project Day08.LayeredAPI.Infrastructure --startup-project Day08.LayeredAPI.API
```

### Apply Migrations
```bash
dotnet ef database update --project Day08.LayeredAPI.Infrastructure --startup-project Day08.LayeredAPI.API
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project Day08.LayeredAPI.Infrastructure --startup-project Day08.LayeredAPI.API
```

### View SQL for Migration
```bash
dotnet ef migrations script --project Day08.LayeredAPI.Infrastructure --startup-project Day08.LayeredAPI.API
```

## Testing the Database Integration

### 1. Get All Employees (should return seeded data)
```http
GET http://localhost:5000/api/employees
```

### 2. Get Employee with Department (eager loading)
```http
GET http://localhost:5000/api/employees/1
```

### 3. Create New Employee
```http
POST http://localhost:5000/api/employees
Content-Type: application/json

{
  "firstName": "Alice",
  "lastName": "Johnson",
  "email": "alice.johnson@company.com",
  "phoneNumber": "+1234567892",
  "departmentId": 1,
  "position": "Junior Developer",
  "salary": 65000
}
```

### 4. Email Uniqueness Constraint
Try creating employee with duplicate email (should fail):
```http
POST http://localhost:5000/api/employees
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "User",
  "email": "john.doe@company.com",
  "departmentId": 1
}
```

## Key Differences from Day 08

| Aspect | Day 08 (In-Memory) | Day 09 (EF Core) |
|--------|-------------------|------------------|
| Data Storage | In-memory list | SQLite database |
| Data Persistence | Lost on restart | Persisted to disk |
| Repository | InMemoryEmployeeRepository | EfEmployeeRepository |
| Relationships | Manual loading | Eager/Lazy loading |
| Concurrency | Not handled | EF Core tracking |
| Transactions | Not supported | DbContext transactions |

## Learning Outcomes

1. **EF Core Fundamentals**: DbContext, DbSet, migrations
2. **Code-First Approach**: Define entities and relationships in code
3. **Fluent API**: Configure entities and relationships
4. **Repository Pattern with EF Core**: Abstract data access
5. **Query Optimization**: Include(), AsNoTracking()
6. **Data Seeding**: Initialize database with test data
7. **Connection String Management**: Configuration-based setup

## Next Steps (Day 10)
- Add JWT Authentication
- Implement role-based authorization
- Secure API endpoints with [Authorize] attribute
- Add login/register endpoints
