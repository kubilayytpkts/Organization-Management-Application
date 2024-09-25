namespace Organization_Management.Api.Entites
{
    public class Employee
    {
        public int EmployeeId { get; set; } 
        public int EmployeeNumber { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public int OrganizationId { get; set; } 
        public virtual Organization Organization { get; set; } // İlişkili organizasyon
    }
}
