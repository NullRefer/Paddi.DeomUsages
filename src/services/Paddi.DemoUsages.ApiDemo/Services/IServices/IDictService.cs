using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Services.IServices;

public interface IDictService : IAppService
{
    Task<Dict> CreateAsync(DictDto input, CancellationToken cancellationToken = default);

    Task<Dict?> UpdateAsync(long id, DictDto input, CancellationToken cancellationToken = default);

    Task<long> BatchDeleteAsync(List<long> idList, CancellationToken cancellationToken = default);

    Task<Dict?> GetAsync(long id, CancellationToken cancellationToken = default);

    Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
