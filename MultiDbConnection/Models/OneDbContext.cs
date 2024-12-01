using Microsoft.EntityFrameworkCore;

namespace MultiDbConnection.Models
{
    public class OneDbContext: DbContext
    {
        public OneDbContext(DbContextOptions<OneDbContext> options) : base(options) { }

        public DbSet<TableA> TableAs { get; set; }
        public DbSet<TableB> TableBs { get; set; }
    }
}
