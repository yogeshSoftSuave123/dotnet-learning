# Day 3: Modern C# and LINQ Exercises

## Overview
This project demonstrates LINQ (Language Integrated Query) capabilities including filtering, grouping, sorting, aggregation, and deferred execution with in-memory collections.

## Run Instructions

```bash
cd Day3.LinqExercises
dotnet build
dotnet run
```

## LINQ Queries Summary

### Query 1: Filter by Salary
**Purpose**: Filter employees earning more than $80,000

**Input**: List of 10 employees

**LINQ Methods Used**: `Where`, `ToList`

**Code**:
```csharp
var highEarners = employees.Where(e => e.Salary > 80000).ToList();
```

**Output**: 6 employees

**Expected Complexity**: O(n) - Linear scan through collection

---

### Query 2: Select (Projection)
**Purpose**: Project specific fields and calculate years of service

**Input**: List of 10 employees

**LINQ Methods Used**: `Select`, `ToList`

**Code**:
```csharp
var projection = employees.Select(e => new
{
    e.Name,
    e.Department,
    YearsOfService = (DateTime.Now - e.HireDate).Days / 365
}).ToList();
```

**Output**: 10 records with Name, Department, and YearsOfService

**Expected Complexity**: O(n) - Transform each element

---

### Query 3: Group By Department
**Purpose**: Group employees by department and calculate statistics

**Input**: List of 10 employees

**LINQ Methods Used**: `GroupBy`, `Count`, `Average`, `ToList`

**Code**:
```csharp
var grouped = employees.GroupBy(e => e.Department).ToList();
foreach (var group in grouped)
{
    Console.WriteLine($"Department: {group.Key}");
    Console.WriteLine($"Count: {group.Count()}");
    Console.WriteLine($"Avg Salary: ${group.Average(e => e.Salary):N2}");
}
```

**Output**: 5 department groups with counts and average salaries
- Engineering: 4 employees
- HR: 2 employees
- Sales: 2 employees
- Marketing: 2 employees

**Expected Complexity**: O(n) - Group operation

---

### Query 4: Order By Salary (Descending)
**Purpose**: Sort employees by salary (highest first), then by name

**Input**: List of 10 employees

**LINQ Methods Used**: `OrderByDescending`, `ThenBy`, `ToList`

**Code**:
```csharp
var sorted = employees.OrderByDescending(e => e.Salary)
                     .ThenBy(e => e.Name)
                     .ToList();
```

**Output**: 10 employees sorted by salary descending

**Expected Complexity**: O(n log n) - Sorting operation (typically quicksort)

---

### Query 5: Aggregate Operations
**Purpose**: Calculate various statistics (sum, average, min, max, count)

**Input**: List of 10 employees

**LINQ Methods Used**: `Count`, `Sum`, `Average`, `Min`, `Max`

**Code**:
```csharp
var totalEmployees = employees.Count();
var totalSalary = employees.Sum(e => e.Salary);
var avgSalary = employees.Average(e => e.Salary);
var maxSalary = employees.Max(e => e.Salary);
var minSalary = employees.Min(e => e.Salary);
```

**Output**:
- Total Employees: 10
- Total Salary Budget: $840,000
- Average Salary: $84,000
- Highest Salary: $120,000
- Lowest Salary: $65,000

**Expected Complexity**: O(n) for each aggregate operation

---

### Query 6: Complex Filtering
**Purpose**: Filter with multiple conditions using boolean logic

**Input**: List of 10 employees

**LINQ Methods Used**: `Where`, `ToList`

**Code**:
```csharp
var filtered = employees.Where(e =>
    (e.Department == "Engineering" || e.Department == "Sales") &&
    e.Salary > 75000 &&
    e.Age < 40
).ToList();
```

**Output**: 3 employees matching all criteria

**Expected Complexity**: O(n) - Single pass with multiple predicates

---

### Query 7: Group By City with Statistics
**Purpose**: Group by city and calculate comprehensive statistics per city

**Input**: List of 10 employees

**LINQ Methods Used**: `GroupBy`, `Select`, `Count`, `Sum`, `Average`, `Distinct`, `OrderByDescending`, `ToList`

