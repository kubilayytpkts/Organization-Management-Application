﻿namespace Organization_Management_Application.UI.Dtos.OrganizationDtos
{
    public class ResultOrganizationDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? ParentOrganizationId { get; set; }
    }
}
