namespace HomeBudgetManagement.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HomeBudgetManagement.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeBudgetManagementContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HomeBudgetManagementContext";
        }

        protected override void Seed(HomeBudgetManagementContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Accounts.AddOrUpdate(new Account() { Id = 0, Balance = 0.0 });
        }
    }
}
