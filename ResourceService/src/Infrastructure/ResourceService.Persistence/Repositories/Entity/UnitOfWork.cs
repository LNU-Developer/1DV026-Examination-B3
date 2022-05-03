using ResourceService.Application.Contracts.Persistence.Entity;

namespace ResourceService.Persistence.Repositories.Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ResourceDbContext _context;

        public UnitOfWork(ResourceDbContext context)
        {
            _context = context;
            Images = new ImageRepository(_context); ;
        }

        public IImageRepository Images { get; private set; }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}