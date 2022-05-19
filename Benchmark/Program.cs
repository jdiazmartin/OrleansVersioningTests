using Grains;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

//BenchmarkRunner.Run<ConcurrentGrains>();


var client = new ClientBuilder()
.UseAdoNetClustering(options =>
{
    options.Invariant = "Npgsql";
    options.ConnectionString = "server=localhost;port=49153;Database=postgres;Integrated Security=True;Pooling=False;User Id=postgres;Password=postgrespw;";
})
    .Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "ClusterId";
        options.ServiceId = "ServiceId";
    })
.Build();

await client.Connect();



var result = Parallel.For(0, 10, async (i) => {
    Console.WriteLine(i);
    var grain = client.GetGrain<ITestConcurrencyGrain>($"{i}");
    await grain.Send(i);
    }
);



Console.WriteLine(result.IsCompleted);
Console.ReadKey();