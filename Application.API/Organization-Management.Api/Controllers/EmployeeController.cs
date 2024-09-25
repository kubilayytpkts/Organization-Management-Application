using Employee_Management.Api.Services.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization_Management.Api.Dtos.EmployeeDtos;

namespace Organization_Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDto employee)
        {
            var result = await _employeeRepository.CreateEmployeeAsync(employee);

            return result == true ? Ok() : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDto employee)
        {
            var result = await _employeeRepository.UpdateEmployeeAsync(employee);

            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeRepository.DeleteEmployeeAsync(id);

            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdEmployee(int id)
        {
            var resultEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return Ok(resultEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var resultEmployess = await _employeeRepository.GetAllEmployeesAsync(); 
            return Ok(resultEmployess);
        }

        [HttpGet("GetEmployeesByOrganizationId/{organizationId}")]
        public async Task<IActionResult> GetEmployeesByOrganizationId(int organizationId)
        {
            var resultEmployees = await _employeeRepository.GetEmployeesByOrganizationId(organizationId);
            return Ok(resultEmployees);
        }

        [HttpGet("GetEmployeesWithOrganizations")]
        public async Task<IActionResult> GetEmployeesWithOrganizations()
        {
            var resultEmployees =await _employeeRepository.GetEmployessWithOrganizations();
            
            return Ok(resultEmployees);
        }

    }
}
