using System;

using Microsoft.EntityFrameworkCore;

using Paddi.DemoUsages.ApiDemo.Dtos.Dict;

namespace Paddi.DemoUsages.ApiDemo.Services;

public class DictService : IDictService
{
    private readonly ApiDemoDbContext _dbContext;
    private readonly ILogger<DictService> _logger;

    public DictService(ApiDemoDbContext dbContext, ILogger<DictService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Dict> CreateAsync(DictDto input)
    {
        var dict = new Dict
        {
            Key = input.Key,
            Value = input.Value
        };
        await _dbContext.AddAsync(dict);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Insert dict - {@Entity}", dict);
        return dict;
    }

    public async Task<long> DeleteAsync(long id)
    {
        var dict = await _dbContext.Set<Dict>().FindAsync(id);
        return dict == null ? -1 : await _dbContext.SoftDeleteAsync(dict);
    }

    public async Task<long> BatchDeleteAsync(List<long> idList) => await _dbContext.Set<Dict>().Where(e => idList.Contains(e.Id)).ExecuteUpdateAsync(e => e.SetProperty(e => e.IsDeleted, true));

    public async Task<Dict?> GetAsync(long id) => await _dbContext.Set<Dict>().FindAsync(id);

    public async Task<Dict?> UpdateAsync(long id, DictDto input)
    {
        var dict = await _dbContext.Set<Dict>().FindAsync(id);
        if (dict == null) return null;

        dict.Key = input.Key;
        dict.Value = input.Value;
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Update dict - {@Entity}", dict);
        return dict;
    }
}
