using HomeBudgetManagement.Core.Domain.MealAggregate;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Infrastructure.EntityFramework
{
    public class HomeBudgetManagementDbContext : DbContext
    {
        public HomeBudgetManagementDbContext(DbContextOptions<HomeBudgetManagementDbContext> options) : base(options)
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


        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<FileAttachment> FileAttachments { get; set; }
    }
}
