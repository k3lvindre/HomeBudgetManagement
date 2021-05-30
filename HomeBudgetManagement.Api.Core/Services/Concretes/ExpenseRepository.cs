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
       

        public ExpenseRepository(
            HomeBudgetManagementDbContext dbContext
        )
        {
            _dbContext = dbContext;
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
                    
                    Account account = await _dbContext.Account.FirstOrDefaultAsync();
                    account.Balance -= expense.Amount;

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

        public async Task<int> AddRangeAsync(List<Expense> expenses)
        {
            int result = 0;

            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbContext.Expenses.AddRangeAsync(expenses);

                    Account account = await _dbContext.Account.FirstOrDefaultAsync();
                    account.Balance -= expenses.Sum(x => x.Amount);

                    result = await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                }
            }

            return result;
        }

        public async Task<Expense> GetByIdAsync(int Id)
        {
            return await _dbContext.Expenses.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(Expense expense)
        {
            int result = 0;

            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Expense expenseFromDb = _dbContext.Expenses.Find(expense.Id);
                    EntityEntry<Expense> entry = _dbContext.Entry<Expense>(expenseFromDb);
                
                    Account account = await _dbContext.Account.FirstOrDefaultAsync();
                    //Add the original balance for correct balance calculation
                    account.Balance += Convert.ToDouble(entry.OriginalValues["Amount"]);
                    account.Balance -= expense.Amount;

                    entry.Property(x => x.Amount).CurrentValue = expense.Amount;
                    entry.State = EntityState.Modified;

                    result = await _dbContext.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception)
                {

                    await dbContextTransaction.RollbackAsync();
                }
            }

            return result;
        }

        public async Task<int> RemoveAsync(int id)
        {
            int result = 0;
            using (IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Expense expense = await _dbContext.Expenses.Where(x => x.Id == id).FirstOrDefaultAsync();
                    if (expense != null)
                    {
                        _dbContext.Expenses.Remove(expense);

                        Account account = await _dbContext.Account.FirstOrDefaultAsync();
                        account.Balance += expense.Amount;

                        result = await _dbContext.SaveChangesAsync();
                        await dbContextTransaction.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await dbContextTransaction.RollbackAsync();
                }
            }

            return result;
        }
    }
}
