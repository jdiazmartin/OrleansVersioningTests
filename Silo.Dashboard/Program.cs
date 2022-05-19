using System.Text.Json;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansDashboard;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IClusterClient clusterClient = new ClientBuilder()
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
    .ConfigureApplicationParts(parts => parts.AddFromDependencyContext().WithReferences())
    .ConfigureLogging(logging =>
        logging.AddJsonConsole(options =>
        {
            options.IncludeScopes = true;
            options.TimestampFormat = "dd/MM/yyyy hh:mm:ss";
            options.JsonWriterOptions = new JsonWriterOptions { Indented = true };
        })
        .AddFilter(level => level >= LogLevel.Warning))
    .Build();

builder
    .Services
    .AddSingleton<IClusterClient>(clusterClient)
    .AddSingleton<IGrainFactory>(clusterClient)
    .AddServicesForSelfHostedDashboard(null);

WebApplication app = builder.Build();

app.UseOrleansDashboard(new DashboardOptions
{
    CounterUpdateIntervalMs = 5000,
});

await clusterClient.Connect(async e =>
{
    await Task.Delay(TimeSpan.FromSeconds(2));
    return true;
});

app.Run();
