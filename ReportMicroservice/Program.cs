using ReportMicroservice;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
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
