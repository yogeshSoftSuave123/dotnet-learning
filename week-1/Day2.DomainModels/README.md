# Day 2: Object-Oriented C# - Domain Models

## Overview
This class library demonstrates object-oriented programming principles in C# including classes, inheritance, interfaces, encapsulation, and SOLID principles through an employee and department management domain model.

## Project Structure
```
Day2.DomainModels/
├── Entities/
│   ├── Person.cs           (Abstract base class)
│   ├── Employee.cs         (Inherits from Person)
│   └── Department.cs       (Aggregates Employees)
├── Interfaces/
│   ├── IEmployeeRepository.cs
│   └── IDepartmentRepository.cs
├── Repositories/
│   ├── InMemoryEmployeeRepository.cs
│   └── InMemoryDepartmentRepository.cs
└── README.md
```

## Domain Classes

### 1. Person (Abstract Base Class)
- **Purpose**: Base class for all person-related entities
- **Properties**: Name, Email
- **Methods**: Abstract `GetDisplayInfo()`, `UpdateEmail()`
- **OOP Concepts**: Abstraction, Encapsulation

### 2. Employee (Concrete Class)
- **Purpose**: Represents an employee entity
- **Inherits**: Person
- **Properties**: EmployeeId, HireDate, Salary, Department
- **Methods**: `GetYearsOfService()`, `GetDisplayInfo()`, `AssignToDepartment()`
- **OOP Concepts**: Inheritance, Polymorphism, Encapsulation

### 3. Department (Aggregate Root)
- **Purpose**: Represents a department and manages its employees
- **Properties**: DepartmentId, Name, Location, Employees (read-only collection)
- **Methods**: `AddEmployee()`, `RemoveEmployee()`, `GetEmployeeCount()`, `GetTotalSalaryExpense()`
- **OOP Concepts**: Encapsulation, Composition

## Interface-Based Design

### IEmployeeRepository Interface
Defines contract for employee data operations:
- `Add(Employee employee)`
- `Remove(string employeeId)`
- `GetById(string employeeId)`
- `GetAll()`
- `GetByDepartment(string departmentId)`
- `Update(Employee employee)`

### Implementation: InMemoryEmployeeRepository
- Implements `IEmployeeRepository`
- Uses `Dictionary<string, Employee>` for storage
- Case-insensitive key comparison
- Validation and error handling

### Benefits of Interface-Based Design
1. **Abstraction**: Clients depend on interface, not implementation
2. **Testability**: Easy to mock for unit testing
3. **Flexibility**: Can swap implementations (in-memory, database, API)
4. **Dependency Inversion**: High-level modules don't depend on low-level details

## Usage Example

```csharp
using Day2.DomainModels.Entities;
using Day2.DomainModels.Interfaces;
using Day2.DomainModels.Repositories;

// Create repositories
IEmployeeRepository empRepo = new InMemoryEmployeeRepository();
IDepartmentRepository deptRepo = new InMemoryDepartmentRepository();

// Create department
var itDept = new Department("IT01", "Information Technology", "Building A");
deptRepo.Add(itDept);

// Create employees
var emp1 = new Employee("E001", "John Doe", "john@company.com",
    DateTime.Parse("2020-01-15"), 75000m);
var emp2 = new Employee("E002", "Jane Smith", "jane@company.com",
    DateTime.Parse("2021-03-20"), 80000m);

// Add employees to repository
empRepo.Add(emp1);
empRepo.Add(emp2);

// Assign employees to department
itDept.AddEmployee(emp1);
itDept.AddEmployee(emp2);

// Query
var allEmployees = empRepo.GetAll();
var itEmployees = empRepo.GetByDepartment("IT01");
var totalExpense = itDept.GetTotalSalaryExpense();

Console.WriteLine($"Total IT Department Expense: ${totalExpense:N2}");
Console.WriteLine($"Employee Count: {itDept.GetEmployeeCount()}");
```

## OOP Concepts Demonstrated

### Encapsulation
- Private fields with public properties
- Validation in constructors and methods
- Read-only collections (`IReadOnlyList<Employee>`)
- Private setters (`public string EmployeeId { get; private set; }`)

### Inheritance
- `Employee` inherits from `Person`
- Base class provides common properties and behavior
- Derived class extends with specific functionality

### Polymorphism
- Abstract method `GetDisplayInfo()` implemented differently in derived classes
- Interface implementations allow polymorphic behavior
- Method overriding

### Abstraction
- Abstract `Person` class cannot be instantiated
- Interfaces define contracts without implementation
- Clients work with abstractions, not concrete types

## Access Modifiers Used

- **public**: Accessible from anywhere (APIs, public methods)
- **private**: Accessible only within the class (internal fields like `_employees`)
- **protected**: Accessible in class and derived classes (properties in `Person`)
- **internal**: Accessible within the same assembly (can be used for internal helpers)

## Records vs Classes

This project uses **classes** because:
- Entities have identity (EmployeeId, DepartmentId)
- Mutable state is needed (salary changes, department assignments)
- Behavior and methods are important

**Records** would be appropriate for:
- Value objects (Address, Money)
- DTOs (Data Transfer Objects)
- Immutable data
- Positional construction

Example record:
```csharp
public record Address(string Street, string City, string ZipCode);
```

## Structs vs Classes

This project uses **classes** because:
- Entities are reference types
- Inheritance is needed
- Objects can be large

**Structs** would be appropriate for:
- Small value types (Point, Color)
- Data without identity
- Performance-critical scenarios

Example struct:
```csharp
public struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}
```

## Class Relationships

### Composition vs Inheritance
- **Inheritance**: Employee "is-a" Person (use inheritance)
- **Composition**: Department "has-a" collection of Employees (use composition)

**Favor composition over inheritance** when:
- Relationship is "has-a" not "is-a"
- You need flexibility to change behavior at runtime
- You want to avoid tight coupling

Example of composition:
```csharp
public class Department
{
    private List<Employee> _employees;  // Composition: Department has employees
}
```

## Build Instructions

```bash
cd Day2.DomainModels
dotnet build
```

## See Also
- [SOLID_PRINCIPLES.md](SOLID_PRINCIPLES.md) - Detailed SOLID explanation
- [CLASS_DIAGRAM.md](CLASS_DIAGRAM.md) - UML class diagram
