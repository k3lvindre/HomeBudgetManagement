using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
        Task<Expense> GetByIdAsync(int Id);
        Task<int> SaveAsync(Expense expense);
        Task<Expense> AddAsync(Expense expense);
        Task<int> AddRangeAsync(List<Expense> expense);
        Task<int> RemoveAsync(int Id);
    }
}
