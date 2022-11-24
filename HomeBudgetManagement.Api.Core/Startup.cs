using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HomeBudgetManagement.Api.Core.Data;
using HomeBudgetManagement.Api.Core.Services;
using Microsoft.OpenApi.Models;
using System;

namespace HomeBudgetManagement.Api.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                //fix case sensitivity of properties of returned entity from the controller when consumed by client
                //during deserialization
                .AddJsonOptions(x=>  {
                    x.JsonSerializerOptions.PropertyNamingPolicy = null;
                 });

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeBudgetManagement.API.Core", Version = "v1" });
            //});

            services.AddDbContext<HomeBudgetManagementDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HbmConnectionString")), ServiceLifetime.Scoped);

            //Singleton - which creates a single instance throughout the application.It creates the instance for the first time and reuses the same object in the all calls.
            //Scoped lifetime services - are created once per request within the scope.It is equivalent to a singleton in the current scope.For example, in MVC it creates one instance for each HTTP request, but it uses the same instance in the other calls within the same web request.
            //Transient lifetime services - are created each time they are requested.This lifetime works best for lightweight, stateless services.
            services.AddApplicationModule();

            //run this on package manager
            //Install-Package Swashbuckle.AspNetCore -Version 6.2.3
            //Enable the middleware for serving the generated JSON document and the Swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HomeBudgetManagement API",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //IWebHostEnvironment.EnvironmentName can be set to any value in launchSettings.json, but the following values are provided by the framework:
            //-Development: The launchSettings.json file sets ASPNETCORE_ENVIRONMENT to Development on the local machine.
            //-Staging
            //-Production : The default if DOTNET_ENVIRONMENT and ASPNETCORE_ENVIRONMENT have not been set.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //adds the Swagger middleware
                app.UseSwagger();
                app.UseSwaggerUI();//enables the Static File Middleware(Static files, such as HTML, CSS, images, and JavaScript).
            }

            if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("Staging_2"))
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
