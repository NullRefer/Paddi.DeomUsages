using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Dtos.Dict;
using Paddi.DemoUsages.ApiDemo.Entities;

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

    public async Task<Dict> CreateAsync(DictDto input, CancellationToken cancellationToken = default)
    {
        var exists = await _repo.Where(e => e.Key == input.Key).AnyAsync(cancellationToken);
        if (exists)
        {
            throw new ArgumentException($"{input.Key} already exists", nameof(input)); 
        }

        var dict = new Dict
        {
            Key = input.Key,
            Value = input.Value
        };
        await _repo.CreateAsync(dict, cancellationToken);
        _logger.LogInformation("Insert dict - {@Entity}", dict);
        return dict;
    }

    public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        return entity == null ? -1 : await _repo.DeleteAsync(entity);
    }

    public Task<long> BatchDeleteAsync(List<long> idList, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async Task<Dict?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var data = await _repo.Where(e => e.Key.Contains(id.ToString()) && !e.IsDeleted).ToListAsync(cancellationToken: cancellationToken);
        var exists = data.Find(d => d.Id == id);
        return exists;
    }

    public async Task<Dict?> GetCategoryAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        var data = await _repo.Where(e => e.Key == "Category" && !e.IsDeleted).ToListAsync(cancellationToken);
        return data.Find(e => e.Value == categoryName);
    }

    public async Task<Dict?> UpdateAsync(long id, DictDto input, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        entity.Key = input.Key;
        entity.Value = input.Value;
        await _repo.UpdateAsync(entity, cancellationToken);
        return entity;
    }
}
