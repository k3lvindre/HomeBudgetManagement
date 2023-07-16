using ReportMicroservice;
using ReportMicroservice.EventFeed;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient<IEvenFeedConsumer, EventFeedConsumer>();
    })
     .ConfigureAppConfiguration((hostingContext, config) =>
     {
         config.Sources.Clear();

         var env = hostingContext.HostingEnvironment;

         config.AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                              optional: true, reloadOnChange: true);

         config.AddEnvironmentVariables();

     })
    .Build();

await host.RunAsync();
