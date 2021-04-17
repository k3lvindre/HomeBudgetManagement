using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using HomeBudgetManagement.Domain;
using Unity.Lifetime;

namespace HomeBudgetManagement.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //Dependency injection using unity
            var container = new UnityContainer();
            container.RegisterType<IExpenseRepository, ExpenseRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IIncomeRepository, IncomeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAccountRepository, AccountRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //register filters globally so you don't have to add it on every controller
            //config.Filters.Add(new BasicAuthenticationAttribute());
        }
    }
}
