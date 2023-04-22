using System.Reflection;
using System.Runtime.CompilerServices;

namespace Paddi.DemoUsages.Core;

public abstract class RunnerBase : IRunnableService
{
    protected void Log(string msg, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int lineNumber = 0)
    {
        var runnerName = GetType().GetCustomAttribute<RunnerAttribute>()!.Name;
        LogInternal(runnerName, msg, callerMemberName, lineNumber);
    }

    private static void LogInternal(string runnerName, string msg, string callerMemberName, int lineNumber)
    {
        var message = $"""
            {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | INFO | Thread-{Environment.CurrentManagedThreadId} | {runnerName}-{callerMemberName}:line {lineNumber} | {msg}
            """;
        Console.WriteLine(message);
    }

    public abstract Task RunAsync(CancellationToken token = default);
}
