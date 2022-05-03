using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ResourceService.Persistence
{
    public class ResourceDbContextFactory : IDesignTimeDbContextFactory<ResourceDbContext>
    {
        public ResourceDbContext CreateDbContext(string[] args)
        {
            var sentinelDbConnectionString = "host=resourcedb;database=ResourceDb;username=ResourceUser;password=1G0tY0u;";
            var optionsBuilder = new DbContextOptionsBuilder<ResourceDbContext>()
                 .UseNpgsql(sentinelDbConnectionString);

            return new ResourceDbContext(optionsBuilder.Options);
        }
    }
}