using EmployeeManagementService_Backend.Domain.Models;
using EmployeeManagementService_Backend.AppLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementService_Backend.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Employee>>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null,
            [FromQuery] string? email = null,
            [FromQuery] string? position = null)
        {
            var result = await _employeeService.GetAllAsync(pageNumber, pageSize, searchTerm, firstName, lastName, email, position);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _employeeService.AddAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Update(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updated = await _employeeService.UpdateAsync(id, employee);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _employeeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
} 