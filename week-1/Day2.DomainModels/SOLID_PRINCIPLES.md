# SOLID Principles Explanation

## Overview
SOLID is an acronym for five design principles that make software designs more understandable, flexible, and maintainable. These principles are fundamental to object-oriented design.

## S - Single Responsibility Principle (SRP)

### Definition
A class should have only one reason to change, meaning it should have only one job or responsibility.

### In This Project
```csharp
// GOOD: Each class has a single responsibility
public class Employee : Person
{
    // Responsibility: Manage employee data and behavior
}

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    // Responsibility: Handle employee data persistence
}

public class Department
{
    // Responsibility: Manage department data and employee aggregation
}
```

### Anti-Pattern (Violation)
```csharp
// BAD: Employee class doing too much
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }

    // Wrong: Employee shouldn't handle its own persistence
    public void SaveToDatabase() { }

    // Wrong: Employee shouldn't format its own display
    public string GeneratePayslipHtml() { }

    // Wrong: Employee shouldn't send emails
    public void SendWelcomeEmail() { }
}
```

### Benefits
- Easier to understand and maintain
- Changes to one responsibility don't affect others
- Easier to test

---

## O - Open/Closed Principle (OCP)

### Definition
Software entities should be open for extension but closed for modification.

### In This Project
```csharp
// Base abstraction
public abstract class Person
{
    public abstract string GetDisplayInfo();
}

// Extended without modifying Person
public class Employee : Person
{
    public override string GetDisplayInfo()
    {
        return $"Employee: {Name} ({EmployeeId})";
    }
}

// Can add new person types without changing existing code
public class Manager : Employee
{
    public List<Employee> DirectReports { get; set; }

    public override string GetDisplayInfo()
    {
        return $"Manager: {Name} ({EmployeeId}) - Reports: {DirectReports.Count}";
    }
}
```

### Interface Example
```csharp
// Open for extension via interface
public interface IEmployeeRepository
{
    void Add(Employee employee);
    // ... other methods
}

// Can create new implementations without modifying interface
public class InMemoryEmployeeRepository : IEmployeeRepository { }
public class SqlEmployeeRepository : IEmployeeRepository { }
public class MongoEmployeeRepository : IEmployeeRepository { }
```

### Benefits
- Add new functionality without breaking existing code
- Reduce risk of introducing bugs
- Promotes code reuse

---

## L - Liskov Substitution Principle (LSP)

### Definition
Objects of a superclass should be replaceable with objects of a subclass without breaking the application.

### In This Project
```csharp
// Base class
public abstract class Person
{
    protected Person(string name, string email)
    {
        // Validation ensures all derived classes have valid state
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        Name = name;
        Email = email;
    }

    public abstract string GetDisplayInfo();
}

// Derived class maintains contract
public class Employee : Person
{
    public Employee(string employeeId, string name, string email, ...)
        : base(name, email)  // Calls base constructor, maintains invariants
    {
        // Additional validation for Employee-specific properties
    }

    public override string GetDisplayInfo()
    {
        // Returns string as expected, doesn't throw unexpected exceptions
        return $"Employee: {Name} ({EmployeeId})";
    }
}

// Usage: Can substitute Employee for Person
Person person = new Employee("E001", "John", "john@email.com", DateTime.Now, 50000m);
string info = person.GetDisplayInfo();  // Works correctly
```

### Anti-Pattern (Violation)
```csharp
// BAD: Derived class violates base class contract
public class ContractWorker : Person
{
    public override string GetDisplayInfo()
    {
        // Violation: Throws exception instead of returning string
        throw new NotImplementedException("Contract workers don't have display info");
    }
}
```

### Benefits
- Ensures derived classes honor the contract of base classes
- Makes code predictable and reliable
- Enables polymorphism without surprises

---

## I - Interface Segregation Principle (ISP)

### Definition
Clients should not be forced to depend on interfaces they don't use. Many specific interfaces are better than one general-purpose interface.

### In This Project
```csharp
// GOOD: Focused interfaces
public interface IEmployeeRepository
{
    void Add(Employee employee);
    void Remove(string employeeId);
    Employee? GetById(string employeeId);
    IEnumerable<Employee> GetAll();
}

public interface IDepartmentRepository
{
    void Add(Department department);
    void Remove(string departmentId);
    Department? GetById(string departmentId);
    IEnumerable<Department> GetAll();
}
```

### Anti-Pattern (Violation)
```csharp
// BAD: Bloated interface forcing unnecessary implementation
public interface IMegaRepository
{
    // Employee methods
    void AddEmployee(Employee employee);
    void RemoveEmployee(string id);

    // Department methods
    void AddDepartment(Department dept);
    void RemoveDepartment(string id);

    // Payroll methods
    void ProcessPayroll();
    void GeneratePayslip();

    // Reporting methods
    void GenerateReport();
    void ExportToExcel();
}

// Classes forced to implement irrelevant methods
public class SimpleEmployeeStore : IMegaRepository
{
    public void AddEmployee(Employee e) { /* implement */ }

    // Forced to implement, but not needed
    public void ProcessPayroll() { throw new NotImplementedException(); }
    public void GenerateReport() { throw new NotImplementedException(); }
    // ... more unused methods
}
```

