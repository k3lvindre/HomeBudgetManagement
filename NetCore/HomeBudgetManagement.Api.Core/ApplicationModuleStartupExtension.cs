using HomeBudgetManagement.Application;
using HomeBudgetManagement.Application.Repository;
using HomeBudgetManagement.Infrastructure.EntityFramework;
using HomeBudgetManagement.Infrastructure.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeBudgetManagement.Api.Core
{
    public static class ApplicationModuleStartupExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddDbContext<HomeBudgetManagementDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("HbmConnectionString")), ServiceLifetime.Scoped);
            //must replace with func<string, IGenericRepository> to get IGenericRepository by name in case of diff. implementation of IGenericRepository
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
