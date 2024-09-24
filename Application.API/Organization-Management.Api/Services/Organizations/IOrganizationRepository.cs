using Organization_Management.Api.Dtos.OrganizationDtos;

namespace Organization_Management.Api.Services.Organizations
{
    public interface IOrganizationRepository
    {
        public Task<bool> CreateOrganizationAsync(CreateOrganizationDto organizationDto);
        public Task<bool> UpdateOrganizationAsync(UpdateOrganizationDto organizationDto);
        public Task<bool> DeleteOrganizationAsync(int id);
        public Task<IEnumerable<ResultOrganizationDto>> GetAllOrganizationsAsync();
        public Task<ResultOrganizationDto> GetOrganizationByIdAsync(int id);

    }
}
