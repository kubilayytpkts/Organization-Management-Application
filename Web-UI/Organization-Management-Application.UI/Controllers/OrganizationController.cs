using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Organization_Management_Application.UI.Dtos.OrganizationDtos;
using Organization_Management_Application.UI.Models;

namespace Organization_Management_Application.UI.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly HttpClient client;
        public OrganizationController(IHttpClientFactory _httpClientFactory)
        {
            client = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseOrganizations = await client.GetAsync("https://localhost:7000/api/Organization");
            if (responseOrganizations.IsSuccessStatusCode)
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
        [HttpGet]
        public async Task<IActionResult> CreateOrganization()
        {
            //Default value 1 
            ViewBag.Organizations = await Get_OrganizationsSelectList(1);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDto organizationDto)
        {
            bool flag = false;
            var responseCreateOrganization = await client.PostAsJsonAsync("https://localhost:7000/api/Organization", organizationDto);
            if(responseCreateOrganization.IsSuccessStatusCode)
                flag = true;

            return Json(new { success = flag });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrganization(int id)
        {
            var resonseGetByIdOrganization = await client.GetAsync($"https://localhost:7000/api/Organization/{id}");
            if (resonseGetByIdOrganization.IsSuccessStatusCode)
            {
                var jsonData = await resonseGetByIdOrganization.Content.ReadAsStringAsync();
                var deserializeData = JsonConvert.DeserializeObject<ResultOrganizationDto>(jsonData);

                ViewBag.Organizations = await Get_OrganizationsSelectList(deserializeData.ParentOrganizationId);

                return View(deserializeData);
            }
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrganization(UpdateOrganizationDto organizationDto)
        {
            bool flag = false;
            var responseUpdateOrganization = await client.PutAsJsonAsync("https://localhost:7000/api/Organization", organizationDto);
            if (responseUpdateOrganization.IsSuccessStatusCode)
            {
                flag = true;
            }
            return Json(new { success = flag });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            bool flag = false;
            var responseDeleteOrganization =await client.DeleteAsync($"https://localhost:7000/api/Organization/{id}/");
            if(responseDeleteOrganization.IsSuccessStatusCode)
            {
                flag = true;
            }
            return Json(new { success = flag });
        }

        //HELPERS METHODS
        private async Task<List<SelectListItem>> Get_OrganizationsSelectList(int? parentOrganizationId)
        {
            var organizations = await Get_OrganizationsAsync();

            var selectList = organizations.Select(x => new SelectListItem
            {
                Value = x.OrganizationId.ToString(),
                Text = x.OrganizationName,
                Selected = x.ParentOrganizationId == parentOrganizationId
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
