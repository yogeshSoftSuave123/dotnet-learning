# Day 4: Unit Testing, Async C# and .NET Tooling

## Overview
This project demonstrates asynchronous programming with async/await, cancellation tokens, unit testing with xUnit, and the Arrange-Act-Assert pattern.

## Project Structure

```
Day4.AsyncApp/          # Class library with async methods
├── EmployeeService.cs  # Service with async operations
└── README.md

Day4.Tests/             # xUnit test project
├── EmployeeServiceTests.cs  # 18 unit tests
└── Day4.Tests.csproj
```

## Build and Test Instructions

### Build Projects
```bash
cd Day4.AsyncApp
dotnet build

cd ../Day4.Tests
dotnet build
```

### Run Tests
```bash
cd Day4.Tests
dotnet test

# Verbose output
dotnet test --logger "console;verbosity=detailed"

# With code coverage
dotnet test /p:CollectCoverage=true
```

## Async Features Demonstrated

### 1. Basic Async/Await
```csharp
public async Task<List<Employee>> GetAllEmployeesAsync(
    CancellationToken cancellationToken = default)
{
    await Task.Delay(1000, cancellationToken);
    cancellationToken.ThrowIfCancellationRequested();
    return _employees.ToList();
}
```

**Key Points**:
- `async` modifier on method
- `Task<T>` or `Task` return type
- `await` operator to await async operations
- Method name ends with `Async` by convention

### 2. Cancellation Tokens
```csharp
public async Task<Employee?> GetEmployeeByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
{
    await Task.Delay(500, cancellationToken);
    cancellationToken.ThrowIfCancellationRequested();
    return _employees.FirstOrDefault(e => e.Id == id);
}
```

**Usage**:
```csharp
var cts = new CancellationTokenSource();
cts.CancelAfter(5000);  // Cancel after 5 seconds

try
{
    var employees = await service.GetAllEmployeesAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

**Benefits**:
- Graceful cancellation of long-running operations
- Resource cleanup
- Improved responsiveness

### 3. Task.WhenAll - Parallel Async Operations
```csharp
public async Task<Dictionary<string, decimal>> GetDepartmentAveragesAsync(
    CancellationToken cancellationToken = default)
{
    var tasks = new List<Task<(string Department, decimal Average)>>();

    foreach (var dept in departments)
    {
        tasks.Add(CalculateDepartmentAverageAsync(dept, cancellationToken));
    }

    var results = await Task.WhenAll(tasks);
    return results.ToDictionary(r => r.Department, r => r.Average);
}
```

**Benefits**:
- Multiple async operations run in parallel
- Waits for all tasks to complete
- More efficient than sequential execution

### 4. Timeout Handling
```csharp
public async Task<List<Employee>> SearchEmployeesWithTimeoutAsync(
    string searchTerm,
    int timeoutMilliseconds = 5000)
{
    using var cts = new CancellationTokenSource(timeoutMilliseconds);

    try
    {
        return await SearchEmployeesAsync(searchTerm, cts.Token);
    }
    catch (OperationCanceledException)
    {
        throw new TimeoutException($"Search timed out after {timeoutMilliseconds}ms");
    }
}
```

### 5. Progress Reporting
```csharp
public async Task<List<Employee>> ProcessEmployeesWithProgressAsync(
    Func<Employee, bool> filter,
    IProgress<int>? progress = null,
    CancellationToken cancellationToken = default)
{
    for (int i = 0; i < totalCount; i++)
    {
        // Process item
        var percentComplete = (int)((i + 1) * 100.0 / totalCount);
        progress?.Report(percentComplete);
    }
}
```

**Usage**:
```csharp
var progress = new Progress<int>(percent =>
{
    Console.WriteLine($"Progress: {percent}%");
});

var result = await service.ProcessEmployeesWithProgressAsync(
    e => e.Salary > 80000,
    progress);
```

### 6. ValueTask for Performance
```csharp
public ValueTask<int> GetEmployeeCountAsync()
{
    // For synchronous operations, ValueTask avoids allocation
    return new ValueTask<int>(_employees.Count);
}
```

**When to use ValueTask**:
- Result is often available synchronously
- Performance-critical scenarios
- Reduces heap allocations

## Unit Testing with xUnit

### Test Project Setup

**Day4.Tests.csproj**:
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.9.0" />
  <PackageReference Include="xunit" Version="2.6.6" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\Day4.AsyncApp\Day4.AsyncApp.csproj" />
</ItemGroup>
```

