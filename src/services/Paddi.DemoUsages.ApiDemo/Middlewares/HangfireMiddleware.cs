using Hangfire;

namespace Paddi.DemoUsages.ApiDemo.Middlewares;

public static class HangfireExtension
{
    public static IApplicationBuilder UsePaddiHangfire(this IApplicationBuilder app) => app.UseHangfireDashboard();
}
