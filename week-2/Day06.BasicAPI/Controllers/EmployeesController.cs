using Microsoft.AspNetCore.Mvc;
using Day06.BasicAPI.Models;
using Day06.BasicAPI.DTOs;

namespace Day06.BasicAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private static readonly List<Employee> _employees = new()
    {
        new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@company.com",
            PhoneNumber = "+1234567890",
            Department = "Engineering",
            Position = "Senior Developer",
            Salary = 95000,
            HireDate = new DateTime(2020, 1, 15),
            IsActive = true
        },
        new Employee
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@company.com",
            PhoneNumber = "+1234567891",
            Department = "HR",
            Position = "HR Manager",
            Salary = 85000,
            HireDate = new DateTime(2019, 6, 10),
            IsActive = true
        }
    };

    private static int _nextId = 3;

    /// <summary>
    /// Get all employees
    /// </summary>
    /// <returns>List of all employees</returns>
    /// <response code="200">Returns the list of employees</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<EmployeeDto>> GetAll()
    {
        var employeeDtos = _employees.Select(MapToDto);
        return Ok(employeeDtos);
    }

    /// <summary>
    /// Get a specific employee by ID
    /// </summary>
    /// <param name="id">The employee ID</param>
    /// <returns>The employee with the specified ID</returns>
    /// <response code="200">Returns the requested employee</response>
    /// <response code="404">If the employee is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EmployeeDto> GetById(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        return Ok(MapToDto(employee));
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    /// <param name="createDto">Employee creation data</param>
    /// <returns>The newly created employee</returns>
    /// <response code="201">Returns the newly created employee</response>
    /// <response code="400">If the request data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EmployeeDto> Create([FromBody] EmployeeCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for duplicate email
        if (_employees.Any(e => e.Email.Equals(createDto.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { message = "An employee with this email already exists" });
        }

        var employee = new Employee
        {
            Id = _nextId++,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PhoneNumber = createDto.PhoneNumber,
            Department = createDto.Department,
            Position = createDto.Position,
            Salary = createDto.Salary,
            HireDate = createDto.HireDate ?? DateTime.UtcNow,
            IsActive = true
        };

        _employees.Add(employee);

        var employeeDto = MapToDto(employee);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employeeDto);
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    /// <param name="id">The employee ID</param>
    /// <param name="updateDto">Employee update data</param>
    /// <returns>No content</returns>
    /// <response code="204">If the employee was successfully updated</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="404">If the employee is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] EmployeeUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employee = _employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        // Check for duplicate email (excluding current employee)
        if (_employees.Any(e => e.Id != id && e.Email.Equals(updateDto.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { message = "An employee with this email already exists" });
        }

        employee.FirstName = updateDto.FirstName;
        employee.LastName = updateDto.LastName;
        employee.Email = updateDto.Email;
        employee.PhoneNumber = updateDto.PhoneNumber;
        employee.Department = updateDto.Department;
        employee.Position = updateDto.Position;
        employee.Salary = updateDto.Salary;
        employee.IsActive = updateDto.IsActive;

        return NoContent();
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    /// <param name="id">The employee ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If the employee was successfully deleted</response>
    /// <response code="404">If the employee is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var employee = _employees.FirstOrDefault(e => e.Id == id);

        if (employee == null)
        {
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        _employees.Remove(employee);
        return NoContent();
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            Department = employee.Department,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive
        };
    }
}
