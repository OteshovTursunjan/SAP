using Microsoft.AspNetCore.Mvc;
using SAP.Application.Model.Employees;
using SAP.Application.Service;

namespace SAP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeController : ControllerBase // Изменено с Controller на ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeController(IEmployeeService employee)
        {
            _employeeService = employee;
        }

        [HttpPost("CreateEmployee")]
        [ProducesResponseType(typeof(EmployeeResponseModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeModel createEmployeeModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var res = await _employeeService.CreateEmployee(createEmployeeModel);

                // Возвращаем 201 Created вместо 200 OK
                return StatusCode(StatusCodes.Status201Created, res);

                // Альтернативный вариант с Created:
                // return Created($"/api/employe/{res.EmployeeID}", res);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("SAP session expired or invalid");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("duplicate") || ex.Message.Contains("already exists"))
            {
                return Conflict("Employee already exists");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error occurred while creating employee");
            }
        }

        // Дополнительно: метод для получения сотрудника (если нужен)
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _employeeService.GetEmployeeById(id);
            return Ok(res);
        }
        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetAllEmployee()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _employeeService.GetAllEmployees();
            return Ok(res);
        }
        [HttpDelete("DeleteEmployees{id}")]
        [ProducesResponseType(typeof(EmployeeResponseModel), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _employeeService.DeleteEmployees(id);
            return Ok(res);
        }
    }
}