using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Data;
using HomeBudgetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class IncomeSummary : IIncomeSummary
    {
        private readonly HomeBudgetManagementDbContext _dbContext;

        public IncomeSummary(HomeBudgetManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Income>> GetItemsByDateRangeAsync(DateTime from, DateTime to)
        {
            //Sample logic for calling sql function in ef
            List<Income> incomes = await _dbContext.Incomes.FromSqlInterpolated($"Select * FROM dbo.fnGetIncomeByDateRange({from},{to})").ToListAsync();

            return incomes;
        }

        public async Task<List<Income>> GetItemsByMonthAsync(int month)
        {
            //Sample logic for calling sql function in ef
            List<Income> incomes = await _dbContext.Incomes.FromSqlInterpolated($"Select * FROM dbo.fnGetIncomeByMonth({month})").ToListAsync();

            return incomes;
        }
    }
}
