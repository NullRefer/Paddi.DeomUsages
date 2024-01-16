using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Services.IServices;

public interface IDictService : IAppService
{
    Task<AppResult<Dict?>> CreateAsync(DictDto input, CancellationToken cancellationToken = default);

    Task<AppResult<Dict?>> UpdateAsync(long id, DictDto input, CancellationToken cancellationToken = default);

    Task<AppResult<Dict?>> GetAsync(long id, CancellationToken cancellationToken = default);

    Task<AppResult<int>> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
