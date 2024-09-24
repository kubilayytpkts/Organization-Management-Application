using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization_Management.Api.Dtos.OrganizationDtos;
using Organization_Management.Api.Services.Organizations;

namespace Organization_Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDto organizationDto)
        {
            var result = await _organizationRepository.CreateOrganizationAsync(organizationDto);

            return result == true ? Ok() : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrganization(UpdateOrganizationDto OrganizationDto)
        {
            var result = await _organizationRepository.UpdateOrganizationAsync(OrganizationDto);

            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(int OrganizationId)
        {
            var result = await _organizationRepository.DeleteOrganizationAsync(OrganizationId);

            return result == true ? NoContent() : BadRequest(result);
        }

        [HttpGet("{OrganizationId}")]
        public async Task<IActionResult> GetByIdOrganization(int OrganizationId)
        {
            var result = await _organizationRepository.GetOrganizationByIdAsync(OrganizationId);

            return result != null ? Ok(result) : NotFound();
        }

        //[HttpGet("GetOrganizationsByCategoryId/{categoryId}")]
        //public async Task<IActionResult> GetOrganizationsByCategoryId(int categoryId)
        //{
        //    var resultOrganizationList = await _organizationRepository.GetOrganizationsByCategoryId(categoryId);
        //    return Ok(resultOrganizationList);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllOrganization()
        {
            var result = await _organizationRepository.GetAllOrganizationsAsync();

            return result != null ? Ok(result) : BadRequest("İşlem başarısız");
        }

        //[HttpGet("GetOrganizationsWithCategory")]
        //public async Task<IActionResult> GetOrganizationsWithCategory()
        //{
        //    var resultOrganizationsWithCaregory = await _organizationRepository.GetOrganizationsWithCategory();
        //    return Ok(resultOrganizationsWithCaregory);
        //}
    }
}
