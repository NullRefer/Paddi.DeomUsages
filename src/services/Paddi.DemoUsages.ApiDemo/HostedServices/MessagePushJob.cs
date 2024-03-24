using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

using Paddi.DemoUsages.ApiDemo.Cache;
using Paddi.DemoUsages.ApiDemo.Hubs;

using StackExchange.Redis;

namespace Paddi.DemoUsages.ApiDemo.HostedServices;

public partial class MessagePushJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMemoryCache _cache;
    private readonly IDatabase _redis;
    private readonly ILogger<MessagePushJob> _logger;

    private const string ServerTimeChannel = "ServerTime";
    private const string WorldMessageChannel = "WorldMessage";
    private const string GroupMessageChannel = "GroupMessage";

    public MessagePushJob(IServiceProvider serviceProvider,
                          IHubContext<ChatHub> hubContext,
                          IMemoryCache cache,
                          IRedisDbProvider redisDbProvider,
                          ILogger<MessagePushJob> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        _cache = cache;
        _redis = redisDbProvider.GetDatabase();
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.WhenAll(
            StartChannelAsync(ServerTimeChannel, PushServerTime, TimeSpan.FromMilliseconds(333), stoppingToken),
            StartChannelAsync(WorldMessageChannel, PushWorldMessage, TimeSpan.FromSeconds(3), stoppingToken),
            StartChannelAsync(GroupMessageChannel, PushGroupMessage, TimeSpan.FromSeconds(3), stoppingToken));

    private Task PushServerTime(IServiceScope scope, CancellationToken token = default)
    {
        return _hubContext.Clients.All.SendAsync(ServerTimeChannel, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), token);
    }

    private async Task PushWorldMessage(IServiceScope scope, CancellationToken token = default)
    {
        var message = await _redis.ListRightPopAsync(ChatHub.GetWorldMessageCacheKey());
        if (message.HasValue)
        {
            await _hubContext.Clients.All.SendAsync(WorldMessageChannel, message.ToString(), token);
        }
    }

    private async Task PushGroupMessage(IServiceScope scope, CancellationToken token = default)
    {
        if (!_cache.TryGetValue<List<string>>(ChatHub.GroupNameMemCacheKey, out var groups) || !groups!.Any())
        {
            return;
        }

        foreach (var group in groups!)
        {
            var message = await _redis.ListRightPopAsync(ChatHub.GetGroupMessageCacheKey(group));
            if (message.HasValue)
            {
                await _hubContext.Clients.Group(group).SendAsync(GroupMessageChannel, message.ToString(), token);
            }
        }
    }

    private async Task StartChannelAsync(string channelName, Func<IServiceScope, CancellationToken, Task> invocation, TimeSpan interval, CancellationToken token = default)
    {
        while (true)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                using var scope = _serviceProvider.CreateAsyncScope();

                await invocation(scope, token);
            }
            catch (Exception exc)
            {
                LogChannelError(exc, channelName);
            }
            finally
            {
                await Task.Delay(interval, token);
            }
        }
    }

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error while pushing {ChannelName}")]
    public partial void LogChannelError(Exception exc, string channelName);
}
