using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansDashboard;

var builder = WebApplication.CreateBuilder(args);

int siloPort = EndpointOptions.DEFAULT_SILO_PORT + 1;   
int gatewayPort = EndpointOptions.DEFAULT_GATEWAY_PORT + 1;

var app = builder.Build();
ISiloHostBuilder siloHostBuilder = new SiloHostBuilder()
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
    .ConfigureEndpoints(
        siloPort: siloPort,
        gatewayPort: gatewayPort)
    .UseDashboard(options => { options.HostSelf = false; })
    .ConfigureApplicationParts(parts => parts.AddFromDependencyContext().WithReferences());

await siloHostBuilder.Build().StartAsync();

app.Run();
