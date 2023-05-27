using Microsoft.Extensions.Options;

using Paddi.DemoUsages.ApiDemo.Cache;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController]
[Route("sys")]
public class SysController : ControllerBase
{
    [HttpGet("redis-option")]
    public ResultDto<RedisOption> GetRedisOption(IOptionsMonitor<RedisOption> option) => this.Result(option.CurrentValue);

    [HttpGet("cache")]
    public async Task<ResultDto<string>> GetCacheValueAsync(string key, [FromServices] IRedisDbProvider redisDbProvider)
    {
        var db = redisDbProvider.GetDatabase();
        return this.Result((string?)await db.StringGetAsync(key) ?? "");
    }

    [HttpGet("config")]
    public ResultDto<string> GetConfiguration(string key, [FromServices] IConfiguration configuration) => this.Result(configuration[key] ?? "");
}
