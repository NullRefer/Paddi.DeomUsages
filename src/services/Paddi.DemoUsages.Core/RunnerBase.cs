using System.Reflection;
using System.Runtime.CompilerServices;

namespace Paddi.DemoUsages.Core;

public abstract class RunnerBase : IRunnableService
{
    private RunnerAttribute RunnerInfo => GetType().GetCustomAttribute<RunnerAttribute>()!;

    protected void Log(string msg, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int lineNumber = 0)
    {
        LogInternal(msg, callerMemberName, lineNumber);
    }

    private void LogInternal(string msg, string callerMemberName, int lineNumber)
    {
        var message = $"""
            {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | INFO | Thread-{Environment.CurrentManagedThreadId} | {RunnerInfo.Name}-{callerMemberName}:line {lineNumber} | {msg}
            """;
        Console.WriteLine(message);
    }

    public virtual async Task RunAsync(CancellationToken token = default)
    {
        await BeforeAsync(token);
        await RunInternalAsync(token);
        await AfterAsync(token);
    }

    protected virtual Task BeforeAsync(CancellationToken token = default)
    {
        var length = RunnerInfo.Name.Length;
        var padding = new string(Enumerable.Range(0, length).Select(e => '=').ToArray());
        Console.WriteLine($"====================={padding}=====================");
        Console.WriteLine($"==================== {RunnerInfo.Name} ====================");
        Console.WriteLine();
        return Task.CompletedTask;
    }

    protected virtual Task AfterAsync(CancellationToken token = default)
    {
        var length = RunnerInfo.Name.Length;
        var padding = new string(Enumerable.Range(0, length).Select(e => '=').ToArray());
        Console.WriteLine();
        Console.WriteLine($"==================== {RunnerInfo.Name} ====================");
        Console.WriteLine($"====================={padding}=====================");
        Console.WriteLine();
        Console.WriteLine();
        return Task.CompletedTask;
    }

    protected abstract Task RunInternalAsync(CancellationToken token = default);
}
