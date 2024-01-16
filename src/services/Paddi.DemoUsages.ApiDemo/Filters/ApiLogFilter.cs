using Microsoft.AspNetCore.Mvc.Filters;

namespace Paddi.DemoUsages.ApiDemo.Filters;

public class ApiLogFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiLogFilter>>();
        var newDict = new Dictionary<string, object?>();
        foreach (var (key, value) in context.ActionArguments)
        {
            if (value is CancellationToken) continue;
            newDict.Add(key, value);
        }
        logger.LogInformation("Request: {Request}", JsonConvert.SerializeObject(newDict));
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiLogFilter>>();
        var responseString = context.Result switch
        {
            ObjectResult objectResult => JsonConvert.SerializeObject(objectResult.Value),
            ContentResult contentResult => JsonConvert.SerializeObject(contentResult.Content),
            _ => context.Result?.ToString() ?? "",
        };
        logger.LogInformation("Response: {Response}", responseString);
    }
}
