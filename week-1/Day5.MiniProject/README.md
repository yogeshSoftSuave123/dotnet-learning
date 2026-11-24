# Day 5: C# Checkpoint and Mini-Build (Gate 1)

## Overview
This mini-project demonstrates all concepts from Week 1: OOP principles, LINQ queries, async/await with cancellation, and comprehensive unit testing with xUnit. It implements an Employee Management System with full CRUD operations.

## Project Structure

```
Day5.MiniProject/
├── Models/
│   └── Employee.cs                 # Domain model with validation
├── Interfaces/
│   └── IEmployeeRepository.cs      # Repository contract
├── Services/
│   ├── EmployeeRepository.cs       # Async repository implementation
│   └── EmployeeService.cs          # Business logic layer
├── Program.cs                      # Console application demo
└── README.md

Day5.Tests/
├── EmployeeTests.cs                # 20+ unit tests for Employee model
├── EmployeeServiceTests.cs         # 25+ unit tests for service layer
└── Day5.Tests.csproj
```

## Features Demonstrated

### 1. Object-Oriented Programming
- **Encapsulation**: Private setters, validation in constructors
- **Abstraction**: Interface-based design (`IEmployeeRepository`)
- **Properties**: Auto-properties, read-only properties
- **Methods**: Business logic encapsulated in domain models

### 2. LINQ Queries
- Filtering with `Where()`
- Projection with `Select()`
- Grouping with `GroupBy()`
- Ordering with `OrderBy()` and `OrderByDescending()`
- Aggregation with `Sum()`, `Average()`, `Count()`
- Take operations

### 3. Async/Await
- All repository operations are async
- Cancellation token support throughout
- Parallel async operations with `Task.WhenAll()`
- Proper async naming conventions

### 4. Collections
- `Dictionary<string, Employee>` for fast lookups
- `List<Employee>` for collections
- LINQ operations on collections

### 5. Exception Handling
- Input validation with appropriate exceptions
- `ArgumentNullException` for null parameters
- `ArgumentException` for invalid values
- `ArgumentOutOfRangeException` for out-of-range values
- `InvalidOperationException` for business rule violations

## Build and Run

### Build the Project
```bash
cd Day5.MiniProject
dotnet build
```

### Run the Application
```bash
dotnet run
```

### Expected Output
```
=== Employee Management System - Week 1 Capstone ===

Initializing sample data...
Added 8 employees

=== All Employees ===
[E001] Alice Johnson    | Engineering     | $95,000  | Active
[E002] Bob Smith        | HR              | $65,000  | Active
...

=== Employee Statistics ===
Total: 8 (8 active, 0 inactive)
Average Salary: $82,500.00
Total Expense: $660,000.00
Departments: 4
Average Age: 36.0 years

=== LINQ Query Demonstrations ===
...
```

## Run Tests

```bash
cd Day5.Tests
dotnet test

# Verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Expected Test Output
```
Passed!  - Failed:     0, Passed:    45+, Skipped:     0
```

## Unit Test Coverage

### Employee Model Tests (20+ tests)

#### Normal Cases
- Constructor with valid data
- Get years of service
- Calculate annual bonus
- Deactivate/activate employee
- ToString formatting

#### Boundary Cases
- Minimum age (18)
- Maximum age (100)
- Zero bonus percentage
- 100% bonus percentage

#### Invalid Inputs
- Null/empty employee ID
- Null/empty name
- Negative salary
- Age below 18
- Age above 100
- Negative bonus percentage
- Bonus percentage above 100

### Service Layer Tests (25+ tests)

#### Normal Cases
- Calculate average salary (all employees)
- Calculate average salary by department
- Get employee count by department
- Get top earners
- Search employees by name/department
- Get statistics
- Get employees by age range
- Deactivate employees by predicate
- Give bonus to top performers

#### Boundary Cases
- Non-existent department returns zero
- Request more top earners than exist
- Empty search term returns empty list
- Exact age match
- Zero bonus percentage

#### Invalid Inputs
- Zero or negative count
- Null predicate
- Negative age
- Min age greater than max age
- Null repository

#### Regression Tests
- Case-insensitive department search

## Key Methods

### Employee Model

```csharp
// Constructor with validation
public Employee(string employeeId, string name, string email,
    string department, decimal salary, DateTime hireDate, int age)

// Calculate years of service
public int GetYearsOfService()

// Calculate bonus
public decimal CalculateAnnualBonus(decimal bonusPercentage)

