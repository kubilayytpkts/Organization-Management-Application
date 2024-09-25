using Organization_Management.Api.Dtos.EmployeeDtos;

namespace Employee_Management.Api.Services.Employees
{
    public interface IEmployeeRepository
    {
        public Task<bool> CreateEmployeeAsync(CreateEmployeeDto employeeDto);
        public Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto EmployeeDto);
        public Task<bool> DeleteEmployeeAsync(int id);
        public Task<List<ResultEmployeeDto>> GetAllEmployeesAsync();
        public Task<ResultEmployeeDto> GetEmployeeByIdAsync(int id);
        public Task<List<ResultEmployeeDto>> GetEmployeesByOrganizationId(int id);
        public Task<List<ResultEmployeeWithOrganizationDto>> GetEmployessWithOrganizations();
    }
}
