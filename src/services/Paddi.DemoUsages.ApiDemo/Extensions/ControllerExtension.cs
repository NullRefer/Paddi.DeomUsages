namespace Paddi.DemoUsages.ApiDemo.Extensions;

public static class ControllerExtension
{
    public static ResultDto<T> Result<T>(this ControllerBase _, T data) => new(data);
}
