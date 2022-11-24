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
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly HomeBudgetManagementDbContext _dbContext;
        private readonly IAccountRepository _accountRepository;

        public ExpenseRepository(HomeBudgetManagementDbContext dbContext,
                                 IAccountRepository accountRepository)
        {
            _dbContext = dbContext;
            _accountRepository = accountRepository;
        }
      
        public async Task<List<Expense>> GetAllAsync()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task<Expense> AddAsync(Expense expense)
        {
          
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Expenses.AddAsync(expense);

                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    //account.Balance -= expense.Amount;

                    await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                   await dbContextTransaction.RollbackAsync();
                }
            }

            return expense;
        }

        public async Task<bool> AddRangeAsync(List<Expense> expenses)
        {
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Expenses.AddRangeAsync(expenses);


                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    //account.Balance -= expenses.Sum(x => x.Amount);

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

        public async Task<Expense> GetByIdAsync(int Id)
        {
            return await _dbContext.Expenses.FindAsync(Id);
        }

        public async Task<bool> SaveAsync(Expense expense)
        {
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Expense expenseFromDb = await _dbContext.Expenses.FindAsync(expense.Id);
                    EntityEntry<Expense> entry = _dbContext.Entry<Expense>(expenseFromDb);


                    //Move to pub/sub
                    //Account account = await _accountRepository.GetAccountAsync();
                    ////Add the original balance for correct balance calculation
                    //account.Balance += Convert.ToDouble(entry.OriginalValues["Amount"]);
                    //account.Balance -= expense.Amount;

                    entry.Property(x => x.Amount).CurrentValue = expense.Amount;
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
                    Expense expense = await _dbContext.Expenses.FindAsync(id);
                    if (expense != null)
                    {
                        _dbContext.Expenses.Remove(expense);

                        //Move to pub/sub
                        //Account account = await _accountRepository.GetAccountAsync();
                        //account.Balance += expense.Amount;

                        //result = await _dbContext.SaveChangesAsync();
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
