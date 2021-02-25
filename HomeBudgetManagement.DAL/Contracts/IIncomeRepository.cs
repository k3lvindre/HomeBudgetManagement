using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public interface IIncomeRepository : IDisposable
    {
        Task<Income> CreateAsync(Income entity);
        Task<int> UpdateAsync(Income entity);
        Task<int> DeleteAsync(Income identity);
        Task<int> DeleteRangeAsync(List<Income> identity);
        Task<Income> GetAsync(int? id);
        Task<List<Income>> GetAllAsync();
        Task<int> CreateRangeAsync(IList<Income> entities);
        Task<List<Income>> ExecuteQueryAsync(string sql);
        Task<List<Income>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects);
    }
}
