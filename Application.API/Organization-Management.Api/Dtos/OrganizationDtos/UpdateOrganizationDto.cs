namespace Organization_Management.Api.Dtos.OrganizationDtos
{
    public class UpdateOrganizationDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? ParentOrganizationId { get; set; }
    }
}
