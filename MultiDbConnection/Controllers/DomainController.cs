using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Models;
using MultiDbConnection.Services;

namespace MultiDbConnection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly DbContext_Multi _dbContext;
        private readonly DynamicDatabaseService _dynamicDatabaseService;

        public DomainController(DynamicDatabaseService dynamicDatabaseService, DbContext_Multi dbContext)
        {
            _dbContext = dbContext;
            _dynamicDatabaseService = dynamicDatabaseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDomain(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return BadRequest("Invalid domain name.");
            }

            var existingDomain = await _dbContext.Domains.FirstOrDefaultAsync(d => d.Route == route);
            if (existingDomain != null)
            {
                return Conflict("Domain already exists.");
            }

            // Use the service to add the domain and setup the corresponding database
            await _dynamicDatabaseService.AddDomainAndSetupDatabase(route);

            return Ok(new { message = "Domain and database setup successfully." });
        }
    }
}
