# Day 1: C# Fundamentals - Employee Management Console App

## Overview
This console application demonstrates C# fundamentals including variables, types, methods, collections, and exception handling through an employee management system.

## Run Instructions

### Prerequisites
- .NET SDK 8.0 or later installed
- Terminal/Command Prompt

### Build and Run
```bash
cd Day1.ConsoleApp
dotnet build
dotnet run
```

## Sample Inputs and Outputs

### Sample 1: Adding Employees
**Input:**
```csharp
manager.AddEmployee(new Employee(1, "John Doe", "Engineering", 75000m));
manager.AddEmployee(new Employee(2, "Jane Smith", "HR", 65000m));
manager.AddEmployee(new Employee(3, "Bob Johnson", "Engineering", 80000m));
```

**Output:**
```
Added: John Doe
Added: Jane Smith
Added: Bob Johnson
Total Employees: 3
ID: 1, Name: John Doe, Department: Engineering, Salary: $75000.00
ID: 2, Name: Jane Smith, Department: HR, Salary: $65000.00
ID: 3, Name: Bob Johnson, Department: Engineering, Salary: $80000.00
```

### Sample 2: Filter by Department
**Input:**
```csharp
var engineers = manager.GetEmployeesByDepartment("Engineering");
```

**Output:**
```
ID: 1, Name: John Doe, Department: Engineering, Salary: $75000.00
ID: 3, Name: Bob Johnson, Department: Engineering, Salary: $80000.00
```

### Sample 3: Average Salary Calculation
**Input:**
```csharp
decimal avgSalary = manager.CalculateAverageSalary();
```

**Output:**
```
Average Salary: $73333.33
```

## Value vs Reference Types Explanation

### Value Types Used
1. **int (Id)**: Value type stored on the stack. When assigned, the value is copied.
   ```csharp
   int id = 1;  // Stored on stack, direct value
   ```

2. **decimal (Salary)**: Value type for precise financial calculations. Avoids floating-point rounding errors.
   ```csharp
   decimal salary = 75000m;  // 'm' suffix indicates decimal literal
   ```

3. **bool (implicit in conditions)**: Value type for true/false logic.

### Reference Types Used
1. **string (Name, Department)**: Reference type, but immutable. Points to heap memory.
   ```csharp
   string name = "John Doe";  // Reference to string object on heap
   ```

2. **List<Employee>**: Reference type collection. The list itself is on the heap, containing references to Employee objects.
   ```csharp
   List<Employee> employees = new List<Employee>();  // Reference on stack, object on heap
   ```

3. **Employee class**: Reference type. Instances stored on heap, variables hold references.
   ```csharp
   Employee emp = new Employee(...);  // emp is reference, object is on heap
   ```

### Nullable Reference Types
The project enables nullable reference types (`<Nullable>enable</Nullable>`):
```csharp
Employee? FindEmployeeById(int id)  // ? indicates method can return null
```

### Key Differences
- **Value types** (int, decimal, bool, struct): Stored on stack, copied by value, no null (unless `Nullable<T>` or `T?`)
- **Reference types** (class, string, arrays, List): Stored on heap, copied by reference, can be null
- **String** is special: reference type but immutable (modifications create new objects)

### Memory Example
```csharp
int x = 10;           // Stack: x = 10
int y = x;            // Stack: y = 10 (copy of value)
x = 20;               // Stack: x = 20, y still = 10

Employee emp1 = new Employee(1, "John", "IT", 50000m);
// Stack: emp1 = reference (memory address)
// Heap: Employee object with Id=1, Name="John", etc.

Employee emp2 = emp1;  // Stack: emp2 = same reference as emp1
emp2.Salary = 60000m;  // Changes the same object, emp1.Salary is now also 60000m
```

## Exception Handling
The application includes validation and exception handling:
- `ArgumentNullException` for null parameters
- `ArgumentException` for invalid values (negative salary, empty department)
- Try-catch blocks can be added for user input scenarios

## Features Demonstrated
- Variable declarations and initialization
- Value types (int, decimal) vs Reference types (class, string, List)
- Methods with parameters and return types
- Collections (List<T>)
- LINQ methods (Where, FirstOrDefault, Average)
- String manipulation and comparison
- Exception handling with validation
- Nullable reference types
