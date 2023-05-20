using HomeBudgetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Api.Core.Data
{
    public class HomeBudgetManagementDbContext : DbContext
    {
        public HomeBudgetManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Expense>().ToTable("Expense");
            //modelBuilder.Entity<Expense>().HasKey(x => x.Id);
            //modelBuilder.Entity<Income>().ToTable("Income");
            //modelBuilder.Entity<Income>().HasKey(x => x.Id);
            //Here we don't need to map account totable since the property name has the same name with the database's table account
            //modelBuilder.Entity<Account>().ToTable("Account");
            //modelBuilder.Entity<Account>().HasKey(x => x.Id);
        }


        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Account> Account { get; set; }

    }
}
