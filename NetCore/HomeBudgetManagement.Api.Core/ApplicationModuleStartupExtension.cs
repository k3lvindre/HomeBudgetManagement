using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.Infrastructure.EntityFramework;
using HomeBudgetManagement.Infrastructure.EntityFramework.Repositories;
using HomeBudgetManagement.Infrastructure.EventFeed;
using HomeBudgetManagement.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeBudgetManagement.Api.Core
{
    public static class ApplicationModuleStartupExtension
    {
        public static IServiceCollection AddApplicationDataServices(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddDbContext<HomeBudgetManagementDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("HbmConnectionString")), ServiceLifetime.Scoped);
            //must replace with func<string, IGenericRepository> to get IGenericRepository by name in case of diff. implementation of IGenericRepository
            services.AddTransient<IBudgetRepository, BudgetRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddApplicantEventHandler(this IServiceCollection services)
        {
            services.AddTransient<IEventFeed, EventFeedSql>();
            return services;
        }
    }
}
