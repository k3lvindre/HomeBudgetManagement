using Autofac;
using Autofac.Extensions.DependencyInjection;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Api.Core.Services.CustomAuthorization;
using HomeBudgetManagement.API.Core.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace HomeBudgetManagement.Api.Core
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // If, for some reason, you need a reference to the built container, you
        // can use the convenience extension method GetAutofacRoot.
        public ILifetimeScope AutofacContainer { get; private set; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Singleton - which creates a single instance throughout the application.It creates the instance for the first time and reuses the same object in the all calls.
            //Scoped lifetime services - are created once per request within the scope.It is equivalent to a singleton in the current scope.For example, in MVC it creates one instance for each HTTP request, but it uses the same instance in the other calls within the same web request.
            //Transient lifetime services - are created each time they are requested.This lifetime works best for lightweight, stateless services.

            services.AddControllers()
                //fix case sensitivity of properties of returned entity from the controller when consumed by client
                //during deserialization
                .AddJsonOptions(x=>  {
                    x.JsonSerializerOptions.PropertyNamingPolicy = null;
                 });

           
            #region Domain services
            //application module
            services.AddApplicationDataServices(Configuration)
                .AddApplicantEventHandler();
            #endregion

            #region Authentication
            //Identity setup
            //Because the IdentityDbContext class is defined in a different assembly, I have to tell Entity Framework
            //Core to create database migrations in the HomeBudgetManagement.Api.Core project
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnectionString"), x=>x.MigrationsAssembly("HomeBudgetManagement.Api.Core")), ServiceLifetime.Scoped);
            services.AddIdentity<IdentityUser, IdentityRole>(
                options => {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<IdentityDbContext>();

            //Add authentication & challenge scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                //add jwt bearer authentication
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience =  Configuration.GetSection("Jwt:Audience").Value,
                        //symmetric key is like having secret key that you can share to someone so he can have access too
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value))
                    };
                });
            #endregion 

            #region Authorization

            services.AddTransient<IAuthService, AuthService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("NickNamePolicy", policy =>
                    policy.Requirements.Add(new CustomAuthorizaionRequirement("kelvs")));
            });
            services.AddSingleton<IAuthorizationHandler, CustomAuthorizaionHandler>();

            #endregion

            #region Swagger
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

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeBudgetManagement.API.Core", Version = "v1" });
            //});
            #endregion

            services.AddHealthChecks();

            //cors with policy
            //services.AddCors(options => {
            //    options.AddPolicy("AllowAllOrigins", builder =>
            //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //    options.AddPolicy("AllowOnlyGet", builder =>
            //        builder.WithMethods("GET").AllowAnyHeader().AllowAnyOrigin());
            //});
            //this can be use in action or controller
            //[EnableCors("AllowAllOrigins")]
            //public IActionResult GetAllRecords(
            //{
            ////Call some service to get records
            //return View();
            //} 
            services.AddCors();

            //AutoFac
            //This example shows ASP.NET Core 1.1 - 2.2 usage,
            //where you return an IServiceProvider from the ConfigureServices(IServiceCollection services) delegate.
            //This is not for ASP.NET Core 3+ or the .NET Core 3+ generic hosting support -
            //ASP.NET Core 3 has deprecated the ability to return a service provider from ConfigureServices.
            //var container = new ContainerBuilder();
            //container.Populate(services);
            //container.RegisterModule(new MediatorModule());
            //container.RegisterModule(new ConfigurationModule(Configuration));
            //return new AutofacServiceProvider(container.Build());
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

            //in real world you should allow from known origin like x.WithOrigins("http://www.example.com")
            //other example:
            //app.UseCors(policyBuilder => policyBuilder.WithHeaders("accept,contenttype").AllowAnyOrigin().WithMethods("GET, POST"));
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHealthChecks("/health");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/hello", async c =>
                {
                    c.Response.StatusCode = StatusCodes.Status200OK;
                    await c.Response.WriteAsync("hello");
                });
            });
        }
    }
}
