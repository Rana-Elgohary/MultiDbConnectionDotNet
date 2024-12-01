
using Microsoft.EntityFrameworkCore;
using MultiDbConnection.Middleware;
using MultiDbConnection.Models;
using MultiDbConnection.Services;

namespace MultiDbConnection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string txt = "AllowAllOrigins";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DbContext_Multi>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

            builder.Services.AddScoped<DynamicDatabaseService>();
            builder.Services.AddScoped<DbContextFactoryService>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy(txt, builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/with-domain"), appBuilder =>
            {
                appBuilder.UseCors(txt);
                appBuilder.UseMiddleware<ConnectionStringMiddleware>();
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(txt);
            app.UseAuthorization();

            
            app.MapControllers();

            app.Run();
        }
    }
}
