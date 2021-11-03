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

            //Here we don't need to map account to table since the property name has the same name with the database's table account
            //modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<Account>().HasKey(x => x.Id);
        }


        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Account> Account { get; set; }

    }
}
