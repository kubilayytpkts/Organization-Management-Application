using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Organization_Management_Application.UI.Dtos.EmployeeDtos;
using Organization_Management_Application.UI.Dtos.OrganizationDtos;

namespace Organization_Management_Application.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient client;
        public EmployeeController(IHttpClientFactory _httpClientFactory)
        {
            client = _httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseEmployess = await client.GetAsync("https://localhost:7000/api/Employee/GetEmployeesWithOrganizations");
            if (responseEmployess.IsSuccessStatusCode)
            {
                var jsonEmployess = await responseEmployess.Content.ReadAsStringAsync();
                var deserializeEmployess = JsonConvert.DeserializeObject<List<ResultEmployeesWithOrganizationsDto>>(jsonEmployess);
                var sortedEmployees = deserializeEmployess.OrderBy(x=>x.EmployeeNumber).ToList();
                
                return View(sortedEmployees);
            }
            return View();
        }

        [HttpGet]
        public async  Task<IActionResult> AddEmployee()
        {
            ViewBag.Organizations = await GetOrganizationsSelectedListAsync(1);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(CreateEmployeeDto employeeDto)
        {
            bool flag = false;

            var resultAddEmployee =await client.PostAsJsonAsync("https://localhost:7000/api/Employee", employeeDto);
            if (resultAddEmployee.IsSuccessStatusCode)
                flag = true;

            return Json(new { success =flag});
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesByOrganizationId(int id)
        {
            var responseEmployees = await client.GetAsync($"https://localhost:7000/api/Employee/GetEmployeesByOrganizationId/{id}");
            if (responseEmployees.IsSuccessStatusCode)
            {
                var jsonEmployees = await responseEmployees.Content.ReadAsStringAsync();
                var deserializeEmployees = JsonConvert.DeserializeObject<List<ResultEmployeeDto>>(jsonEmployees);

                return View(deserializeEmployees);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int id)
        {
            var resultEmployee = await client.GetAsync($"https://localhost:7000/api/Employee/{id}");
            if (resultEmployee.IsSuccessStatusCode)
            {
                var jsonEmployee = await resultEmployee.Content.ReadAsStringAsync();
                var deserializeEmploye = JsonConvert.DeserializeObject<ResultEmployeeDto>(jsonEmployee);

                ViewBag.Organizations = await GetOrganizationsSelectedListAsync(deserializeEmploye.OrganizationId);
                return View(deserializeEmploye);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeDto updateEmployee)
        {
            bool flag = false;
            var resultUpdateEmployee = await client.PutAsJsonAsync("https://localhost:7000/api/Employee", updateEmployee);
            if (resultUpdateEmployee.IsSuccessStatusCode)
                flag = true;

            return Json(new { success = flag });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            bool flag = false;
            var resultDeleteEmployee = await client.DeleteAsync($"https://localhost:7000/api/Employee/{id}");
            if (resultDeleteEmployee.IsSuccessStatusCode)
                flag = true;

            return Json(new {success=flag});
        }


        //HELPER METHOD
        private async Task<List<SelectListItem>> GetOrganizationsSelectedListAsync(int? organizationId)
        {
            var resultOrganizations = await client.GetAsync("https://localhost:7000/api/Organization");
            var jsonOrganizations = await resultOrganizations.Content.ReadAsStringAsync();

            var deserializeOrganizations = JsonConvert.DeserializeObject<List<ResultOrganizationDto>>(jsonOrganizations);

            var selectList = (from x in deserializeOrganizations
                              select new SelectListItem
                              {
                                  Text = x.OrganizationName,
                                  Value = x.OrganizationId.ToString(),
                                  Selected = x.ParentOrganizationId == organizationId
                              }).ToList();
            return selectList;
        }
    }
}