**Code**:
```csharp
var cityStats = employees.GroupBy(e => e.City)
    .Select(g => new
    {
        City = g.Key,
        EmployeeCount = g.Count(),
        TotalSalary = g.Sum(e => e.Salary),
        AverageSalary = g.Average(e => e.Salary),
        Departments = g.Select(e => e.Department).Distinct().Count()
    })
    .OrderByDescending(x => x.EmployeeCount)
    .ToList();
```

**Output**: Statistics for 3 cities
- Seattle: 4 employees, $408,000 total, $102,000 avg
- Portland: 3 employees, $207,000 total, $69,000 avg
- Austin: 3 employees, $225,000 total, $75,000 avg

**Expected Complexity**: O(n) - Grouping and aggregation

---

### Query 8: Deferred Execution
**Purpose**: Demonstrate deferred execution (lazy evaluation) in LINQ

**Input**: List of 10 employees (then 11 after adding one)

**LINQ Methods Used**: `Where`, `Select`, `ToList`

**Code**:
```csharp
// Query is defined but not executed
var query = employees.Where(e => e.Department == "Engineering")
                    .Select(e => e.Name);

// Add new employee
employees.Add(new Employee { ... });

// Execute query NOW (includes newly added employee)
var result = query.ToList();
```

**Output**: 5 Engineering employees (includes the newly added one)

**Expected Complexity**: O(n) - Executed only when enumerated

**Key Learning**: Queries are not executed until materialized (ToList, ToArray, foreach, Count, etc.)

---

## Unit-Testable Methods

The following methods are designed to be unit-tested:

### 1. FilterByDepartment(string department)
- **Purpose**: Get all employees in a specific department
- **Returns**: `List<Employee>`
- **Complexity**: O(n)

### 2. GroupAndCountByDepartment()
- **Purpose**: Count employees per department
- **Returns**: `Dictionary<string, int>`
- **Complexity**: O(n)

### 3. CalculateAverageSalaryByDepartment(string department)
- **Purpose**: Calculate average salary for a department
- **Returns**: `decimal`
- **Complexity**: O(n)

### 4. SearchByName(string searchTerm)
- **Purpose**: Find employees by partial name match
- **Returns**: `List<Employee>`
- **Complexity**: O(n)

### 5. GetTopEarners(int count)
- **Purpose**: Get top N highest-paid employees
- **Returns**: `List<Employee>`
- **Complexity**: O(n log n) due to sorting

## Key LINQ Concepts

### Method Syntax vs Query Syntax

**Method Syntax (Used in this project)**:
```csharp
var result = employees.Where(e => e.Salary > 80000)
                     .OrderBy(e => e.Name)
                     .ToList();
```

**Query Syntax (Alternative)**:
```csharp
var result = (from e in employees
              where e.Salary > 80000
              orderby e.Name
              select e).ToList();
```

### Deferred Execution

LINQ queries are **not executed** when defined, only when:
- Calling `.ToList()`, `.ToArray()`, `.ToDictionary()`
- Using `foreach` loop
- Calling aggregates: `.Count()`, `.Sum()`, `.First()`, etc.

**Example**:
```csharp
var query = employees.Where(e => e.Age > 30);  // Not executed
var list = query.ToList();                      // Executed NOW
```

### IEnumerable vs IQueryable

