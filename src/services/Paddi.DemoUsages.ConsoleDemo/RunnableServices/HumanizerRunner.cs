using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

using Humanizer;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices;

[Runner("Humanize", "Interpret sentence or strings", "https://github.com/Humanizr/Humanizer")]
public class HumanizerRunner : RunnerBase
{
    private const string SentenceEasy = "This is a sentence easy to understand";
    private const string SentenceHard = "ThisIsASentenceEasyToUnderstand";

    protected override Task RunInternalAsync(CancellationToken token = default)
    {
        HumanizeString();
        DehumanizeString();
        TruncateString();
        TransformString();
        FormatString();
        HumanizeEnum();
        HumanizeDateTime();

        return Task.CompletedTask;
    }

    private void HandleString(string rawString, Func<string, string> handler, [CallerMemberName] string callerMemberName = "")
    {
        Log("Before {0}: {1}", callerMemberName, args: Args(callerMemberName, rawString));
        Log("After {0}: {1}", callerMemberName, args: Args(callerMemberName, handler(rawString)));
    }

    private void HumanizeString()
    {
        HandleString(SentenceHard, StringHumanizeExtensions.Humanize);
    }

    private void DehumanizeString()
    {
        HandleString(SentenceEasy, StringDehumanizeExtensions.Dehumanize);
    }

    private void TruncateString()
    {
        HandleString(SentenceEasy, s => TruncateExtensions.Truncate(s, 3));
    }

    private void TransformString()
    {
        HandleString(SentenceEasy, s => To.Transform(s, To.TitleCase));
        HandleString(SentenceEasy, s => To.Transform(s, To.LowerCase));
        HandleString(SentenceEasy, s => To.Transform(s, To.UpperCase));
        HandleString(SentenceEasy, s => To.Transform(s, To.SentenceCase));
    }

    private void FormatString()
    {
        HandleString("This is a sentence with format: {0} - {1}", s => s.FormatWith("NameReplaced", "ValueReplaced"));
    }

    private void HumanizeEnum()
    {
        Log("Before {0}: {1}", args: Args("HumanizeEnum", HumanizeDemo.WithDescription));
        Log("After {0}: {1}", args: Args("HumanizeEnum", HumanizeDemo.WithDescription.Humanize()));
    }

    private enum HumanizeDemo { [Description("This is in attribute")] WithDescription, WithoutDescription }

    private void HumanizeDateTime()
    {
        var now = DateTime.Now.AddDays(1.1);
        Log($"Now: {now.Humanize(culture: CultureInfo.CurrentCulture)}");
    }
}
