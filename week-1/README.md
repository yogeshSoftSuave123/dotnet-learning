# .NET Training: Week 1 - C# Foundations

## Overview
This repository contains all code implementations for Week 1 of the .NET training program, covering C# fundamentals, OOP, LINQ, async programming, and unit testing.

## Prerequisites
- .NET SDK 8.0 or later
- Text editor or IDE (Visual Studio, VS Code, or Rider)
- Terminal/Command Prompt

## Install .NET SDK
If you don't have .NET installed:

### Ubuntu/Linux
```bash
wget https://dot.net/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
```

### Windows
Download from: https://dotnet.microsoft.com/download/dotnet/8.0

### Verify Installation
```bash
dotnet --version
# Should output: 8.0.x or later
```

## Project Structure

```
week-1/
├── Day1.ConsoleApp/              # C# Fundamentals
│   ├── Program.cs
│   ├── README.md
│   └── VALUE_VS_REFERENCE_TYPES.md
│
├── Day2.DomainModels/            # Object-Oriented C#
│   ├── Entities/
│   ├── Interfaces/
│   ├── Repositories/
│   ├── README.md
│   ├── SOLID_PRINCIPLES.md
│   └── CLASS_DIAGRAM.md
│
├── Day3.LinqExercises/           # Modern C# and LINQ
│   ├── Program.cs
│   └── README.md
│
├── Day4.AsyncApp/                # Async C# and Unit Testing
│   ├── EmployeeService.cs
│   └── README.md
│
├── Day4.Tests/                   # xUnit Test Project
│   └── EmployeeServiceTests.cs
│
├── Day5.MiniProject/             # Capstone Mini-Build
│   ├── Models/
│   ├── Interfaces/
│   ├── Services/
│   ├── Program.cs
│   └── README.md
│
├── Day5.Tests/                   # Comprehensive Tests
│   ├── EmployeeTests.cs
│   └── EmployeeServiceTests.cs
│
├── Week1Assessment.sln           # Solution file
└── README.md                     # This file
```

## Quick Start

### Build All Projects
```bash
cd week-1
dotnet build Week1Assessment.sln
```

### Run Individual Projects

#### Day 1: Console Application
```bash
cd Day1.ConsoleApp
dotnet run
```

#### Day 3: LINQ Exercises
```bash
cd Day3.LinqExercises
dotnet run
```

#### Day 5: Mini Project
```bash
cd Day5.MiniProject
dotnet run
```

### Run All Tests
```bash
# Run Day 4 tests
cd Day4.Tests
dotnet test

# Run Day 5 tests
cd ../Day5.Tests
dotnet test

# Run all tests from solution directory
cd ..
dotnet test Week1Assessment.sln
```

## Daily Breakdown

### Day 1: C# Fundamentals ✅
**Topics**: Variables, types, operators, control flow, methods, collections, exceptions

**Deliverables**:
- ✅ Console application processing employee records
- ✅ README with run instructions and 3 sample I/O
- ✅ Detailed notes on value vs reference types
- ✅ LeetCode: Concatenation of Array (conceptual)

**Key Files**:
- `Day1.ConsoleApp/Program.cs` - Employee management console app
- `Day1.ConsoleApp/VALUE_VS_REFERENCE_TYPES.md` - Comprehensive explanation

**Run**: `cd Day1.ConsoleApp && dotnet run`

---

### Day 2: Object-Oriented C# ✅
**Topics**: Classes, inheritance, polymorphism, interfaces, SOLID principles

**Deliverables**:
- ✅ Domain model class library (Employee, Department, Person)
- ✅ Interface-based repository pattern
- ✅ UML class diagram documentation
- ✅ SOLID principles explanation with examples
- ✅ LeetCode: Number of Steps to Reduce a Number to Zero (conceptual)

**Key Files**:
- `Day2.DomainModels/Entities/` - Domain models
- `Day2.DomainModels/Interfaces/` - Repository interfaces
- `Day2.DomainModels/SOLID_PRINCIPLES.md` - SOLID explained
- `Day2.DomainModels/CLASS_DIAGRAM.md` - UML diagrams

**Build**: `cd Day2.DomainModels && dotnet build`

---

### Day 3: Modern C# and LINQ ✅
**Topics**: Collections, LINQ, lambdas, delegates, pattern matching

**Deliverables**:
- ✅ LINQ exercise file with 8+ queries
- ✅ Unit-testable filtering and grouping methods
- ✅ Each query documented with input, output, complexity
- ✅ LeetCode: Contains Duplicate (conceptual)

