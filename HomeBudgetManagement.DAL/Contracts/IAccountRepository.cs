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
    }
}
