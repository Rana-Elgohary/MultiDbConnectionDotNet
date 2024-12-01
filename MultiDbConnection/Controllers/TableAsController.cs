using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Models;
using MultiDbConnection.Services;

namespace MultiDbConnection.Controllers
{
    [Route("api/with-domain/[controller]")]
    [ApiController]
    public class TableAsController : ControllerBase
    {
        private readonly DbContextFactoryService _dbContextFactory;

        public TableAsController(DbContextFactoryService dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [HttpGet("get-tablea-data")]
        public async Task<ActionResult<IEnumerable<TableA>>> GetTableAData()
        {
            try
            {
                // Use the factory service to create the DbContext
                using (var dbContext = _dbContextFactory.CreateOneDbContext(HttpContext))
                {
                    // Fetch data from TableAs
                    var data = await dbContext.TableAs.ToListAsync();

                    if (data == null || !data.Any())
                    {
                        return NotFound("No data found in TableAs.");
                    }

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
