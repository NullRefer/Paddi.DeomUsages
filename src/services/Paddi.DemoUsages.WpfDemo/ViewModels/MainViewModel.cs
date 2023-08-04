using Microsoft.AspNetCore.SignalR.Client;

namespace Paddi.DemoUsages.WpfDemo.ViewModels;

internal class MainViewModel : ObservableObject
{
    private readonly HubConnection _hubConnection;

    public MainViewModel()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5580/ChatHub")
            .WithAutomaticReconnect()
            .Build();
        _hubConnection.StartAsync();
        _hubConnection.On<string>("ServerTime", ReceiveServerTime);
        _hubConnection.On<string>("WorldMessage", ReceiveWorldMessage);
        //_hubConnection.On<string>("GroupMessage", ServerTime);

        CurrentDateTime = DateTime.Now.ToString();
        ConnectToGroup();
        SendWorldMessageCommand = new AsyncRelayCommand<string>(SendWorldMessageAsync);
    }

    private string _dateTime;
    public string CurrentDateTime
    {
        get => _dateTime;
        set => SetProperty(ref _dateTime, value);
    }

    private string _worldMessage;
    public string WorldMessage
    {
        get => _worldMessage;
        set => SetProperty(ref _worldMessage, value);
    }

    private string _groupName;
    public string GroupName
    {
        get => _groupName;
        set => SetProperty(ref _groupName, value);
    }

    private string _msgToSend;
    public string MsgToSend
    {
        get => _msgToSend;
        set => SetProperty(ref _msgToSend, value);
    }

    public IAsyncRelayCommand<string> SendWorldMessageCommand { get; }

    private void ConnectToGroup()
    {
        _hubConnection.InvokeAsync("AddToGroup", Guid.NewGuid().ToString(), GroupName).GetAwaiter().GetResult();
    }

    private void ReceiveServerTime(string dateTime)
    {
        CurrentDateTime = DateTime.Parse(dateTime).ToString("yyyy-MM-dd HH:mm:ss");
    }

    private void ReceiveWorldMessage(string message)
    {
        WorldMessage += Environment.NewLine + message;
    }

    private async Task SendWorldMessageAsync(string? msg)
    {
        await _hubConnection.InvokeAsync("BroadcastMessage", MsgToSend);
    }
}
