using Microsoft.EntityFrameworkCore;
using Organization_Management.Api.Entites;

namespace Organization_Management.Api.Concrete.EfDbContext
{
    public class EfContext:DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EfContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Organization> Organization { get; set; }
    }
}
