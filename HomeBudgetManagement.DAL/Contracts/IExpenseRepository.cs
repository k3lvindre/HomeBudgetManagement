using System.Collections.Generic;
using System.Threading.Tasks;
using HomeBudgetManagement.Models;
using System;
namespace HomeBudgetManagement.Domain
{
    //repositories design patterns
    public interface IExpenseRepository : IDisposable
    {
        Task<Expense> CreateAsync(Expense entity);
        Task<int> UpdateAsync(Expense entity);
        Task<int> DeleteAsync(Expense identity);
        Task<int> DeleteRangeAsync(List<Expense> identity);
        Task<Expense> GetAsync(int? id);
        Task<List<Expense>> GetAllAsync();
        Task<int> CreateRangeAsync(IList<Expense> entities);
        Task<List<Expense>> ExecuteQueryAsync(string sql);
        Task<List<Expense>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects);
    }
}
