# Class Relationship Diagram (UML)

## Visual Representation

```
┌─────────────────────────────┐
│      <<abstract>>           │
│         Person              │
├─────────────────────────────┤
│ # Name: string              │
│ # Email: string             │
├─────────────────────────────┤
│ + Person(name, email)       │
│ + UpdateEmail(newEmail)     │
│ + GetDisplayInfo()* string  │
└──────────────┬──────────────┘
               │
               │ inherits (is-a)
               ▼
┌─────────────────────────────┐
│         Employee            │
├─────────────────────────────┤
│ + EmployeeId: string        │
│ + HireDate: DateTime        │
│ + Salary: decimal           │
│ + Department: Department?   │
├─────────────────────────────┤
│ + Employee(...)             │
│ + GetYearsOfService(): int  │
│ + GetDisplayInfo(): string  │
│ + AssignToDepartment(dept)  │
└──────────────┬──────────────┘
               │
               │ belongs to (many-to-one)
               ▼
┌─────────────────────────────┐
│        Department           │
├─────────────────────────────┤
│ + DepartmentId: string      │
│ + Name: string              │
│ + Location: string          │
│ - _employees: List<Employee>│
│ + Employees: IReadOnlyList  │
├─────────────────────────────┤
│ + Department(...)           │
│ + AddEmployee(employee)     │
│ + RemoveEmployee(employee)  │
│ + GetEmployeeCount(): int   │
│ + GetTotalSalaryExpense()   │
└─────────────────────────────┘


┌─────────────────────────────┐
│      <<interface>>          │
│   IEmployeeRepository       │
├─────────────────────────────┤
│ + Add(employee)             │
│ + Remove(employeeId)        │
│ + GetById(id): Employee?    │
│ + GetAll(): IEnumerable     │
│ + GetByDepartment(id)       │
│ + Update(employee)          │
└──────────────┬──────────────┘
               │
               │ implements
               ▼
┌─────────────────────────────┐
│ InMemoryEmployeeRepository  │
├─────────────────────────────┤
│ - _employees: Dictionary    │
├─────────────────────────────┤
│ + Add(employee)             │
│ + Remove(employeeId)        │
│ + GetById(id): Employee?    │
│ + GetAll(): IEnumerable     │
│ + GetByDepartment(id)       │
│ + Update(employee)          │
└─────────────────────────────┘


┌─────────────────────────────┐
│      <<interface>>          │
│   IDepartmentRepository     │
├─────────────────────────────┤
│ + Add(department)           │
│ + Remove(departmentId)      │
│ + GetById(id): Department?  │
│ + GetAll(): IEnumerable     │
│ + Update(department)        │
└──────────────┬──────────────┘
               │
               │ implements
               ▼
┌─────────────────────────────┐
│InMemoryDepartmentRepository │
├─────────────────────────────┤
│ - _departments: Dictionary  │
├─────────────────────────────┤
│ + Add(department)           │
│ + Remove(departmentId)      │
│ + GetById(id): Department?  │
│ + GetAll(): IEnumerable     │
│ + Update(department)        │
└─────────────────────────────┘
```

## Relationship Types

### 1. Inheritance (is-a)
```
Person (abstract)
   ↑
   │ inherits
   │
Employee
```
- **Type**: Generalization/Inheritance
- **Description**: Employee "is-a" Person
- **Implementation**: `public class Employee : Person`
- **Benefits**: Code reuse, polymorphism

### 2. Composition (has-a)
```
Department
   │
   │ contains (1 to many)
   ▼
Employee (collection)
```
- **Type**: Composition/Aggregation
- **Description**: Department "has" Employees
- **Implementation**: `private List<Employee> _employees`
- **Cardinality**: One Department can have zero to many Employees

### 3. Association (uses)
```
Employee ──────► Department
```
- **Type**: Association
- **Description**: Employee is associated with Department
- **Implementation**: `public Department? Department { get; set; }`
- **Cardinality**: One Employee belongs to zero or one Department

### 4. Realization (implements)
```
<<interface>>
IEmployeeRepository
       ↑
       │ implements
       │
InMemoryEmployeeRepository
```
- **Type**: Realization/Implementation
- **Description**: Concrete class implements interface contract
- **Implementation**: `public class InMemoryEmployeeRepository : IEmployeeRepository`

## UML Notation Legend

