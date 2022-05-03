using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthService.Identity
{
    public class AuthDbContextDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var sentinelDbConnectionString = "host=authdb;database=AuthDb;username=SentinelUser;password=1mW@tch1ngY0u;";
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>()
                 .UseNpgsql(sentinelDbConnectionString);

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}