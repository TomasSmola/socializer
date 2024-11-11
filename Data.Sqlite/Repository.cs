using socializer.Data.Domain.Interfaces;

namespace socializer.Data.Sqlite;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext dbContext;

    public Repository(DataContext context)
    {
        this.dbContext = context;
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task Delete(TEntity entity, CancellationToken cancellationToken)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAllAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken)
    {
        var entities = dbContext.Set<TEntity>().Where(predicate);

        dbContext.Set<TEntity>().RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<TEntity> GetAll()
    {
        return dbContext.Set<TEntity>();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