| Feature | IEnumerable<T> | IQueryable<T> |
|---------|----------------|---------------|
| **Target** | In-memory collections | Database queries (EF Core) |
| **Execution** | Client-side (C#) | Server-side (SQL) |
| **Performance** | Good for small data | Efficient for large data |
| **Extension Methods** | `System.Linq.Enumerable` | `System.Linq.Queryable` |

**This project uses IEnumerable** (in-memory operations).

### Lambda Expressions

Lambdas are anonymous functions used in LINQ:

```csharp
// Lambda syntax: (parameter) => expression
e => e.Salary > 80000

// Equivalent to:
bool GreaterThan80k(Employee e)
{
    return e.Salary > 80000;
}

// With multiple parameters:
(x, y) => x + y

// With code block:
e => {
    var bonus = e.Salary * 0.1m;
    return e.Salary + bonus;
}
```

### Delegates

LINQ uses delegates under the hood:

```csharp
// Func<T, TResult> delegate
Func<Employee, bool> predicate = e => e.Salary > 80000;
var result = employees.Where(predicate);

// Predicate<T> (returns bool)
Predicate<Employee> isHighEarner = e => e.Salary > 80000;

// Action<T> (void return)
Action<Employee> printEmployee = e => Console.WriteLine(e.Name);
```

### Extension Methods

LINQ is built on extension methods:

```csharp
public static class MyLinqExtensions
{
    public static IEnumerable<T> WhereExt<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }
}

// Usage
var result = employees.WhereExt(e => e.Salary > 80000);
```

### Common LINQ Methods

| Method | Purpose | Complexity |
|--------|---------|-----------|
| `Where` | Filter elements | O(n) |
| `Select` | Transform/project elements | O(n) |
| `OrderBy` | Sort ascending | O(n log n) |
| `OrderByDescending` | Sort descending | O(n log n) |
| `GroupBy` | Group by key | O(n) |
| `First` | Get first element | O(1) |
| `FirstOrDefault` | Get first or default | O(1) |
| `Single` | Get only element (throws if 0 or >1) | O(n) |
| `Any` | Check if any element matches | O(n) |
| `All` | Check if all elements match | O(n) |
| `Count` | Count elements | O(n) |
| `Sum` | Sum numeric values | O(n) |
| `Average` | Calculate average | O(n) |
| `Min` / `Max` | Find minimum/maximum | O(n) |
| `Take` | Take first N elements | O(n) |
| `Skip` | Skip first N elements | O(n) |
| `Distinct` | Remove duplicates | O(n) |
| `ToList` / `ToArray` | Materialize query | O(n) |

## Generics

LINQ heavily uses generics:

```csharp
// Generic List<T>
List<Employee> employees = new List<Employee>();

// Generic method
public List<T> Filter<T>(List<T> items, Func<T, bool> predicate)
{
    return items.Where(predicate).ToList();
}

// Generic Dictionary<TKey, TValue>
Dictionary<string, int> deptCounts = employees
    .GroupBy(e => e.Department)
    .ToDictionary(g => g.Key, g => g.Count());
```

## List vs Dictionary

| Feature | List<T> | Dictionary<TKey, TValue> |
|---------|---------|--------------------------|
| **Lookup** | O(n) - linear search | O(1) - hash lookup |
| **Add** | O(1) amortized | O(1) amortized |
| **Order** | Maintains insertion order | No guaranteed order |
| **Duplicates** | Allowed | Keys must be unique |
| **Use Case** | Sequential access | Key-value pairs, fast lookup |

**Example**:
```csharp
// List - good for iteration
List<Employee> empList = new List<Employee>();
empList.Add(employee);
var found = empList.FirstOrDefault(e => e.Id == 5);  // O(n)

// Dictionary - good for lookups
Dictionary<int, Employee> empDict = new Dictionary<int, Employee>();
empDict[employee.Id] = employee;
var found = empDict[5];  // O(1)
```

## Pattern Matching

Modern C# supports pattern matching in LINQ:

```csharp
// Switch expression with pattern matching
var salaryCategory = employee.Salary switch
{
    < 60000 => "Entry Level",
    >= 60000 and < 80000 => "Mid Level",
    >= 80000 and < 100000 => "Senior",
    >= 100000 => "Principal",
    _ => "Unknown"
};

// Type pattern
var items = collection.OfType<Employee>();  // Filter by type

// Property pattern
var seattleEngineers = employees.Where(e => e is { City: "Seattle", Department: "Engineering" });
```

## Best Practices

1. **Use meaningful variable names** in lambdas when logic is complex
2. **Prefer method chaining** for readability
3. **Use `Any()` instead of `Count() > 0`** for existence checks
4. **Avoid multiple enumerations** - materialize with `ToList()` if needed multiple times
5. **Use `FirstOrDefault()` instead of `First()`** to avoid exceptions
6. **Be aware of deferred execution** - understand when queries execute

## Performance Tips

1. **Filter early**: Apply `Where` before expensive operations like `Select` or `OrderBy`
2. **Use indexed properties**: Dictionary lookups are O(1) vs List search O(n)
3. **Avoid N+1 queries**: Load related data in one query when possible
4. **Consider ToList() cost**: Only materialize when necessary
5. **Watch for multiple enumerations**: Each enumeration re-executes the query

## Further Reading

- C# LINQ documentation: https://learn.microsoft.com/en-us/dotnet/csharp/linq/
- Lambda expressions: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
- Extension methods: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
