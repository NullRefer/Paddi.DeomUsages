namespace Paddi.DemoUsages.Core;

public interface IRunnableService
{
    Task RunAsync(CancellationToken token = default);
}
