using Employee_Management.Api.Services.Employees;
using Microsoft.EntityFrameworkCore;
using Organization_Management.Api.Concrete.Dapper;
using Organization_Management.Api.Concrete.EfDbContext;
using Organization_Management.Api.Services.Employees;
using Organization_Management.Api.Services.Organizations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<EfContext>();

// EfContext'i Scoped olarak ekleyin ve connection string'i kullanýn.
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
