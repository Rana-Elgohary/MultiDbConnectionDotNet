using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Models;

namespace MultiDbConnection.Services
{
    public class DynamicDatabaseService
    {
        private readonly DbContext_Multi _dbContext_Multi;

        public DynamicDatabaseService(DbContext_Multi mainDbContext)
        {
            _dbContext_Multi = mainDbContext;
        }

        public async Task AddDomainAndSetupDatabase(string domainName)
        {
            // Create the connection string dynamically
            string connectionString = $"Data Source=.;Initial Catalog={domainName};Integrated Security=True; TrustServerCertificate=True";

            // Add the domain to the Domain table
            var domain = new Domain { Route = domainName, ConnectionString = connectionString };
            _dbContext_Multi.Domains.Add(domain);
            await _dbContext_Multi.SaveChangesAsync();

            // Create the new database and apply migrations
            var optionsBuilder = new DbContextOptionsBuilder<OneDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var dbContext = new OneDbContext(optionsBuilder.Options))
            {
                // Create the database if it doesn't exist
                bool databaseExists = await dbContext.Database.CanConnectAsync();
                if (!databaseExists)
                {
                    await dbContext.Database.EnsureCreatedAsync();  // Only create the database if it doesn't exist
                }
                else
                {
                    await dbContext.Database.MigrateAsync();  // This will apply pending migrations only
                }
            }
        }
    }
}
