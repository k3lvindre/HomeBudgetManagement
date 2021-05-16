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
    }
}
