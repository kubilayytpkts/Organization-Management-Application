namespace Organization_Management_Application.UI.Dtos.OrganizationDtos

{
    public class CreateOrganizationDto
    {
        public string OrganizationName { get; set; }
        public int? ParentOrganizationId { get; set; }
    }
}