**Key Files**:
- `Day3.LinqExercises/Program.cs` - 8 LINQ query demonstrations
- `Day3.LinqExercises/README.md` - Query documentation

**Run**: `cd Day3.LinqExercises && dotnet run`

---

### Day 4: Unit Testing, Async C# and .NET Tooling ✅
**Topics**: xUnit, async/await, Task, cancellation tokens, .NET CLI

**Deliverables**:
- ✅ Solution with app, class library, and xUnit test projects
- ✅ 18 passing unit tests with Arrange-Act-Assert
- ✅ Async method with cancellation handling
- ✅ Screenshot/log of `dotnet build` and `dotnet test` succeeding
- ✅ LeetCode: Best Time to Buy and Sell Stock (conceptual)

**Key Files**:
- `Day4.AsyncApp/EmployeeService.cs` - Async service with cancellation
- `Day4.Tests/EmployeeServiceTests.cs` - 18 comprehensive unit tests
- `Day4.AsyncApp/README.md` - Async concepts explained

**Test**: `cd Day4.Tests && dotnet test`

---

### Day 5: C# Checkpoint and Mini-Build (Gate 1) ✅
**Topics**: Integration of all Week 1 concepts

**Deliverables**:
- ✅ Mini-project source code with README
- ✅ 45+ passing unit tests (normal, boundary, invalid inputs)
- ✅ Regression tests
- ✅ Walkthrough documentation for class, LINQ query, async operation
- ✅ LeetCode: Valid Anagram (conceptual)

**Key Files**:
- `Day5.MiniProject/` - Complete employee management system
- `Day5.Tests/` - Comprehensive test suite (45+ tests)
- `Day5.MiniProject/README.md` - Full project documentation

**Run**: `cd Day5.MiniProject && dotnet run`
**Test**: `cd Day5.Tests && dotnet test`

---

## Concepts Covered

### C# Fundamentals (Day 1)
- ✅ Variables and data types
- ✅ Value types vs Reference types
- ✅ Nullable types
- ✅ Methods and parameters
- ✅ Collections (List, arrays)
- ✅ Exception handling
- ✅ String manipulation

### Object-Oriented Programming (Day 2)
- ✅ Classes and objects
- ✅ Constructors and properties
- ✅ Encapsulation
- ✅ Inheritance and polymorphism
- ✅ Abstract classes
- ✅ Interfaces
- ✅ SOLID principles
- ✅ Composition vs inheritance

### LINQ and Modern C# (Day 3)
- ✅ Lambda expressions
- ✅ LINQ queries (Where, Select, GroupBy, OrderBy)
- ✅ Deferred execution
- ✅ Aggregation (Sum, Average, Count, Min, Max)
- ✅ Generics
- ✅ Extension methods
- ✅ Pattern matching

### Async Programming (Day 4)
- ✅ async/await keywords
- ✅ Task<T> and Task
- ✅ CancellationToken
- ✅ Task.WhenAll for parallel operations
- ✅ Exception handling in async code
- ✅ Async best practices

### Unit Testing (Day 4-5)
- ✅ xUnit framework
- ✅ [Fact] and [Theory] attributes
- ✅ Arrange-Act-Assert pattern
- ✅ Test naming conventions
- ✅ Testing normal cases
- ✅ Testing boundary conditions
- ✅ Testing invalid inputs
- ✅ Exception testing
- ✅ Regression testing

### .NET Tooling
- ✅ dotnet CLI commands
- ✅ Project files (.csproj)
- ✅ Solution files (.sln)
- ✅ NuGet packages
- ✅ Project references
- ✅ dotnet build, run, test

## Test Execution Results

### Expected Output
```bash
dotnet test Week1Assessment.sln

# Expected:
# Day4.Tests: 18 tests passed
# Day5.Tests: 45+ tests passed
# Total: 63+ tests passed, 0 failed
```

## Key Learning Outcomes

By the end of Week 1, you should be able to:

1. **Write idiomatic C# code** with proper naming conventions and style
2. **Design object-oriented systems** using SOLID principles
3. **Query collections effectively** using LINQ
4. **Write asynchronous code** with proper cancellation support
5. **Test code thoroughly** with unit tests covering edge cases
6. **Use .NET CLI** for building, running, and testing projects
7. **Explain technical concepts** without notes:
   - Value vs reference types
   - Interface vs abstract class
   - Composition vs inheritance
   - SOLID principles
   - Deferred execution in LINQ
   - IEnumerable vs IQueryable
   - Task vs Thread
   - What makes a good unit test

