using AutoMapper;
using Employee_Management.Api.Services.Employees;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organization_Management.Api.Concrete.EfDbContext;
using Organization_Management.Api.Dtos.EmployeeDtos;
using Organization_Management.Api.Dtos.OrganizationDtos;
using Organization_Management.Api.Entites;
using System.Text.Json.Serialization;

namespace Organization_Management.Api.Services.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EfContext _efContext;
        private readonly IMapper _mapper;
        private readonly HttpClient client;

        public EmployeeRepository(EfContext efContext, IMapper mapper, IHttpClientFactory _httpClientFactory)
        {
            _efContext = efContext;
            _mapper = mapper;
            client = _httpClientFactory.CreateClient();
        }

        public async Task<bool> CreateEmployeeAsync(CreateEmployeeDto employeeDto)
        {
            if(await ValidateNewEmployeeNumber(employeeDto.EmployeeNumber))
            {
                var mappedEmployee = _mapper.Map<Employee>(employeeDto);
                await _efContext.Set<Employee>().AddAsync(mappedEmployee);

                var result = await _efContext.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _efContext.Employee.FindAsync(id);
            if (employee == null)
            {
                return false;
            }

            _efContext.Employee.Remove(employee);
            var result = await _efContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<ResultEmployeeDto>> GetAllEmployeesAsync()
        {
            var resultEmployees = await _efContext.Set<Employee>().ToListAsync();
            var mappedEmployees = _mapper.Map<List<ResultEmployeeDto>>(resultEmployees);

            return mappedEmployees;
        }

        public async Task<ResultEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var resultEmployee = await _efContext.Set<Employee>().FindAsync(id);
            var mappedEmployee = _mapper.Map<ResultEmployeeDto>(resultEmployee);

            return mappedEmployee;
        }

        public async Task<List<ResultEmployeeDto>> GetEmployeesByOrganizationId(int id)
        {
            var resultEmployees = await _efContext.Set<Employee>().Where(x => x.OrganizationId == id).ToListAsync();
            var mappedEmployees = _mapper.Map<List<ResultEmployeeDto>>(resultEmployees);

            return mappedEmployees;
        }

        public async Task<List<ResultEmployeeWithOrganizationDto>> GetEmployessWithOrganizations()
        {
            var employeeList = await GetAllEmployeesAsync();

            var mappedEmployeeList = _mapper.Map<List<ResultEmployeeWithOrganizationDto>>(employeeList);
            var organizationList = new List<ResultOrganizationDto>();

            var responseOrganizations = await client.GetAsync("https://localhost:7000/api/Organization");
            if (responseOrganizations.IsSuccessStatusCode)
            {
                var jsonOrganization = await responseOrganizations.Content.ReadAsStringAsync();
                organizationList = JsonConvert.DeserializeObject<List<ResultOrganizationDto>>(jsonOrganization);
            }

            foreach (var employee in mappedEmployeeList)
            {
                foreach (var organization in organizationList)
                {
                    if(employee.OrganizationId == organization.OrganizationId)
                    {
                        employee.organization = new ResultOrganizationDto();
                        employee.organization.OrganizationId = organization.OrganizationId;
                        employee.organization.OrganizationName = organization.OrganizationName;
                    }
                    continue;
                }
            }

            return mappedEmployeeList;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto employeeDto)
        {
            var existingEmployee = await _efContext.Employee.FindAsync(employeeDto.EmployeeId);

            if (existingEmployee == null)
            {

                return false;
            }

            _mapper.Map(employeeDto, existingEmployee);
            var result = await _efContext.SaveChangesAsync();

            return result > 0;
        }

        private async Task<bool> ValidateNewEmployeeNumber(int employeeNumber)
        {
            var employees =await _efContext.Employee.FirstOrDefaultAsync(x => x.EmployeeNumber == employeeNumber);
            //If the new employee number is registered, we cannot register it.

            return employees == null;
        }
    }
}
