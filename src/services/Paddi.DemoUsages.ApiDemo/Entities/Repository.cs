namespace Paddi.DemoUsages.ApiDemo.Entities;

public interface IRepository { }

public interface IRepository<T> : IRepository where T : class, IPaddiEntity
{
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate);
}

public class Repository<T> : IRepository<T> where T : class, IPaddiEntity
{
    private readonly ApiDemoDbContext _dbContext;

    public Repository(ApiDemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        return await dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<int> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        await dbSet.AddAsync(entity, cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        dbSet.Update(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        dbSet.Remove(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<T> GetAll() => _dbContext.Set<T>().AsQueryable();

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate) => _dbContext.Set<T>().Where(predicate);
}
