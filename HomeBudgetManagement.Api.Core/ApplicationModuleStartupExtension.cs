using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core
{
    public static class ApplicationModuleStartupExtension
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ISummary<Expense>, Summary>();
            return services;
        }
    }
}
