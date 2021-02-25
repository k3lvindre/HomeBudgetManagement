using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public class IncomeRepository : IIncomeRepository
    {
        private HomeBudgetManagementContext _homeBudgetManagementContext;

        public IncomeRepository(HomeBudgetManagementContext homeBudgetManagementContext)
        {
            _homeBudgetManagementContext = homeBudgetManagementContext;
        }

        public IncomeRepository()
        {
            _homeBudgetManagementContext = new HomeBudgetManagementContext();
        }

        public async Task<Income> CreateAsync(Income entity)
        {
            _homeBudgetManagementContext.Incomes.Add(entity);
            await _homeBudgetManagementContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CreateRangeAsync(IList<Income> entities)
        {
            _homeBudgetManagementContext.Incomes.AddRange(entities);
            return await _homeBudgetManagementContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Income identity)
        {
            _homeBudgetManagementContext.Entry<Income>(identity).State = EntityState.Deleted;
            return await _homeBudgetManagementContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(List<Income> range)
        {
            foreach (Income item in range)
            {
                _homeBudgetManagementContext.Entry<Income>(item).State = EntityState.Deleted;
            }
            return await _homeBudgetManagementContext.SaveChangesAsync();
        }

        public async Task<List<Income>> ExecuteQueryAsync(string sql)
        {
            return await _homeBudgetManagementContext.Incomes.SqlQuery(sql).ToListAsync();
        }

        public async Task<List<Income>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects)
        {
            return await _homeBudgetManagementContext.Incomes.SqlQuery(sql, sqlParametersObjects).ToListAsync();
        }

        public async Task<List<Income>> GetAllAsync()
        {
            return await _homeBudgetManagementContext.Incomes.ToListAsync<Income>();
        }

        public async Task<Income> GetAsync(int? id)
        {
            return await _homeBudgetManagementContext.Incomes.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Income entity)
        {
             _homeBudgetManagementContext.Entry<Income>(entity).State = EntityState.Modified;
             return await _homeBudgetManagementContext.SaveChangesAsync();
        }

        //Implement Idisposable,  We can also implement "`Finalize" to override GC memory handling but it take some time.
        //by implementing idisposable we let consumer handle the disposing/memory handling
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_homeBudgetManagementContext != null)
                {
                    _homeBudgetManagementContext.Dispose();
                    _homeBudgetManagementContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
