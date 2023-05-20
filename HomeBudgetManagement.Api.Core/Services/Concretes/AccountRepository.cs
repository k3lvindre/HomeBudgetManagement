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

        public Task<Account> AddAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRangeAsync(List<Account> entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            var account = await _dbContext.Account.FindAsync(id);
            return account;
        }

        public Task<List<Account>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            _dbContext.Entry<Account>(account).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
