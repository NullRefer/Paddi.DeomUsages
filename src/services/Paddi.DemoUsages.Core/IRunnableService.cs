namespace Paddi.DemoUsages.Core;

public interface IRunnableService
{
    bool Skip { get; }
    Task RunAsync(CancellationToken token = default);
}
