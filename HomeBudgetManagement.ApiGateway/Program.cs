using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

//ocelot
builder.Services.AddOcelot(builder.Configuration);

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

var app = builder.Build();

//add ocelot middleware
await app.UseOcelot();
app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");
app.Run();