### Arrange-Act-Assert Pattern

```csharp
[Fact]
public async Task GetEmployeeByIdAsync_ValidId_ReturnsEmployee()
{
    // Arrange - Set up test data and dependencies
    var service = new EmployeeService();
    var expectedId = 1;

    // Act - Execute the method being tested
    var result = await service.GetEmployeeByIdAsync(expectedId);

    // Assert - Verify the results
    Assert.NotNull(result);
    Assert.Equal(expectedId, result.Id);
    Assert.Equal("Alice Johnson", result.Name);
}
```

### Test Types

#### 1. Fact Tests
Single test case with no parameters:
```csharp
[Fact]
public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
{
    var service = new EmployeeService();
    var result = await service.GetAllEmployeesAsync();
    Assert.Equal(5, result.Count);
}
```

#### 2. Theory Tests
Parameterized tests with multiple test cases:
```csharp
[Theory]
[InlineData(0)]
[InlineData(-1)]
[InlineData(-10)]
public async Task GetEmployeeByIdAsync_InvalidId_ThrowsException(int invalidId)
{
    var service = new EmployeeService();

    await Assert.ThrowsAsync<ArgumentException>(() =>
        service.GetEmployeeByIdAsync(invalidId));
}
```

### Common xUnit Assertions

```csharp
// Equality
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);

// Null checks
Assert.Null(value);
Assert.NotNull(value);

// Boolean
Assert.True(condition);
Assert.False(condition);

// Collections
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.Single(collection);
Assert.Contains(item, collection);
Assert.All(collection, item => Assert.True(condition));

// Exceptions
Assert.Throws<TException>(() => method());
await Assert.ThrowsAsync<TException>(() => asyncMethod());

// Ranges
Assert.InRange(actual, low, high);
```

### Test Categories

#### Normal Cases (Happy Path)
- Valid inputs return expected outputs
- Tests 1, 2, 5, 8, 10, 12, 13

#### Boundary Cases
- Edge values (zero, empty, null)
- Tests 3, 6, 11, 17

#### Invalid Inputs
- Negative values, null, empty strings
- Tests 4, 7

#### Exception Handling
- Methods throw correct exceptions
- Tests 4, 7, 9, 14, 15

#### Cancellation
- Operations respect cancellation tokens
- Tests 9, 15

#### Concurrency
- Multiple simultaneous operations
- Test 18

## Test Coverage Summary

| Test # | Test Name | Type | Purpose |
|--------|-----------|------|---------|
| 1 | GetAllEmployeesAsync_ReturnsAllEmployees | Normal | Verify basic async operation |
| 2 | GetEmployeeByIdAsync_ValidId_ReturnsEmployee | Normal | Test valid ID lookup |
| 3 | GetEmployeeByIdAsync_InvalidId_ReturnsNull | Boundary | Test non-existent ID |
| 4 | GetEmployeeByIdAsync_ZeroOrNegativeId_ThrowsException | Invalid | Test invalid input |
| 5 | CalculateAverageSalaryAsync_ValidDepartment | Normal | Test aggregation |
| 6 | CalculateAverageSalaryAsync_NonExistent_ReturnsZero | Boundary | Test missing data |
| 7 | CalculateAverageSalaryAsync_NullDept_ThrowsException | Invalid | Test null input |
| 8 | GetStatisticsAsync_ReturnsValidStatistics | Normal | Test complex calculation |
| 9 | GetAllEmployeesAsync_CancellationToken_Throws | Cancellation | Test cancellation |
| 10 | SearchEmployeesAsync_ValidTerm_ReturnsMatching | Normal | Test search |
| 11 | SearchEmployeesAsync_EmptyTerm_ReturnsEmpty | Boundary | Test empty search |
| 12 | GetDepartmentAveragesAsync_ReturnsCorrect | Normal | Test parallel ops |
| 13 | GetEmployeeCountAsync_ReturnsCorrectCount | Normal | Test ValueTask |
| 14 | SearchWithTimeout_ShortTimeout_ThrowsTimeout | Timeout | Test timeout |
| 15 | GetStatisticsAsync_CancelledMidway_Throws | Cancellation | Test mid-operation cancel |
| 16 | ProcessWithProgress_ReportsProgress | Progress | Test progress reporting |
| 17 | SearchByDepartment_ReturnsMatches | Boundary | Test department search |
| 18 | GetEmployeeByIdAsync_ConcurrentCalls | Concurrency | Test concurrent access |

