using HomeBudgetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly HomeBudgetManagementDbContext _dbContext;

        public AccountRepository(HomeBudgetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            var account = await _dbContext.Account.FindAsync(id);
            return account;
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            _dbContext.Entry<Account>(account).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
