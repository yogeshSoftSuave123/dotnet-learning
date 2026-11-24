# Day 06: ASP.NET Core and REST API Basics

## Overview
This project demonstrates fundamental ASP.NET Core Web API concepts including:
- RESTful CRUD endpoints
- Controllers, routing, and action methods
- Model binding and DTOs
- JSON serialization
- HTTP verbs and proper status codes
- Swagger/OpenAPI documentation

## Domain Model
Employee Management System with the following entity:

**Employee**
- Id (int)
- FirstName (string)
- LastName (string)
- Email (string)
- PhoneNumber (string, optional)
- Department (string)
- Position (string)
- Salary (decimal)
- HireDate (DateTime)
- IsActive (bool)

## API Endpoints

### GET /api/employees
Retrieve all employees.
- **Response**: 200 OK with array of employee DTOs

### GET /api/employees/{id}
Retrieve a specific employee by ID.
- **Response**: 200 OK with employee DTO
- **Response**: 404 Not Found if employee doesn't exist

### POST /api/employees
Create a new employee.
- **Request Body**: EmployeeCreateDto
- **Response**: 201 Created with employee DTO and Location header
- **Response**: 400 Bad Request if validation fails

### PUT /api/employees/{id}
Update an existing employee.
- **Request Body**: EmployeeUpdateDto
- **Response**: 204 No Content on success
- **Response**: 400 Bad Request if validation fails
- **Response**: 404 Not Found if employee doesn't exist

### DELETE /api/employees/{id}
Delete an employee.
- **Response**: 204 No Content on success
- **Response**: 404 Not Found if employee doesn't exist

## HTTP Status Codes Used

| Status Code | Meaning | When Used |
|-------------|---------|-----------|
| 200 OK | Request successful | GET requests that return data |
| 201 Created | Resource created successfully | POST requests that create new resources |
| 204 No Content | Request successful, no content to return | PUT and DELETE operations |
| 400 Bad Request | Invalid request data | Validation failures, duplicate emails |
| 404 Not Found | Resource not found | GET, PUT, DELETE on non-existent ID |

## Request Flow
1. HTTP request arrives at Kestrel web server
2. Routing middleware matches URL to controller action
3. Model binding populates action parameters from request
4. Data annotations validate the model
5. Controller action executes business logic
6. Result is serialized to JSON and returned with appropriate status code

## Running the API

```bash
cd Day06.BasicAPI
dotnet restore
dotnet build
dotnet run
```

The API will start on:
- HTTPS: https://localhost:7000
- HTTP: http://localhost:5000

## Testing the API

### Using Swagger UI
Navigate to https://localhost:7000 (Swagger UI is served at the root in development mode)

### Using HTTP File
Open `Day06-API-Tests.http` in Visual Studio Code with the REST Client extension, or use the requests as templates for other HTTP clients.

### Using curl
```bash
# Get all employees
curl -X GET https://localhost:7000/api/employees

# Get employee by ID
curl -X GET https://localhost:7000/api/employees/1

# Create new employee
curl -X POST https://localhost:7000/api/employees \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Alice",
    "lastName": "Johnson",
    "email": "alice@company.com",
    "department": "Marketing",
    "position": "Manager",
    "salary": 85000
  }'

# Update employee
curl -X PUT https://localhost:7000/api/employees/1 \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@company.com",
    "phoneNumber": "+1234567890",
    "department": "Engineering",
    "position": "Senior Developer",
    "salary": 100000,
    "isActive": true
  }'

# Delete employee
curl -X DELETE https://localhost:7000/api/employees/1
```

## Running Tests

```bash
cd ../Day06.Tests
dotnet test
```

## Validation Rules

### EmployeeCreateDto / EmployeeUpdateDto
- **FirstName**: Required, 2-50 characters
- **LastName**: Required, 2-50 characters
- **Email**: Required, valid email format
- **PhoneNumber**: Optional, must be valid phone format
- **Department**: Required, max 100 characters
- **Position**: Required, max 100 characters
- **Salary**: Must be between 0 and 10,000,000

## Interview Questions & Answers

### What happens when a request reaches the API?
1. Request arrives at Kestrel web server
2. Middleware pipeline processes the request (logging, routing, authentication, etc.)
3. Routing middleware matches the URL and HTTP verb to a controller action
4. Model binding extracts data from the request (route parameters, query string, body)
5. Model validation runs using Data Annotations
6. If validation passes, the controller action executes
7. The action returns an IActionResult
8. MVC middleware converts the result to an HTTP response
9. Response flows back through middleware pipeline
10. Kestrel sends the HTTP response to the client

### PUT vs PATCH?
- **PUT**: Complete replacement of a resource. Client must send all fields. Idempotent.
- **PATCH**: Partial update of a resource. Client sends only fields to update. Can be idempotent depending on implementation.
- In this project, PUT is used for full updates where all fields must be provided.

### When would you return 201, 400, 404 or 204?
- **201 Created**: After successfully creating a new resource (POST). Includes Location header pointing to the new resource.
- **400 Bad Request**: When client sends invalid data (validation failure, malformed JSON, business rule violation).
- **404 Not Found**: When requested resource doesn't exist (GET/PUT/DELETE by non-existent ID).
- **204 No Content**: After successful operations that don't return data (PUT, DELETE, PATCH).

## Project Structure
```
Day06.BasicAPI/
├── Controllers/
│   └── EmployeesController.cs    # REST API endpoints
├── DTOs/
│   ├── EmployeeDto.cs            # Response DTO
│   ├── EmployeeCreateDto.cs      # Create request DTO
│   └── EmployeeUpdateDto.cs      # Update request DTO
├── Models/
│   └── Employee.cs               # Domain entity
├── Program.cs                     # Application startup
├── Day06-API-Tests.http          # HTTP test file
└── README.md                      # This file

Day06.Tests/
└── EmployeeValidationTests.cs    # Unit tests for validation
```

## Key Concepts Demonstrated

### 1. RESTful Conventions
- Resource-based URLs (/api/employees)
- Proper HTTP verbs (GET, POST, PUT, DELETE)
- Appropriate status codes
- JSON as content type

### 2. DTOs (Data Transfer Objects)
- Separation between domain models and API contracts
- Different DTOs for create, update, and read operations
- Computed properties (FullName)

### 3. Model Binding
- FromBody attribute for request body
- Route parameters ({id})
- Automatic JSON deserialization

### 4. Validation
- Data Annotations for declarative validation
- ModelState.IsValid check
- Custom business logic validation (duplicate email)

### 5. OpenAPI/Swagger
- Automatic API documentation
- XML comments for detailed descriptions
- Try-it-out functionality
- Schema generation

## Next Steps (Day 07)
- Add dependency injection
- Implement custom middleware
- Add logging with ILogger
- Configuration management
- Service lifetimes
