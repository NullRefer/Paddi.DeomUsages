using Polly;

namespace Paddi.DemoUsages.ConsoleDemo.RunnableServices.PollyRunner;

[Runner("Polly", "Provide resilience policies to handle faults", "https://github.com/App-vNext/Polly")]
internal class PollyRunner : RunnerBase
{
    protected override async Task RunInternalAsync(CancellationToken token = default)
    {
        await RetryPolicyAsync();
        await CircuitBreakPolicyAsync();
        await TimeoutPolicyAsync();
        await BulkheadIsolationPolicyAsync();
        await RateLimitPolicyAsync();
    }

    private Task<bool> DemoTaskThrowArgumentNullException(string? param1, string param2, CancellationToken token = default)
    {
        if (param1 == null)
        {
            throw new ArgumentNullException(nameof(param1));
        }

        if (param2 == null)
        {
            throw new ArgumentNullException(nameof(param2));
        }

        if (param1 == param2)
        {
            return Task.FromResult(false);
        }

        Log("Demo task execute success!");
        return Task.FromResult(true);
    }

    private async Task RetryPolicyAsync()
    {
        try
        {
            var result = await Policy
                .Handle<ArgumentNullException>(e => e.ParamName == "param1")
                .RetryAsync(3, (exc, cnt) => Log($"Exception occurs at {cnt} retry"))
                .ExecuteAsync(async () => await DemoTaskThrowArgumentNullException(null, "has", default));
        }
        catch (Exception)
        {
        }
        Log("Retry done!");
    }

    private async Task CircuitBreakPolicyAsync()
    {
        try
        {
            var result = await Policy
                .Handle<ArgumentNullException>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(5))
                .ExecuteAsync(async () =>
                {
                    await DemoTaskThrowArgumentNullException(null, "any");
                    await DemoTaskThrowArgumentNullException(null, "any");
                    await DemoTaskThrowArgumentNullException(null, "any");
                    await DemoTaskThrowArgumentNullException(null, "any");
                    await DemoTaskThrowArgumentNullException(null, "any");
                    await DemoTaskThrowArgumentNullException(null, "any");
                    return await DemoTaskThrowArgumentNullException(null, "any");
                });
        }
        catch (Exception)
        {
        }
        Log("CircuitBreak done!");
    }

    private async Task TimeoutPolicyAsync()
    {
        try
        {
            var result = await Policy
                .TimeoutAsync(3)
                .ExecuteAsync(async token =>
                {
                    await Task.Delay(1000, token);
                    Log("1s passed");
                    await Task.Delay(3000, token);
                    Log("Success");
                    return true;
                }, CancellationToken.None);
        }
        catch (Exception)
        {
        }
        Log("Timeout success");
    }

    private async Task BulkheadIsolationPolicyAsync()
    {
        try
        {
            var policy = Policy.Bulkhead(5, _ => Log("Max of 5 exceeds"));
            foreach (var i in Enumerable.Range(0, 10))
            {
                await Task.Run(() => policy.Execute(() => Log($"This is {i} times called.")));
            }
        }
        catch (Exception)
        {
        }
        await Task.CompletedTask;
    }

    private async Task RateLimitPolicyAsync()
    {
        try
        {
            var policy = Policy.RateLimitAsync(10, TimeSpan.FromSeconds(10));
            foreach (var i in Enumerable.Range(0, 10))
            {
                await policy.ExecuteAsync(async () =>
                {
                    Log($"This is {i} times called");
                    await Task.Delay(985);
                });
            }
        }
        catch (Exception exc)
        {
            Log(exc.ToString());
        }

    }
}
