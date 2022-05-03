using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Persistence.Repositories.Entity;

namespace ResourceService.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var resourceDbConnectionString = Environment.GetEnvironmentVariable("RESOURCEDB_CONNECTIONSTRING") is not null ? Environment.GetEnvironmentVariable("RESOURCEDB_CONNECTIONSTRING") : configuration["ConnectionStrings:Resource"];
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<ResourceDbContext>(option =>
            {
                option.UseNpgsql(resourceDbConnectionString);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
            if (runMigrations is not null)
            {
                Console.WriteLine("Running migrations");
                var optionBuilder = new DbContextOptionsBuilder<ResourceDbContext>();
                optionBuilder.UseNpgsql(resourceDbConnectionString);
                using var context = new ResourceDbContext(optionBuilder.Options);
                context.Database.Migrate();
            }

            return services;
        }
    }
}