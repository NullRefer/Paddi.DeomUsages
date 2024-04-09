using Microsoft.EntityFrameworkCore;

namespace Paddi.DemoUsages.ApiDemo.Services;

public class DictService
{
    private readonly ApiDemoDbContext _dbContext;
    private readonly ILogger<DictService> _logger;

    public DictService(ApiDemoDbContext dbContext, ILogger<DictService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Dict> CreateAsync(DictDto input, CancellationToken cancellationToken = default)
    {
        var dict = new Dict
        {
            Key = input.Key,
            Value = input.Value
        };
        await _dbContext.AddAsync(dict, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Insert dict - {@Entity}", dict);
        return dict;
    }

    public async Task<long> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var dict = await _dbContext.Set<Dict>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        return dict == null ? -1 : await _dbContext.SoftDeleteAsync(dict);
    }

    public async Task<long> BatchDeleteAsync(List<long> idList, CancellationToken cancellationToken = default) => await _dbContext.Set<Dict>().Where(e => idList.Contains(e.Id)).ExecuteUpdateAsync(e => e.SetProperty(e => e.IsDeleted, true), cancellationToken: cancellationToken);

    public async Task<Dict?> GetAsync(long id, CancellationToken cancellationToken = default) => await _dbContext.Set<Dict>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);

    public async Task<Dict?> UpdateAsync(long id, DictDto input, CancellationToken cancellationToken = default)
    {
        var dict = await _dbContext.Set<Dict>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        if (dict == null) return null;

        dict.Key = input.Key;
        dict.Value = input.Value;
        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Update dict - {@Entity}", dict);
        return dict;
    }
}
