using Microsoft.Extensions.Options;

using Paddi.DemoUsages.ApiDemo.Cache;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("sys")]
public class SysController : DemoControllerBase
{
    [HttpGet("redis-option")]
    public ActionResult<ApiResultDto<RedisOption>> GetRedisOption(IOptionsMonitor<RedisOption> option) => Result(option.CurrentValue);

    [HttpGet("cache")]
    public async Task<ActionResult<ApiResultDto<string>>> GetCacheValueAsync(string key, [FromServices] IRedisDbProvider redisDbProvider)
    {
        var db = redisDbProvider.GetDatabase();
        return Result((string?)await db.StringGetAsync(key) ?? "");
    }

    [HttpPost("cache")]
    public async Task<ActionResult<ApiResultDto<bool>>> SetCacheValueAsync(string key, string value, [FromServices] IRedisDbProvider redisDbProvider)
    {
        var db = redisDbProvider.GetDatabase();
        return Result(await db.StringSetAsync(key, value));
    }

    [HttpGet("config")]
    public ActionResult<ApiResultDto<string>> GetConfiguration(string key, [FromServices] IConfiguration configuration) => Result(configuration[key] ?? "");
}
