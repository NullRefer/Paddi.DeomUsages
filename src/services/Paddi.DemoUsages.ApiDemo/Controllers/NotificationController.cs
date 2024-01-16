using Hangfire;

namespace Paddi.DemoUsages.ApiDemo.Controllers;

[ApiController, Route("notifications")]
public class NotificationController : DemoControllerBase
{
    private readonly IBackgroundJobClient _bgClient;

    public NotificationController(IBackgroundJobClient bgClient) => _bgClient = bgClient;

    [HttpPost]
    public ActionResult<ApiResultDto<long>> CreateNotificationAsync([FromBody] SimpleNotification notification, CancellationToken cancellationToken = default)
    {
        _bgClient.Enqueue<IDictService>(service => service.GetAsync(1, cancellationToken));
        return Ok(1);
    }

    public record SimpleNotification(string Msg, DateTime DueAt);
}
