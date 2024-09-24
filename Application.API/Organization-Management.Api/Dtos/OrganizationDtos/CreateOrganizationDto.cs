namespace Organization_Management.Api.Dtos.OrganizationDtos
{
    public class CreateOrganizationDto
    {
        public string OrganizationName { get; set; }
        public int? ParentOrganizationId { get; set; }
    }
}
