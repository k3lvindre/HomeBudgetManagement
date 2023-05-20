using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Data;
using HomeBudgetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class Summary : ISummary<Expense>
    {
        private readonly HomeBudgetManagementDbContext _dbContext;

        public Summary(HomeBudgetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Expense>> GetItemsByDateRangeAsync(DateTime from, DateTime to)
        {
            //Sample logic for calling sql function in ef
            List<Expense> Expenses = await _dbContext.Expenses.FromSqlInterpolated($"Select * FROM dbo.fnGetExpenseByDateRange({from},{to})").ToListAsync();

            return Expenses;
        }

        public async Task<List<Expense>> GetItemsByMonthAsync(int month)
        {
            //Sample logic for calling sql function in ef
            List<Expense> Expenses = await _dbContext.Expenses.FromSqlInterpolated($"Select * FROM dbo.fnGetExpenseByMonth({month})").ToListAsync();

            return Expenses;
        }
    }
}
