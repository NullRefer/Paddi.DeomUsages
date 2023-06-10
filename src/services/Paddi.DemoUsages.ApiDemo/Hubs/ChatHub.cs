using System.Collections.Concurrent;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace Paddi.DemoUsages.ApiDemo.Hubs;

public class ChatHub : Hub
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<ChatHub> _logger;

    private readonly ConcurrentDictionary<string, HashSet<string>> _userGroups = new();

    public const string GroupNameMemCacheKey = "ChatHub.GroupName";

    public ChatHub(IMemoryCache cache, ILogger<ChatHub> logger)
    {
        _cache = cache;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override Task OnConnectedAsync()
    {
        if (!_userGroups.ContainsKey(Context.ConnectionId))
        {
            _userGroups[Context.ConnectionId] = new HashSet<string>();
            _logger.LogDebug("ConnectionId - {ConnectionId} has join the hub", Context.ConnectionId);
        }
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _userGroups.TryRemove(Context.ConnectionId, out _);
        _logger.LogDebug("ConnectionId - {ConnectionId} has left the hub", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    [HubMethodName("AddToGroup")]
    public async Task AddToGroup(string identity, string groupName, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(identity))
        {
            _logger.LogInformation("GroupName or Identity for connectionId - {Id} is null or whitespace", Context.ConnectionId);
            return;
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName, token);
        EnrichGroups(groupName);
        await Clients.All.SendAsync("NewerJoin", identity, token);
    }

    [HubMethodName("BroadcastMessage")]
    public async Task BroadcastMessageAsync(object message, CancellationToken token = default)
    {
        await Clients.All.SendAsync("WorldChannel", message.ToString(), token);
        _logger.LogInformation("{ConnectionId} sends message to WorldChannel: {Msg}", Context.ConnectionId, message);
    }

    [HubMethodName("SendGroupMessage")]
    public async Task SendGroupMessageAsync(string groupName, object message, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            _logger.LogWarning("GroupName provided by {ConnectionId} is null or whitespace", Context.ConnectionId);
            return;
        }
        await Clients.Group(groupName).SendAsync("GroupChannel", message.ToString(), token);
        _logger.LogInformation("{ConnectionId} sends message to {GroupName}: {Msg}", Context.ConnectionId, groupName, message);
    }

    public static string GetWorldMessageCacheKey() => "World:messages";
    public static string GetGroupMessageCacheKey(string groupName)
        => !string.IsNullOrWhiteSpace(groupName) ? $"{groupName}:messages" : throw new ArgumentNullException(nameof(groupName));

    private void EnrichGroups(string groupName)
    {
        var groups = _cache.Get<HashSet<string>>(GroupNameMemCacheKey) ?? new HashSet<string>();
        _ = groups.Add(groupName);

        _cache.Set(GroupNameMemCacheKey, groups);
    }
}
