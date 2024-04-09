using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("sys")]
public class SysController : DemoControllerBase
{
    [HttpGet("redis-option")]
    public ActionResult<ApiResultDto<RedisOption>> GetRedisOption(IOptionsMonitor<RedisOption> option) => Result(option.CurrentValue);

    [HttpGet("cache")]
    public async Task<ActionResult<ApiResultDto<string>>> GetCacheValueAsync(string key, [FromServices] IDatabase redis)
    {
        var result = (string?)await redis.StringGetAsync(key);
        return Result(result ?? "");
    }

    [HttpPost("cache")]
    public async Task<ActionResult<ApiResultDto<bool>>> SetCacheValueAsync(string key, string value, [FromServices] IDatabase db)
    {
        return Result(await db.StringSetAsync(key, value, TimeSpan.FromHours(1)));
    }

    [HttpGet("config")]
    public ActionResult<ApiResultDto<string>> GetConfiguration(string key, [FromServices] IConfiguration configuration) => Result(configuration[key] ?? "");
}
