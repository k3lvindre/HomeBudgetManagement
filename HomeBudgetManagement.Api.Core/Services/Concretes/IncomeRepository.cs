using HomeBudgetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly HomeBudgetManagementDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;

        public IncomeRepository(HomeBudgetManagementDbContext dbContext,
            IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
        }

        public async Task<List<Income>> GetAllAsync()
        {
            return await _dbContext.Incomes.ToListAsync();
        }

        public async Task<Income> AddAsync(Income income)
        {

            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Incomes.AddAsync(income);

                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    //account.Balance += income.Amount;

                    await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                }
            }

            return income;
        }

        public async Task<bool> AddRangeAsync(List<Income> incomes)
        {
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Incomes.AddRangeAsync(incomes);

                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    //account.Balance += incomes.Sum(x => x.Amount);

                    await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                    return false;
                }
            }

            return true;
        }

        public async Task<Income> GetByIdAsync(int Id)
        {
            return await _dbContext.Incomes.FindAsync(Id);
        }

        public async Task<bool> SaveAsync(Income income)
        {
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Income incomeFromDb = await _dbContext.Incomes.FindAsync(income.Id);
                    EntityEntry<Income> entry = _dbContext.Entry<Income>(incomeFromDb);

                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    ////Add the original balance for correct balance calculation
                    //account.Balance -= Convert.ToDouble(entry.OriginalValues["Amount"]);
                    //account.Balance += income.Amount;

                    entry.Property(x => x.Amount).CurrentValue = income.Amount;
                    entry.State = EntityState.Modified;

                    await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {

                    await dbContextTransaction.RollbackAsync();
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Income income = await _dbContext.Incomes.FindAsync(id);
                    if (income != null)
                    {
                        _dbContext.Incomes.Remove(income);

                        //Move to pub/sub
                        //Account account = await _accountRepository.GetAccountAsync();
                        //account.Balance -= income.Amount;

                        await _dbContext.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                    return false;
                }
            }

            return true;
        }
    }
}
