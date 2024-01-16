using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Dtos.Dict;
using Paddi.DemoUsages.ApiDemo.Repository;

namespace Paddi.DemoUsages.ApiDemo.Services;

public class DictRepoService : IDictService
{
    private readonly IRepository<Dict> _repo;
    private readonly ILogger<DictRepoService> _logger;

    public DictRepoService(IRepository<Dict> repo, ILogger<DictRepoService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<AppResult<Dict?>> CreateAsync(DictDto input, CancellationToken cancellationToken = default)
    {
        var exists = await _repo.Where(e => e.Key == input.Key).AnyAsync(cancellationToken);
        if (exists)
        {
            return AppResult<Dict?>.Ng($"{input.Key} already exists");
        }

        var dict = new Dict
        {
            Key = input.Key,
            Value = input.Value
        };
        _ = await _repo.CreateAsync(dict, cancellationToken);
        _logger.LogInformation("Insert dict - {@Entity}", dict);
        return AppResult<Dict?>.Ok(dict);
    }

    public async Task<AppResult<int>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetAsync(id, cancellationToken);
        if (entity == null)
        {
            return AppResult<int>.Ng($"Not found with id - {id}");
        }
        var result = await _repo.DeleteAsync(entity, cancellationToken);
        _logger.LogInformation("Delete entity - {Id}", entity.Id);
        return AppResult.Ok(result);
    }

    public async Task<AppResult<Dict?>> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var data = await _repo.GetAsync(id, cancellationToken);
        return AppResult<Dict?>.Ok(data);
    }

    public async Task<AppResult<Dict?>> UpdateAsync(long id, DictDto input, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetAsync(id, cancellationToken);
        if (entity == null)
        {
            return AppResult<Dict?>.Ng($"Not found with id - {id}");
        }

        entity.Key = input.Key;
        entity.Value = input.Value;
        await _repo.UpdateAsync(entity, cancellationToken);
        return AppResult<Dict?>.Ok(entity);
    }
}
