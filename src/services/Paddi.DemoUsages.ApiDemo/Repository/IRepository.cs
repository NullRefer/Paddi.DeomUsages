using Paddi.DemoUsages.ApiDemo.Entities;

namespace Paddi.DemoUsages.ApiDemo.Repository;

public interface IRepository { }

public interface IRepository<T> : IRepository where T : class, IPaddiEntity
{
    Task<T?> GetAsync(long id, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool tracking = false);
}