**Total: 18 Unit Tests**

## Task vs Thread

| Feature | Task | Thread |
|---------|------|--------|
| **Abstraction** | High-level | Low-level |
| **Resource Usage** | Efficient (thread pool) | Heavy (dedicated thread) |
| **Return Values** | Task<T> | No built-in support |
| **Cancellation** | CancellationToken | Manual |
| **Exception Handling** | Automatic propagation | Manual handling |
| **Composability** | Task.WhenAll, WhenAny | Manual coordination |
| **Use Case** | I/O-bound, async operations | CPU-bound, long-running |

**Task Example**:
```csharp
var task = Task.Run(() => LongRunningOperation());
var result = await task;
```

**Thread Example**:
```csharp
var thread = new Thread(() => LongRunningOperation());
thread.Start();
thread.Join();  // Wait for completion
```

## Async Best Practices

1. **Always await async methods** - Don't use `.Result` or `.Wait()`
2. **Use ConfigureAwait(false)** in libraries to avoid deadlocks
3. **Don't mix sync and async** - Use async all the way
4. **Name async methods with Async suffix**
5. **Return Task, not async void** (except event handlers)
6. **Pass CancellationToken** to allow cancellation
7. **Use Task.WhenAll** for parallel operations, not sequential awaits

## Common Pitfalls

### 1. Deadlock with .Result
```csharp
// DON'T DO THIS - Can cause deadlock
var result = GetEmployeesAsync().Result;

// DO THIS - Await properly
var result = await GetEmployeesAsync();
```

### 2. Fire and Forget
```csharp
// DON'T DO THIS - Exceptions are lost
_ = SomeAsyncMethod();

// DO THIS - Await or handle exceptions
await SomeAsyncMethod();
```

### 3. Async Void
```csharp
// DON'T DO THIS - Can't be awaited, exceptions crash app
public async void ProcessData() { }

// DO THIS - Return Task
public async Task ProcessData() { }
```

### 4. Unnecessary Async
```csharp
// DON'T DO THIS - Unnecessary async/await
public async Task<int> GetCount()
{
    return await Task.FromResult(employees.Count);
}

// DO THIS - Return Task directly
public Task<int> GetCount()
{
    return Task.FromResult(employees.Count);
}

// OR BETTER - Use ValueTask
public ValueTask<int> GetCount()
{
    return new ValueTask<int>(employees.Count);
}
```

## .NET CLI Commands

### Project Management
```bash
# Create new project
dotnet new console -n MyApp
dotnet new classlib -n MyLibrary
dotnet new xunit -n MyTests

# Add project reference
dotnet add MyTests reference MyLibrary

# Add NuGet package
dotnet add package xunit
```

### Build and Run
```bash
# Restore dependencies
dotnet restore

# Build project
dotnet build

# Build in Release mode
dotnet build -c Release

# Run application
dotnet run

# Run with arguments
dotnet run -- arg1 arg2
```

### Testing
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter FullyQualifiedName~GetEmployeeByIdAsync

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

### Debugging
```bash
# Build with debug symbols
dotnet build -c Debug

# Run with debugger
dotnet run --launch-profile Debug
```

## What Makes a Good Unit Test?

1. **Fast**: Runs quickly (milliseconds)
2. **Isolated**: Independent of other tests
3. **Repeatable**: Same results every time
4. **Self-checking**: Automatic pass/fail (assertions)
5. **Timely**: Written with or before code

### Test Naming Convention
```
[MethodName]_[Scenario]_[ExpectedBehavior]

Examples:
- GetEmployeeByIdAsync_ValidId_ReturnsEmployee
- CalculateAverageSalaryAsync_NullDept_ThrowsException
- SearchEmployeesAsync_EmptyTerm_ReturnsEmptyList
```

## Running the Tests

```bash
cd Day4.Tests

# Run all tests
dotnet test

# Expected output:
# Starting test execution, please wait...
# A total of 1 test files matched the specified pattern.
#
# Passed!  - Failed:     0, Passed:    18, Skipped:     0, Total:    18
```

## Further Reading

- Async/await: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
- xUnit documentation: https://xunit.net/
- Unit testing best practices: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
