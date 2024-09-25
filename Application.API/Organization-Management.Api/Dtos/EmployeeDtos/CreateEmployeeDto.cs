namespace Organization_Management.Api.Dtos.EmployeeDtos
{
    public class CreateEmployeeDto
    {
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OrganizationId { get; set; }
    }
}
