using HomeBudgetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountAsync();
        Task<int> UpdateAccountAsync(Account account);
    }
}
