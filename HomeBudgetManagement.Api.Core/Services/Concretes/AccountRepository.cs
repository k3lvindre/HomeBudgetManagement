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

        public async Task<Account> GetAccountAsync()
        {
            if(!_dbContext.Account.Any())
            {
                await _dbContext.Account.AddAsync(new Account() { Id = 0, Balance = 0 });
                await _dbContext.SaveChangesAsync();
            }

            return await _dbContext.Account.FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAccountAsync(Account account)
        {
            _dbContext.Entry<Account>(account).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }
    }
}
