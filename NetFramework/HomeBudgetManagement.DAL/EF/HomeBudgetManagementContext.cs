using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public class HomeBudgetManagementContext : DbContext
    {
        public HomeBudgetManagementContext() : base("name=HomeBudgetManagementDB")
        {
            //Custom db initializer for creating/dropping database and upsert 
            //use this if you don't want to use migration or if you want to always reset your database
            //this contains seed method same with migration
            //reset db everytime app runs 
            //Database.SetInitializer(new MyCustomDatabaseInitializer());

            //prevent joined tables to load to improve the data access which make it faster
            this.Configuration.LazyLoadingEnabled = false; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>().ToTable("Expense");
            modelBuilder.Entity<Expense>().HasKey(x => x.Id);
            modelBuilder.Entity<Income>().ToTable("Income");
            modelBuilder.Entity<Income>().HasKey(x => x.Id);
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Account>().HasKey(x => x.Id);
        }


        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }

    }
}
