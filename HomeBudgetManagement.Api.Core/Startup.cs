using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using HomeBudgetManagement.API.Core.Infrastructure;
using HomeBudgetManagement.Application.EventFeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using HomeBudgetManagement.Infrastructure.EventFeed;

namespace HomeBudgetManagement.Api.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // If, for some reason, you need a reference to the built container, you
        // can use the convenience extension method GetAutofacRoot.
        public ILifetimeScope AutofacContainer { get; private set; }

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


            //Singleton - which creates a single instance throughout the application.It creates the instance for the first time and reuses the same object in the all calls.
            //Scoped lifetime services - are created once per request within the scope.It is equivalent to a singleton in the current scope.For example, in MVC it creates one instance for each HTTP request, but it uses the same instance in the other calls within the same web request.
            //Transient lifetime services - are created each time they are requested.This lifetime works best for lightweight, stateless services.
            services.AddCustomDbContext(Configuration);

            //run this on package manager
            //Install-Package Swashbuckle.AspNetCore -Version 6.2.3
            //Enable the middleware for serving the generated JSON document and the Swagger UI
            //Access Swagger document and API documentation
            //To see the generated Swagger document i.e the OpenAPI specification document, navigate to http://localhost:<port>/swagger/v1/swagger.json
            //To see the swagger documentation UI navigate to http://localhost:<port>/swagger
            //To serve the Swagger UI at the application root URL (http://localhost:<port>/), set the RoutePrefix property to an empty string:
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V1");
            //    c.RoutePrefix = string.Empty;
            //});
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
            services.AddHealthChecks();

            //This example shows ASP.NET Core 1.1 - 2.2 usage,
            //where you return an IServiceProvider from the ConfigureServices(IServiceCollection services) delegate.
            //This is not for ASP.NET Core 3+ or the .NET Core 3+ generic hosting support -
            //ASP.NET Core 3 has deprecated the ability to return a service provider from ConfigureServices.
            //var container = new ContainerBuilder();
            //container.Populate(services);
            //container.RegisterModule(new MediatorModule());
            //container.RegisterModule(new ConfigurationModule(Configuration));
            //return new AutofacServiceProvider(container.Build());

            services.AddTransient<IEventFeed, EventFeedSql>();
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

            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            app.UseHealthChecks("/health");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        //kELVIN: METHOD USE BY AUTOFAC TO REGISTER SERVICES.
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new MediatorModule());
        }
    }
}
