using HomeBudgetManagement.Core.Domain.AccountAggregate;
using HomeBudgetManagement.Core.Domain.ExpenseAggregate;

//This namespace Should be move to separate project
namespace HomeBudgetManagement.Application
{
    public interface IUnitOfWork
    {
        //Start the database Transaction
        void CreateTransaction();
        //Commit the database Transaction
        void Commit();
        //Rollback the database Transaction
        void Rollback();
        //DbContext Class SaveChanges method
        Task<int> SaveChangesAsync();


        IGenericRepository<Expense> Expenses { get; }
        IGenericRepository<Account> Accounts { get; }
    }
}
