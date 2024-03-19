using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo.Repository;

public class Repository<T> : IRepository<T> where T : class, IPaddiEntity
{
    private readonly ApiDemoDbContext _dbContext;

    public Repository(ApiDemoDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<T?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        return await dbSet.FindAsync(new object?[] { id }, cancellationToken);
    }

    public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        dbSet.Remove(entity);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var dbSet = _dbContext.Set<T>();
        var entity = await dbSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return -1;
        }
        dbSet.Remove(entity);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<T> GetAll() => _dbContext.Set<T>().AsQueryable();

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracking = false) => tracking ? _dbContext.Set<T>().Where(predicate) : _dbContext.Set<T>().Where(predicate).AsNoTracking();
}
