using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Models;

namespace MultiDbConnection.Middleware
{
    public class ConnectionStringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public ConnectionStringMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract domain name from a specific header, e.g., "X-Domain-Name"
            var domainName = context.Request.Headers["DomainName"].FirstOrDefault();

            if (string.IsNullOrEmpty(domainName))
            {
                // Handle missing or invalid header
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Domain name header is missing or invalid.");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext_Multi>();

                // Fetch the connection string for the given domain name
                var domain = await dbContext.Domains.FirstOrDefaultAsync(d => d.Route == domainName);

                if (domain != null)
                {
                    // Store connection string in HttpContext.Items
                    context.Items["ConnectionString"] = domain.ConnectionString;
                }
                else
                {
                    // Handle missing domain in the database
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Domain not found in the database.");
                    return;
                }
            }

            // Continue processing the request
            await _next(context);
        }
    }
}
