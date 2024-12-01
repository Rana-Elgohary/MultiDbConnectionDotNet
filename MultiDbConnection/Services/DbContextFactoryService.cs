using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Models;

namespace MultiDbConnection.Services
{
    public class DbContextFactoryService
    {
        public OneDbContext CreateOneDbContext(HttpContext httpContext)
        {
            // Retrieve the connection string from HttpContext
            var connectionString = httpContext.Items["ConnectionString"] as string;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string not found in HttpContext.");
            }

            // Create a new DbContext with the dynamic connection string
            var optionsBuilder = new DbContextOptionsBuilder<OneDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OneDbContext(optionsBuilder.Options);
        }
    }
}
