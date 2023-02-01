using Example.MondayCom.App;
using Example.MondayCom.Services.MondayComApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// See https://aka.ms/new-console-template for more information

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOptions<MondayComOptions>()
            .Configure<IConfiguration>((options, configuration) =>
            {
                configuration.GetSection(MondayComOptions.Name).Bind(options);
            });

        services.AddTransient<MondayComApiClient>();
        services.AddTransient<MondayComApiCaller>();
        services.AddLogging();
    })
    .ConfigureHostConfiguration(builder =>
    {
        var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Development";
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();
    })
    .ConfigureAppConfiguration(builder =>
    {
        var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Development";
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();
    })
    .Build();

await StartMondayComApi(host);

// await host.RunAsync();

static async Task StartMondayComApi(IHost host)
{
    CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(60));
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    var sp = host.Services;

    var caller = sp.GetRequiredService<MondayComApiCaller>();

    await caller.GetBoardItemsAsync(cancellationToken);

    await caller.GetBoardItemByColumnValueAsync(cancellationToken);

    await caller.GetBoardItemsAsync(cancellationToken);

    await caller.CreateBoardItemAsync(1, cancellationToken);

    await caller.UpdateBoardItemAsync(1, cancellationToken);
}