using Hangfire;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("notifications")]
public class NotificationController : ControllerBase
{
    private readonly IBackgroundJobClient _bgClient;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(IBackgroundJobClient bgClient, ILogger<NotificationController> logger)
    {
        _bgClient = bgClient;
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<ResultDto<long>> CreateNotificationAsync([FromBody] SimpleNotification notification, CancellationToken cancellationToken = default)
    {
        _bgClient.Enqueue<IWeatherForecastService>(service => service.GetAllAsync(cancellationToken));
        return Ok(1);
    }

    public record SimpleNotification(string Msg, DateTime DueAt);
}
