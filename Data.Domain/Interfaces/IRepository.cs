
namespace socializer.Data.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken);

        Task Delete(TEntity entity, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task DeleteAllAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken);
    }
}
