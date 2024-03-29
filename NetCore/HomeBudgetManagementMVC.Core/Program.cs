﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeBudgetManagement.MVC.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.Sources.Clear();

                     var env = hostingContext.HostingEnvironment;

                     config.AddJsonFile($"appsettings.json",
                                          optional: true, reloadOnChange: true);

                     config.AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                          optional: true, reloadOnChange: true);

                     config.AddEnvironmentVariables();
                 })

                .UseStartup<Startup>();
    }
}
