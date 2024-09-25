using AutoMapper;
using Organization_Management.Api.Dtos.EmployeeDtos;
using Organization_Management.Api.Entites;

namespace Organization_Management.Api.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
                CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
                CreateMap<Employee, UpdateEmployeeDto>().ReverseMap();
                CreateMap<Employee, ResultEmployeeDto>().ReverseMap();

            CreateMap<ResultEmployeeWithOrganizationDto, ResultEmployeeDto>().ReverseMap();
        }
    }
}
