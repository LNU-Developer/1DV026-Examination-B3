using ResourceService.Application.Contracts.Persistence.Entity;
using ResourceService.Domain.Entities;
using ResourceService.Persistence.Repositories.Common;

namespace ResourceService.Persistence.Repositories.Entity
{
    public class ImageRepository : Repository<ResourceDbContext, Image>, IImageRepository
    {
        public ImageRepository(ResourceDbContext context) : base(context)
        {
        }

    }
}
