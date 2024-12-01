using Microsoft.EntityFrameworkCore;

namespace MultiDbConnection.Models
{
    public class DbContext_Multi : DbContext
    {
        public DbSet<Domain> Domains { get; set; }

        public DbContext_Multi(DbContextOptions<DbContext_Multi> options) : base(options) { }
    }
}