### Better Approach
```csharp
// GOOD: Split into focused interfaces
public interface IEmployeeRepository { }
public interface IPayrollProcessor { }
public interface IReportGenerator { }

// Implement only what's needed
public class EmployeeStore : IEmployeeRepository { }
public class PayrollService : IPayrollProcessor { }
public class ReportService : IReportGenerator, IEmployeeRepository { }
```

### Benefits
- Classes implement only relevant methods
- Easier to understand and maintain
- More flexible composition

---

## D - Dependency Inversion Principle (DIP)

### Definition
High-level modules should not depend on low-level modules. Both should depend on abstractions. Abstractions should not depend on details. Details should depend on abstractions.

### In This Project
```csharp
// High-level abstraction
public interface IEmployeeRepository
{
    void Add(Employee employee);
    Employee? GetById(string employeeId);
}

// Low-level implementation
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private Dictionary<string, Employee> _employees;

    public void Add(Employee employee)
    {
        _employees[employee.EmployeeId] = employee;
    }

    public Employee? GetById(string employeeId)
    {
        _employees.TryGetValue(employeeId, out var employee);
        return employee;
    }
}

// High-level service depends on abstraction, not concrete class
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    // Dependency injection: depends on interface
    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public void RegisterEmployee(Employee employee)
    {
        // Business logic
        _repository.Add(employee);
    }
}

// Usage: Can swap implementations
var service1 = new EmployeeService(new InMemoryEmployeeRepository());
var service2 = new EmployeeService(new SqlEmployeeRepository());
var service3 = new EmployeeService(new MockEmployeeRepository());  // For testing
```

### Anti-Pattern (Violation)
```csharp
// BAD: High-level class depends on concrete implementation
public class EmployeeService
{
    private InMemoryEmployeeRepository _repository;

    public EmployeeService()
    {
        // Tightly coupled to concrete class
        _repository = new InMemoryEmployeeRepository();
    }

    public void RegisterEmployee(Employee employee)
    {
        _repository.Add(employee);
    }
}

// Problems:
// - Can't swap to different repository
// - Hard to test (can't mock)
// - Changes to InMemoryEmployeeRepository affect EmployeeService
```

### Benefits
- Loose coupling between modules
- Easy to swap implementations
- Testability (can inject mocks)
- Flexibility and extensibility

---

## SOLID in Action: Real Example

### Without SOLID
```csharp
// Violates multiple SOLID principles
public class EmployeeManager
{
    private List<Employee> _employees = new List<Employee>();

    public void AddEmployee(string name, decimal salary)
    {
        var emp = new Employee { Name = name, Salary = salary };
        _employees.Add(emp);

        // SRP violation: Managing database connection
        using (var connection = new SqlConnection("..."))
        {
            connection.Open();
            var command = new SqlCommand("INSERT INTO Employees...", connection);
            command.ExecuteNonQuery();
        }

        // SRP violation: Sending email
        var smtp = new SmtpClient();
        smtp.Send("welcome@company.com", emp.Email, "Welcome!", "...");

        // SRP violation: Logging
        File.AppendAllText("log.txt", $"Added employee {name}");
    }

    // OCP violation: Need to modify class to add new employee type
    // DIP violation: Depends on concrete SqlConnection
    // ISP violation: If this were an interface, it would force too many responsibilities
}
```

### With SOLID
```csharp
// Single Responsibility: Each class has one job
public class EmployeeService
{
    private readonly IEmployeeRepository _repository;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    // Dependency Inversion: Depends on abstractions
    public EmployeeService(
        IEmployeeRepository repository,
        IEmailService emailService,
        ILogger logger)
    {
        _repository = repository;
        _emailService = emailService;
        _logger = logger;
    }

    public void RegisterEmployee(Employee employee)
    {
        _repository.Add(employee);
        _emailService.SendWelcomeEmail(employee);
        _logger.Log($"Added employee {employee.Name}");
    }
}

// Open/Closed: Can extend via inheritance without modification
public interface IEmployeeRepository { }
public class InMemoryEmployeeRepository : IEmployeeRepository { }
public class SqlEmployeeRepository : IEmployeeRepository { }

// Interface Segregation: Focused interfaces
public interface IEmailService
{
    void SendWelcomeEmail(Employee employee);
}

public interface ILogger
{
    void Log(string message);
}
```

---

## Summary

| Principle | Key Question | Benefit |
|-----------|-------------|---------|
| **Single Responsibility** | Does this class have only one reason to change? | Easier maintenance |
| **Open/Closed** | Can I add features without modifying existing code? | Less risk, more extensibility |
| **Liskov Substitution** | Can I replace parent with child without issues? | Reliable polymorphism |
| **Interface Segregation** | Am I forced to implement methods I don't need? | Cleaner interfaces |
| **Dependency Inversion** | Do I depend on abstractions or concrete classes? | Loose coupling, testability |

## Applying SOLID

1. **Start with SRP**: One class, one responsibility
2. **Define interfaces (ISP, DIP)**: Code to abstractions
3. **Enable extension (OCP)**: Use inheritance and interfaces
4. **Honor contracts (LSP)**: Derived classes must work like base classes
5. **Inject dependencies (DIP)**: Constructor injection of interfaces

SOLID principles work together to create maintainable, flexible, and testable code.
