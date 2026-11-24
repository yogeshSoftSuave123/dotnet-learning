using Microsoft.AspNetCore.Mvc;
using Day07.DI.Middleware.Models;
using Day07.DI.Middleware.Services;

namespace Day07.DI.Middleware.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeesController> _logger;
    private readonly IGuidGeneratorService _guidGenerator1;
    private readonly IGuidGeneratorService _guidGenerator2;

    // Constructor injection - all dependencies injected by DI container
    public EmployeesController(
        IEmployeeService employeeService,
        ILogger<EmployeesController> logger,
        IGuidGeneratorService guidGenerator1,
        IGuidGeneratorService guidGenerator2)
    {
        _employeeService = employeeService;
        _logger = logger;
        _guidGenerator1 = guidGenerator1;
        _guidGenerator2 = guidGenerator2;

        // Log transient service instances to demonstrate they're different
        _logger.LogDebug("Controller created with GuidGenerator1: {Id1}, GuidGenerator2: {Id2}",
            _guidGenerator1.ServiceInstanceId, _guidGenerator2.ServiceInstanceId);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
    {
        using (_logger.BeginScope(new Dictionary<string, object> {
            ["Action"] = "GetAllEmployees",
            ["RequestId"] = HttpContext.Items["RequestId"] ?? Guid.NewGuid()
        }))
        {
            _logger.LogInformation("Getting all employees");
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        _logger.LogInformation("Getting employee {EmployeeId}", id);
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            _logger.LogWarning("Employee {EmployeeId} not found", id);
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        return Ok(employee);
    }

    [HttpGet("department/{department}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetByDepartment(string department)
    {
        _logger.LogInformation("Getting employees in department {Department}", department);
        var employees = await _employeeService.GetEmployeesByDepartmentAsync(department);
        return Ok(employees);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Create([FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _employeeService.CreateEmployeeAsync(employee);
            _logger.LogInformation("Created employee {EmployeeId}", created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid employee data");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _employeeService.UpdateEmployeeAsync(id, employee);

        if (!success)
        {
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _employeeService.DeleteEmployeeAsync(id);

        if (!success)
        {
            return NotFound(new { message = $"Employee with ID {id} not found" });
        }

        return NoContent();
    }
}
