using Paddi.DemoUsages.Core;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices;

[Runner("Periodic Timer", "Provides a periodic timer that enables waiting asynchronously for timer ticks", "https://learn.microsoft.com/en-us/dotnet/api/system.threading.periodictimer?view=net-6.0")]
public class PeriodicTimerRunner : RunnerBase
{
    protected override async Task RunInternalAsync(CancellationToken token = default)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        var cnt = 0;
        while (await timer.WaitForNextTickAsync(token))
        {
            Log($"Tick {++cnt}");
            if (cnt == 3)
            {
                timer.Dispose();
            }
        }
    }
}
