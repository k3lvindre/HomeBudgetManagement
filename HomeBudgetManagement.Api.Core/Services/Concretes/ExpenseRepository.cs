using HomeBudgetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly HomeBudgetManagementDbContext _dbContext;

        public ExpenseRepository(HomeBudgetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      
        public async Task<List<Expense>> GetAllAsync()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task<Expense> AddAsync(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
            await _dbContext.SaveChangesAsync();
            return expense;
        }

        public async Task<int> AddRangeAsync(List<Expense> expense)
        {
            await _dbContext.Expenses.AddRangeAsync(expense);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Expense> GetByIdAsync(int Id)
        {
            return await _dbContext.Expenses.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveAsync(Expense expense)
        {
            _dbContext.Entry(expense).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveAsync(int id)
        {
            Expense expense = await _dbContext.Expenses.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (expense != null)
            {
                _dbContext.Expenses.Remove(expense);
                return await _dbContext.SaveChangesAsync();
            }
            else return 0;
        }
    }
}
