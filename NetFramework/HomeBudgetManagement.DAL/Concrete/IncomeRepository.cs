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
        private IAccountRepository _accountRepository;

        public IncomeRepository(HomeBudgetManagementContext homeBudgetManagementContext)
        {
            _homeBudgetManagementContext = homeBudgetManagementContext;
            _accountRepository = new AccountRepository(_homeBudgetManagementContext);
        }

        public IncomeRepository()
        {
            _homeBudgetManagementContext = new HomeBudgetManagementContext();
            _accountRepository = new AccountRepository(_homeBudgetManagementContext);
        }

        public async Task<Income> CreateAsync(Income entity)
        {
            using(DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Incomes.Add(entity);

                    await _accountRepository.AddBalanceAsync(entity.Amount);

                    await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return entity;

        }

        public async Task<int> CreateRangeAsync(IList<Income> entities)
        {
            using(DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Incomes.AddRange(entities);

                    double totalAmount = entities.Select(e => e.Amount).CustomSum(); //used my custom extension method
                    await _accountRepository.AddBalanceAsync(totalAmount);



                    int result = await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

            }
            return 0;
        }

        public async Task<int> DeleteAsync(Income identity)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Entry<Income>(identity).State = EntityState.Deleted;

                    await _accountRepository.DeductBalanceAsync(identity.Amount);

                    int result =  await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return 0;
        }

        public async Task<int> DeleteRangeAsync(List<Income> range)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Income item in range)
                    {
                        _homeBudgetManagementContext.Entry<Income>(item).State = EntityState.Deleted;
                    }

                    await _accountRepository.DeductBalanceAsync(range.Select(e => e.Amount).CustomSum()); //used my custom extension method


                    int result = await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();

                    return result;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return 0;
        }

        public async Task<int> UpdateAsync(Income entity)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    Income origIncome = await this.GetAsync(entity.Id);

                    await _accountRepository.UpdateBalanceAsync(entity.Amount, origIncome.Amount);

                    //update expense
                    origIncome.Description = entity.Description;
                    origIncome.Date = entity.Date;
                    origIncome.File = entity.File;
                    origIncome.FileExtension = entity.FileExtension;
                    origIncome.Amount = entity.Amount;


                    int result = await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();
                    return result;

                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return 0;
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
