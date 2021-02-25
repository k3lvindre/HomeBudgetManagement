using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public class AccountRepository : IAccountRepository
    {
        HomeBudgetManagementContext _dbContext;
        public AccountRepository()
        {
            _dbContext = new HomeBudgetManagementContext();
        }

        public  async Task<Account> GetAsync()
        {
            Account account = await _dbContext.Accounts.FirstOrDefaultAsync();
            return account;
        }

        public async Task<bool> SaveAsync(Account account)
        {
            //use transaction to lock it
            using (DbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    _dbContext.Entry(account).State = EntityState.Modified;
                    int result = await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return result > 0;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return false;
        }

        //Implement Idisposable,  We can also implement "`Finalize" to override GC memory handling but it take some time.
        //by implementing idisposable we let consumer handle the disposing/memory handling
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
