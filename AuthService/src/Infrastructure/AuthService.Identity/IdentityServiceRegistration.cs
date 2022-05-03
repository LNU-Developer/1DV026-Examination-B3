using AuthService.Application.Contracts.Identity;
using AuthService.Application.Models.Identity;
using AuthService.Identity.Models;
using AuthService.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            var authDbConnectionString = Environment.GetEnvironmentVariable("AUTHDB_CONNECTIONSTRING") is not null ? Environment.GetEnvironmentVariable("AUTHDB_CONNECTIONSTRING") : null;
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddDbContext<AuthDbContext>(option =>
             {
                 option.UseNpgsql(authDbConnectionString);
                 // .LogTo(Console.WriteLine) //Enable logging in Console.
                 // .EnableSensitiveDataLogging()
                 // .EnableDetailedErrors()
             });

            var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
            if (runMigrations is not null)
            {
                Console.WriteLine("Running migrations");
                var optionBuilder = new DbContextOptionsBuilder<AuthDbContext>();
                optionBuilder.UseNpgsql(authDbConnectionString);
                using var authContext = new AuthDbContext(optionBuilder.Options);
                authContext.Database.Migrate();
            }
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}