## Common .NET CLI Commands

```bash
# Create new projects
dotnet new console -n MyApp
dotnet new classlib -n MyLibrary
dotnet new xunit -n MyTests
dotnet new sln -n MySolution

# Add projects to solution
dotnet sln add MyApp/MyApp.csproj
dotnet sln add MyLibrary/MyLibrary.csproj
dotnet sln add MyTests/MyTests.csproj

# Add project reference
dotnet add MyTests reference MyLibrary

# Add NuGet package
dotnet add package xunit
dotnet add package Microsoft.NET.Test.Sdk

# Restore dependencies
dotnet restore

# Build
dotnet build                    # Build current project
dotnet build MySolution.sln     # Build entire solution
dotnet build -c Release         # Build in Release mode

# Run
dotnet run                      # Run current project
dotnet run --project MyApp      # Run specific project

# Test
dotnet test                     # Run all tests
dotnet test --logger "console;verbosity=detailed"
dotnet test --filter "FullyQualifiedName~EmployeeTests"

# Clean
dotnet clean
```

## Code Quality Standards

### Naming Conventions
- **Classes**: PascalCase (`Employee`, `EmployeeService`)
- **Methods**: PascalCase (`GetEmployeeById`, `CalculateAverage`)
- **Properties**: PascalCase (`EmployeeId`, `Name`)
- **Private fields**: _camelCase (`_employees`, `_repository`)
- **Parameters**: camelCase (`employeeId`, `department`)
- **Local variables**: camelCase (`result`, `avgSalary`)

### Method Naming
- Async methods end with `Async`: `GetEmployeesAsync()`
- Boolean methods start with Is/Has/Can: `IsActive()`, `HasPermission()`
- Event handlers: On/Handle prefix: `OnButtonClick()`, `HandleError()`

### Test Naming
```
[MethodName]_[Scenario]_[ExpectedBehavior]

Examples:
- GetEmployeeById_ValidId_ReturnsEmployee
- CalculateAverage_EmptyList_ReturnsZero
- Constructor_NullName_ThrowsException
```

## Gate 1 Pass Criteria

✅ **Working Code**: All projects build without errors
✅ **Passing Tests**: 63+ tests passing, 0 failures
✅ **Knowledge Check**: Can explain OOP, LINQ, collections, async/await
✅ **Code Walkthrough**: Can explain one class, one LINQ query, one async operation
✅ **SOLID Understanding**: Can provide examples from own code

## Common Issues and Solutions

### Issue: `dotnet: command not found`
**Solution**: Install .NET SDK and add to PATH

### Issue: Tests not discovered
**Solution**:
```bash
dotnet restore
dotnet build
dotnet test
```

### Issue: Project reference errors
**Solution**: Check .csproj file has correct `<ProjectReference>` paths

### Issue: NuGet package restore fails
**Solution**:
```bash
dotnet nuget locals all --clear
dotnet restore
```

## Next Steps: Week 2 Preview

Week 2 will cover:
- ASP.NET Core Web API
- Entity Framework Core
- Dependency Injection
- Middleware and filters
- RESTful API design
- API testing with integration tests

## Resources

### Official Documentation
- C# Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/
- .NET Fundamentals: https://learn.microsoft.com/en-us/dotnet/fundamentals/
- LINQ: https://learn.microsoft.com/en-us/dotnet/csharp/linq/
- Async Programming: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
- xUnit: https://xunit.net/

### Learning Resources
- C# Yellow Book (free): http://www.csharpcourse.com/
- .NET CLI Reference: https://learn.microsoft.com/en-us/dotnet/core/tools/

## LeetCode Problems (Optional Practice)

Week 1 suggested problems:
- Day 1: Concatenation of Array (Easy)
- Day 2: Number of Steps to Reduce a Number to Zero (Easy)
- Day 3: Contains Duplicate (Easy)
- Day 4: Best Time to Buy and Sell Stock (Easy)
- Day 5: Valid Anagram (Easy)

Solve in C# to practice language syntax and built-in methods.

## Support

For questions or issues:
1. Check the README files in each day's folder
2. Review the code comments and documentation
3. Refer to official Microsoft documentation
4. Search Stack Overflow for specific errors

## License

This training material is for educational purposes.

---

**Week 1 Status**: ✅ Complete
**Gate 1**: ✅ Ready for Review
**Next**: Week 2 - ASP.NET Core and Web APIs
