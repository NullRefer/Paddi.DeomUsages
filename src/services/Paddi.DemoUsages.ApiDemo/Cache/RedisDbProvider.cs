using Microsoft.Extensions.Options;

using StackExchange.Redis;

namespace Paddi.DemoUsages.ApiDemo.Cache;

public interface IRedisDbProvider
{
    IDatabase GetDatabase();
}

public class RedisDbProvider : IRedisDbProvider
{
    private readonly IOptions<RedisOption> _redisOption;
    private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

    public RedisDbProvider(IOptions<RedisOption> redisOption)
    {
        _redisOption = redisOption ?? throw new ArgumentNullException(nameof(redisOption));
        _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
    }

    private ConnectionMultiplexer CreateConnectionMultiplexer()
    {
        var config = ConfigurationOptions.Parse(_redisOption.Value.ConnectionString);
        return ConnectionMultiplexer.Connect(config);
    }

    public IDatabase GetDatabase() => _connectionMultiplexer.Value.GetDatabase();
}
