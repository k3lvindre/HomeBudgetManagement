//****USE FOR General Reposisotry pattern rather that indivudal Repository****//

using HomeBudgetManagement.Application;
using HomeBudgetManagement.Core.Domain.AccountAggregate;
using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeBudgetManagement.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeBudgetManagementDbContext _context;
        private IDbContextTransaction? _transaction = null;
        private readonly IGenericRepository<Expense> _expenses;
        private readonly IGenericRepository<Account> _account;

        public UnitOfWork(HomeBudgetManagementDbContext context,
            IGenericRepository<Expense> expenses,
            IGenericRepository<Account> account)
        {
            _context = context;
            _expenses = expenses;
            _account = account;
        }

        public IGenericRepository<Expense> Expenses
        {
            get
            {
                return _expenses;
            }
        }

        public IGenericRepository<Account> Accounts
        {
            get
            {
                return _account;
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