| Symbol | Meaning |
|--------|---------|
| `+` | Public member |
| `-` | Private member |
| `#` | Protected member |
| `*` | Abstract method |
| `<<interface>>` | Interface stereotype |
| `<<abstract>>` | Abstract class |
| `?` | Nullable type |
| `───►` | Association (uses) |
| `───▷` | Inheritance (is-a) |
| `◆───` | Composition (has-a, strong ownership) |
| `◇───` | Aggregation (has-a, weak ownership) |

## Detailed Relationships

### Person ◄─── Employee (Inheritance)
- **Relationship**: Inheritance/Generalization
- **Multiplicity**: N/A (class-level relationship)
- **Description**: Employee inherits properties and methods from Person
- **Code**:
  ```csharp
  public abstract class Person { }
  public class Employee : Person { }
  ```

### Department ◆──── Employee (Composition)
- **Relationship**: Composition (strong ownership)
- **Multiplicity**: 1 Department to 0..* Employees
- **Description**: Department contains and manages Employees
- **Code**:
  ```csharp
  public class Department
  {
      private List<Employee> _employees;
  }
  ```

### Employee ────► Department (Association)
- **Relationship**: Association (reference)
- **Multiplicity**: 1 Employee to 0..1 Department
- **Description**: Employee references its Department
- **Code**:
  ```csharp
  public class Employee
  {
      public Department? Department { get; set; }
  }
  ```

### IEmployeeRepository ◄─── InMemoryEmployeeRepository (Realization)
- **Relationship**: Realization/Implementation
- **Multiplicity**: N/A (class-level relationship)
- **Description**: Concrete class implements interface contract
- **Code**:
  ```csharp
  public interface IEmployeeRepository { }
  public class InMemoryEmployeeRepository : IEmployeeRepository { }
  ```

## Design Patterns Applied

### 1. Repository Pattern
- **Interfaces**: `IEmployeeRepository`, `IDepartmentRepository`
- **Purpose**: Abstract data access logic
- **Benefits**: Decouples domain from persistence

### 2. Aggregate Root Pattern
- **Class**: `Department`
- **Purpose**: Manages collection of Employees
- **Benefits**: Maintains consistency, encapsulates collection operations

### 3. Value Object Candidates
- Could use for: `Money` (amount + currency), `Address`, `Email`
- **Current**: Using primitive types for simplicity

## Class Hierarchy Tree

```
Object (System.Object)
│
├── Person (abstract)
│   └── Employee
│       └── Manager (future extension)
│
├── Department
│
├── InMemoryEmployeeRepository : IEmployeeRepository
│
└── InMemoryDepartmentRepository : IDepartmentRepository
```

## Cardinality Explanation

### Department ↔ Employee
```
Department 1 ────── 0..* Employee
   │                      │
   │                      │
One department      Zero or many employees
```

**Rules**:
- One Department can have zero to many Employees
- One Employee belongs to zero or one Department
- Department manages the relationship (composition)
- Employee references Department (association)

### Implementation
```csharp
// Department side (composition - owns the relationship)
public class Department
{
    private List<Employee> _employees;

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
        employee.AssignToDepartment(this);
    }
}

// Employee side (association - references Department)
public class Employee
{
    public Department? Department { get; set; }
}
```

## Key Design Decisions

1. **Abstract Person Class**
   - Why: Common properties/behavior for all person types
   - Alternative: Could use interface, but loses code reuse

2. **Private Collection in Department**
   - Why: Encapsulation - control how employees are added/removed
   - Exposes: `IReadOnlyList<Employee>` for safe external access

3. **Interface-Based Repositories**
   - Why: Dependency inversion, testability
   - Allows: Multiple implementations (in-memory, SQL, NoSQL)

4. **Nullable Department Reference**
   - Why: Employee can exist without being assigned to department
   - Type: `Department?` (nullable reference type)

## Extension Points

Future enhancements without breaking existing code:

```csharp
// New person type
public class Manager : Employee
{
    public List<Employee> DirectReports { get; set; }
}

// New repository implementation
public class SqlEmployeeRepository : IEmployeeRepository
{
    // Database implementation
}

// New behavior via interface
public interface ISalaryCalculator
{
    decimal CalculateBonus(Employee employee);
}
```

This design follows SOLID principles and allows easy extension without modification.
