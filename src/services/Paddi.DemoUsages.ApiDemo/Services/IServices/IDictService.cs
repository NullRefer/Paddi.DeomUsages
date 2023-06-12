using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Services.IServices;

public interface IDictService : IAppService
{
    Task<Dict> CreateAsync(DictDto input);

    Task<Dict?> UpdateAsync(long id, DictDto input);

    Task<Dict?> GetAsync(long id);

    Task<long> DeleteAsync(long id);
}
