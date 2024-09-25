using Organization_Management.Api.Dtos.OrganizationDtos;
using Organization_Management.Api.Entites;

namespace Organization_Management.Api.Dtos.EmployeeDtos
{
    public class ResultEmployeeWithOrganizationDto
    {
        public int EmployeeId { get; set; }
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrganizationId { get; set; }
        public ResultOrganizationDto organization { get; set; }
    }
}
