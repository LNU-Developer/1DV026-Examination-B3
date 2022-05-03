namespace ResourceService.Application.Contracts.Persistence.Entity
{
    public interface IUnitOfWork
    {
        IImageRepository Images { get; }
        Task<int> CompleteAsync();
        int Complete();
    }
}