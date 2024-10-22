﻿using Organization_Management.Api.Concrete.Dapper;
using Organization_Management.Api.Dtos.OrganizationDtos;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Organization_Management.Api.Entites;
using Microsoft.AspNetCore.Http.Metadata;
using Organization_Management.Api.Dtos.EmployeeDtos;


namespace Organization_Management.Api.Services.Organizations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DapperContext _dapperContext;

        public OrganizationRepository( DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> CreateOrganizationAsync(CreateOrganizationDto organizationDto)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = @"INSERT INTO Organization(OrganizationName,ParentOrganizationId) VALUES (@OrganizationName,@ParentOrganizationId)";
                var result = await connection.ExecuteAsync(sql, organizationDto);

                return result == 1;
            }
        }

        public async Task<bool> DeleteOrganizationAsync(int id)
        {
            if(await DeleteOrganizationControlAsync(id))
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    string sql = "DELETE FROM Organization WHERE OrganizationId=@Id";
                    var result = await connection.ExecuteAsync(sql, new { Id = id });

                    return result == 1;
                }
            }
            return false;
        }

        public async Task<IEnumerable<ResultOrganizationDto>> GetAllOrganizationsAsync()
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = "SELECT * FROM Organization";
                var resultOrganization = await connection.QueryAsync<ResultOrganizationDto>(sql);

                return resultOrganization;
            }
        }

        public async Task<ResultOrganizationDto> GetOrganizationByIdAsync(int id)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = @"SELECT * FROM Organization WHERE OrganizationId = @OrganizationId";
                var resultOrganization = await connection.QueryFirstOrDefaultAsync<ResultOrganizationDto>(sql, new { OrganizationId = id });

                return resultOrganization;
            }
        }

        public async Task<bool> UpdateOrganizationAsync(UpdateOrganizationDto organizationDto)
        {
            
            using (var connection = _dapperContext.CreateConnection())
            {
                string sql = @"UPDATE Organization 
                          SET  OrganizationName = @OrganizationName, ParentOrganizationId = @ParentOrganizationId  WHERE OrganizationId=@OrganizationId";
                var result = await connection.ExecuteAsync(sql, organizationDto);

                return result == 1;
            }
        }

        private async Task<bool> DeleteOrganizationControlAsync(int organizationId)
        {
            bool flag = true;
            using (var connection = _dapperContext.CreateConnection())
            {
                string organizationSql = "SELECT * FROM Organization WHERE ParentOrganizationId = @ParentOrganizationId";
                var resultOrganizations = await connection.QuerySingleOrDefaultAsync<ResultOrganizationDto>(organizationSql, new { ParentOrganizationId = organizationId });

                string employeeSql = "SELECT * FROM Employee WHERE OrganizationId = @OrganizationId";
                var resultEmployees = await connection.QuerySingleOrDefaultAsync<ResultEmployeeDto>(employeeSql, new { OrganizationId = organizationId });

                if (resultOrganizations != null || resultEmployees != null)
                    flag = false; // If there is an organization or employee relationship you cannot delete it.
            }

            return flag;
        }
    }
}
