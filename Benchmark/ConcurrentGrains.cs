using BenchmarkDotNet.Attributes;
using Grains;
using Orleans;

namespace Benchmark;

[SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 10)]
public class ConcurrentGrains
{
    private IClusterClient client;

    [GlobalSetup]
    public async Task Setup()
    {
        Console.WriteLine("CTOR");
        this.client = new ClientBuilder()
        .UseLocalhostClustering()
        .Build();
        await client.Connect();
    }

    [Benchmark]
    public Task InvokeGrain()
    {
        var grain = client.GetGrain<ITestConcurrencyGrain>("Antonio");

        grain.Send(2);

        return Task.CompletedTask;
    }
}
