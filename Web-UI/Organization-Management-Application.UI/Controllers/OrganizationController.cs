using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Organization_Management_Application.UI.Dtos.OrganizationDtos;
using Organization_Management_Application.UI.Models;

namespace Organization_Management_Application.UI.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;
        public OrganizationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var responseOrganizations =await client.GetAsync("https://localhost:7000/api/Organization");
            if(responseOrganizations.IsSuccessStatusCode)
            {
                var jsonOrganizations = await responseOrganizations.Content.ReadAsStringAsync();
                var organizations = JsonConvert.DeserializeObject<List<ResultOrganizationDto>>(jsonOrganizations);

                // Parent-child ilişkisini hazırlayın
                var hierarchicalOrganizations = organizations
                    .Where(o => o.ParentOrganizationId == null)
                    .Select(o => new
                    {
                        Id = o.OrganizationId,
                        Name = o.OrganizationName,
                        Children = GetChildren(organizations, o.OrganizationId)
                    }).ToList();

                // ViewBag veya ViewData ile bu yapıyı frontend'e yollayacağız
                ViewBag.OrganizationsTree = JsonConvert.SerializeObject(hierarchicalOrganizations);

            }
            return View();
        }

        public async Task<IActionResult> UpdateOrganization(int id)
        {
            var resonseGetByIdOrganization =await client.GetAsync($"https://localhost:7000/api/Organization/{id}");
            if(resonseGetByIdOrganization.IsSuccessStatusCode)
            {
                var jsonData =await resonseGetByIdOrganization.Content.ReadAsStringAsync();
                var deserializeData = JsonConvert.DeserializeObject<ResultOrganizationDto>(jsonData);

                ViewBag.Organizations =await Get_OrganizationsSelectList(deserializeData.OrganizationId);

                return View(deserializeData);
            }
            return View();
        }

        private async Task<List<SelectListItem>> Get_OrganizationsSelectList(int organizationId)
        {
            var selectList = (from x in await Get_OrganizationsAsync()
                              select new SelectListItem
                              {
                                  Value=x.OrganizationId.ToString(),
                                  Text=x.OrganizationName,
                                  Selected = organizationId == x.OrganizationId
                              }).ToList();

            return selectList;
        }

        private async Task<List<ResultOrganizationDto>> Get_OrganizationsAsync()
        {
            var response = await client.GetAsync("https://localhost:7000/api/Organization");
            if (response.IsSuccessStatusCode)
            {
                var jsonOrganization = await response.Content.ReadAsStringAsync();
                var deserializeOrganization = JsonConvert.DeserializeObject<List<ResultOrganizationDto>>(jsonOrganization);

                return deserializeOrganization;
            }
            return null;
        }

        //HELPER METHOD
        private List<TreeModel> GetChildren(List<ResultOrganizationDto> allOrganizations, int parentId)
        {
            return allOrganizations
                .Where(o => o.ParentOrganizationId == parentId)
                .Select(o => new TreeModel
                {
                    Id = o.OrganizationId,
                    Name = o.OrganizationName,
                    Children = GetChildren(allOrganizations, o.OrganizationId)
                }).ToList();
        }
    }
}
