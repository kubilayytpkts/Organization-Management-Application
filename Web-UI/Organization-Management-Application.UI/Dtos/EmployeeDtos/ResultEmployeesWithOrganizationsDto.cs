using Organization_Management_Application.UI.Dtos.OrganizationDtos;

namespace Organization_Management_Application.UI.Dtos.EmployeeDtos
{
    public class ResultEmployeesWithOrganizationsDto
    {
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrganizationId { get; set; }
        public ResultOrganizationDto Organization { get; set; }
    }
}