// Activate/Deactivate
public void Deactivate()
public void Activate()
```

### Repository Interface

```csharp
Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
Task<Employee?> GetByIdAsync(string employeeId, CancellationToken cancellationToken = default)
Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
Task DeleteAsync(string employeeId, CancellationToken cancellationToken = default)
Task<List<Employee>> FindByDepartmentAsync(string department, CancellationToken cancellationToken = default)
```

### Service Layer

```csharp
Task<decimal> CalculateAverageSalaryAsync(string? department = null, CancellationToken cancellationToken = default)
Task<Dictionary<string, int>> GetEmployeeCountByDepartmentAsync(CancellationToken cancellationToken = default)
Task<List<Employee>> GetTopEarnersAsync(int count, CancellationToken cancellationToken = default)
Task<List<Employee>> SearchEmployeesAsync(string searchTerm, CancellationToken cancellationToken = default)
Task<EmployeeStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
Task<List<Employee>> GetEmployeesByAgeRangeAsync(int minAge, int maxAge, CancellationToken cancellationToken = default)
```

## Code Walkthrough Examples

### 1. LINQ Query Example
```csharp
// Get top earners using LINQ
public async Task<List<Employee>> GetTopEarnersAsync(int count, CancellationToken cancellationToken = default)
{
    var employees = await _repository.GetAllAsync(cancellationToken);

    return employees
        .OrderByDescending(e => e.Salary)  // Sort by salary descending
        .Take(count)                        // Take top N
        .ToList();                          // Materialize query
}
```

**Explanation**:
- Retrieves all employees asynchronously
- Uses LINQ `OrderByDescending` to sort (O(n log n))
- Uses `Take` to get top N results
- Materializes with `ToList()`

### 2. Async Operation Example
```csharp
// Calculate statistics with async operations
public async Task<EmployeeStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
{
    var employees = await _repository.GetAllAsync(cancellationToken);

    return new EmployeeStatistics
    {
        TotalEmployees = employees.Count,
        ActiveEmployees = employees.Count(e => e.IsActive),
        AverageSalary = employees.Average(e => e.Salary),
        TotalSalaryExpense = employees.Sum(e => e.Salary)
    };
}
```

**Explanation**:
- Uses `await` to asynchronously get employees
- Respects cancellation token
- Uses LINQ aggregation methods
- Returns strongly-typed statistics object

### 3. SOLID Principle: Dependency Inversion
```csharp
// Service depends on interface, not concrete class
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
```

**Benefits**:
- Can swap implementations (in-memory, database, mock)
- Easy to test with mock repositories
- Follows Dependency Inversion Principle

## Test Types Demonstrated

### 1. Arrange-Act-Assert Pattern
```csharp
[Fact]
public async Task GetTopEarnersAsync_ReturnsCorrectEmployees()
{
    // Arrange - Set up test data
    var repository = await CreateRepositoryWithSampleDataAsync();
    var service = new EmployeeService(repository);

    // Act - Execute the method
    var result = await service.GetTopEarnersAsync(3);

    // Assert - Verify results
    Assert.Equal(3, result.Count);
    Assert.Equal("Charlie", result[0].Name);
}
```

### 2. Theory Tests with Multiple Inputs
```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
public async Task GetTopEarnersAsync_InvalidCount_ThrowsException(int invalidCount)
{
    var service = new EmployeeService(repository);

    await Assert.ThrowsAsync<ArgumentException>(() =>
        service.GetTopEarnersAsync(invalidCount));
}
```

### 3. Boundary Tests
```csharp
[Fact]
public void Constructor_MinimumAge_CreatesEmployee()
{
    var employee = new Employee("E001", "John", "john@test.com", "IT", 50000, DateTime.Now, 18);
    Assert.Equal(18, employee.Age);
}
```

### 4. Regression Tests
```csharp
[Fact]
public async Task CalculateAverageSalaryAsync_CaseInsensitiveDept_ReturnsCorrectAverage()
{
    var result1 = await service.CalculateAverageSalaryAsync("engineering");
    var result2 = await service.CalculateAverageSalaryAsync("ENGINEERING");

    Assert.Equal(result1, result2);
}
```

## Async Pitfalls Avoided

1. **Don't use `.Result` or `.Wait()`** - Always use `await`
2. **Propagate CancellationToken** - Pass it through all async methods
3. **Handle OperationCanceledException** - Catch cancellation gracefully
4. **Avoid async void** - Return `Task` instead
5. **Don't create unnecessary tasks** - Use `ValueTask` when appropriate

## OOP, LINQ, Collections and Async Summary

### OOP Principles Applied
- **Encapsulation**: Private fields, public properties, validation
- **Abstraction**: Interface-based repositories
- **Single Responsibility**: Each class has one job
- **Dependency Inversion**: Depend on interfaces, not concrete classes

### LINQ Queries Used
- `Where` - Filter employees by criteria
- `Select` - Project/transform data
- `OrderBy/OrderByDescending` - Sort results
- `GroupBy` - Group by department
- `Sum, Average, Count, Min, Max` - Aggregations
- `Take` - Limit results
- `Distinct` - Remove duplicates
- `ToDictionary` - Convert to dictionary

### Collections Used
- `List<Employee>` - Ordered collection
- `Dictionary<string, Employee>` - Key-value storage with O(1) lookup
- `IEnumerable<T>` - Deferred execution
- `IReadOnlyList<T>` - Read-only access

### Async/Await Concepts
- `async Task<T>` - Asynchronous method returning value
- `await` - Await async operation
- `CancellationToken` - Cooperative cancellation
- `Task.WhenAll` - Parallel async operations
- Exception propagation in async methods

## Gate 1 Checklist

✅ Working code with no compilation errors
✅ Passing unit tests (45+ tests)
✅ OOP principles demonstrated (encapsulation, abstraction, interfaces)
✅ LINQ queries for filtering, grouping, sorting (8+ different operations)
✅ Async/await with cancellation token support
✅ Collections (List, Dictionary)
✅ Exception handling with appropriate exception types
✅ Unit tests for normal, boundary, and invalid inputs
✅ Arrange-Act-Assert pattern used consistently
✅ Clear code structure and naming conventions

## Knowledge Check Topics

Can you explain without notes:

1. **OOP**: Difference between class and interface? When to use abstract class vs interface?
2. **LINQ**: What is deferred execution? IEnumerable vs IQueryable?
3. **Collections**: When to use List vs Dictionary? Time complexity of operations?
4. **Async/Await**: Task vs Thread? How does cancellation token work? What is ConfigureAwait?

## Further Improvements

Potential enhancements for Week 2:
- Add database persistence (Entity Framework Core)
- RESTful API endpoints (ASP.NET Core)
- Logging and dependency injection
- More complex business rules
- Integration tests
- Performance optimization

## References

- C# Programming Guide: https://learn.microsoft.com/en-us/dotnet/csharp/
- LINQ Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/linq/
- Async Programming: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
- Unit Testing: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
