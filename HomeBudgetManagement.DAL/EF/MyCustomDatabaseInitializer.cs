using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    //Drop and Create Database Always
    public class MyCustomDatabaseInitializer:  DropCreateDatabaseAlways<HomeBudgetManagementContext>
    {
        protected override void Seed(HomeBudgetManagementContext context)
        {
            //add or update default data
            var expense = new Expense { Description = "Grocery", Amount = 200.50, Date = DateTime.Now };
            context.Expenses.AddOrUpdate(expense);
            context.SaveChanges();
        }
    }
}
