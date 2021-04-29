using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public interface IAccountRepository :IDisposable    
    {
      Task<Account> GetAsync();
      Task<bool> SaveAsync(Account account);
      Task AddBalanceAsync(double amount, bool save = false);
      Task DeductFromBalanceAsync(double amount, bool save = false);
      Task UpdateBalanceAsync(double amountToAdd, double amountToDeduct, bool save = false);
    }
}
