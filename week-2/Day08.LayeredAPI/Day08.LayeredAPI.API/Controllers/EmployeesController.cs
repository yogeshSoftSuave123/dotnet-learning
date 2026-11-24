using Microsoft.AspNetCore.Mvc;
using Day08.LayeredAPI.Application.DTOs;
using Day08.LayeredAPI.Application.Interfaces;

namespace Day08.LayeredAPI.API.Controllers;

/// <summary>
/// Employee API controller
/// Thin controller that delegates to service layer
/// Handles HTTP concerns (routing, status codes, model validation)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeService employeeService,
        ILogger<EmployeesController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get employee by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        // Exception handling is done by global middleware
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }

    /// <summary>
    /// Get employees by department
    /// </summary>
    [HttpGet("department/{departmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetByDepartment(int departmentId)
    {
        var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
        return Ok(employees);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] EmployeeCreateDto createDto)
    {
        // Model state validation happens automatically
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employee = await _employeeService.CreateEmployeeAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] EmployeeUpdateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employee = await _employeeService.UpdateEmployeeAsync(id, updateDto);
        return Ok(employee);
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}
