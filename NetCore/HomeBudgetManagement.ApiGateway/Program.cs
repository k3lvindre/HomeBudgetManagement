using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables();

//ocelot
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddHealthChecks();

//this is how to Add authentication & challenge scheme to api gateway
//This config(scheme & IssuerSigningKey) must matched with HomeBudgetManagement.Api.Core so authentication for this api gateway
//will work on HomeBudgetManagement.Api.Core
builder.Services.AddAuthentication(options =>
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
            ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
            ValidAudience = configuration.GetSection("Jwt:Audience").Value,
            //symmetric key is like having secret key that you can share to someone so he can have access too
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Key").Value!))
        };
    });

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

builder.Services.AddCors();

var app = builder.Build();


//in real world you should allow from known origin like x.WithOrigins("http://www.example.com")
//other example:
//app.UseCors(policyBuilder => policyBuilder.WithHeaders("accept,contenttype").AllowAnyOrigin().WithMethods("GET, POST"));
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHealthChecks("/health");

//add ocelot middleware
await app.UseOcelot();
app.UseAuthentication();
app.UseAuthorization();


//Different between these two Run is:
//1. Adding terminal middleware
//app.Run(async context =>
//{
//    // Handle requests for specific paths
//    if (context.Request.Path == "/special")
//    {
//        await context.Response.WriteAsync("Special endpoint");
//    }
//});

//app.Run(async context =>
//{
//    // Default handler for requests not handled by other middleware
//    context.Response.StatusCode = 404;
//    await context.Response.WriteAsync("Resource not found");
//});

//2. Actually running the server
app.Run();
