# Day 09: EF Core and Database Integration - Implementation Guide

## Overview
Day 09 replaces the in-memory repositories with Entity Framework Core for real database persistence.

## Implementation Steps

### 1. Add EF Core Packages
```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer  # or Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools

cd ../Day08.LayeredAPI.API
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### 2. Create DbContext

**Infrastructure/Data/ApplicationDbContext.cs**:
```csharp
using Microsoft.EntityFrameworkCore;
using Day08.LayeredAPI.Domain.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Salary).HasPrecision(18, 2);

            // Relationship
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
        });

        // Seed data
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Engineering", Location = "Building A" },
            new Department { Id = 2, Name = "Human Resources", Location = "Building B" },
            new Department { Id = 3, Name = "Marketing", Location = "Building C" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                DepartmentId = 1,
                Position = "Senior Developer",
                Salary = 95000,
                HireDate = new DateTime(2020, 1, 15)
            }
        );
    }
}
```

### 3. Create EF Core Repositories

**Infrastructure/Repositories/EfEmployeeRepository.cs**:
```csharp
using Microsoft.EntityFrameworkCore;
using Day08.LayeredAPI.Application.Interfaces;
using Day08.LayeredAPI.Domain.Entities;
using Day08.LayeredAPI.Infrastructure.Data;

public class EfEmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EfEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)  // Eager loading
            .AsNoTracking()  // Read-only query
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Entry(employee).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
```

### 4. Register DbContext in Program.cs

```csharp
// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    // or options.UseSqlite("Data Source=employees.db"));

// Register EF Core repositories instead of In-Memory
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
```

### 5. Add Connection String

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=employees.db"
  }
}
```

### 6. Create and Apply Migrations

```bash
cd Day08.LayeredAPI/Day08.LayeredAPI.API

# Create initial migration
dotnet ef migrations add InitialCreate --project ../Day08.LayeredAPI.Infrastructure

# Apply migration (creates database)
dotnet ef database update

# View generated SQL
dotnet ef migrations script

# Remove last migration (if needed)
dotnet ef migrations remove
```

### 7. Add Filtering, Sorting, and Pagination

**Application/DTOs/PagedResult.cs**:
```csharp
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}

public class EmployeeQueryParameters
{
    public string? SearchTerm { get; set; }
    public int? DepartmentId { get; set; }
    public string SortBy { get; set; } = "LastName";
    public bool Descending { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

**Service with filtering**:
```csharp
public async Task<PagedResult<EmployeeDto>> GetEmployeesAsync(EmployeeQueryParameters parameters)
{
    var query = _context.Employees
        .Include(e => e.Department)
        .AsNoTracking();

    // Filtering
    if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
    {
        query = query.Where(e =>
            e.FirstName.Contains(parameters.SearchTerm) ||
            e.LastName.Contains(parameters.SearchTerm) ||
            e.Email.Contains(parameters.SearchTerm));
    }

    if (parameters.DepartmentId.HasValue)
    {
        query = query.Where(e => e.DepartmentId == parameters.DepartmentId.Value);
    }

    // Sorting
    query = parameters.SortBy.ToLower() switch
    {
        "firstname" => parameters.Descending
            ? query.OrderByDescending(e => e.FirstName)
            : query.OrderBy(e => e.FirstName),
        "email" => parameters.Descending
            ? query.OrderByDescending(e => e.Email)
            : query.OrderBy(e => e.Email),
        "hiredate" => parameters.Descending
            ? query.OrderByDescending(e => e.HireDate)
            : query.OrderBy(e => e.HireDate),
        _ => parameters.Descending
            ? query.OrderByDescending(e => e.LastName)
            : query.OrderBy(e => e.LastName)
    };

    // Pagination
    var totalCount = await query.CountAsync();
    var items = await query
        .Skip((parameters.Page - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .ToListAsync();

    return new PagedResult<EmployeeDto>
    {
        Items = items.Select(MapToDto).ToList(),
        Page = parameters.Page,
        PageSize = parameters.PageSize,
        TotalCount = totalCount
    };
}
```

## Key Concepts

### Tracking vs AsNoTracking
- **Tracking**: EF Core tracks changes to entities. Use for updates.
- **AsNoTracking**: Read-only queries, better performance.

### Eager Loading vs Lazy Loading
- **Eager Loading**: `.Include(e => e.Department)` - loads related data upfront
- **Explicit Loading**: Load related data on demand
- **Lazy Loading**: Automatic (requires virtual properties)

### N+1 Problem
```csharp
// BAD: N+1 queries (1 for employees + N for departments)
var employees = await _context.Employees.ToListAsync();
foreach (var emp in employees)
{
    var dept = emp.Department.Name;  // Separate query per employee!
}

// GOOD: Single query with join
var employees = await _context.Employees
    .Include(e => e.Department)  // Single query with join
    .ToListAsync();
```

## Interview Questions

**EF Core vs Dapper?**
- EF Core: Full ORM, change tracking, migrations, LINQ
- Dapper: Micro-ORM, faster, more control, write SQL

**What causes the N+1 problem?**
- Querying related entities in a loop instead of eager loading
- Fix: Use `.Include()` for eager loading

**Tracking vs no-tracking?**
- Tracking: For updates, EF tracks changes
- No-tracking: Read-only, better performance, use for queries

## Migration Commands Reference
```bash
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Rollback to specific migration
dotnet ef database update PreviousMigrationName

# Generate SQL script
dotnet ef migrations script

# Remove last migration
dotnet ef migrations remove

# Drop database
dotnet ef database drop
```
