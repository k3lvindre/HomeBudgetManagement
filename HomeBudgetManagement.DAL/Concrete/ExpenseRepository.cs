﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Domain
{
    public class ExpenseRepository : IExpenseRepository
    {
        private HomeBudgetManagementContext _homeBudgetManagementContext;

        public ExpenseRepository(HomeBudgetManagementContext homeBudgetManagementContext)
        {
            _homeBudgetManagementContext = homeBudgetManagementContext;
        }

        public ExpenseRepository()
        {
            _homeBudgetManagementContext = new HomeBudgetManagementContext();
        }

        public async Task<Expense> CreateAsync(Expense entity)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Expenses.Add(entity);
                    Account account = _homeBudgetManagementContext.Accounts.FirstOrDefault();
                    account.Balance -= entity.Amount;

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

        public async Task<int> CreateRangeAsync(IList<Expense> entities)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Expenses.AddRange(entities);
                    Account account = _homeBudgetManagementContext.Accounts.FirstOrDefault();
                    account.Balance -= entities.Select(e=>e.Amount).Sum();

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

        public async Task<int> DeleteAsync(Expense identity)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    _homeBudgetManagementContext.Entry<Expense>(identity).State = EntityState.Deleted;
                    Account account = _homeBudgetManagementContext.Accounts.FirstOrDefault();
                    account.Balance += identity.Amount;

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

        public async Task<int> DeleteRangeAsync(List<Expense> range)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (Expense item in range)
                    {
                        _homeBudgetManagementContext.Entry<Expense>(item).State = EntityState.Deleted;
                    }
                    Account account = _homeBudgetManagementContext.Accounts.FirstOrDefault();
                    account.Balance += range.Select(e => e.Amount).Sum();

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

        public async Task<List<Expense>> ExecuteQueryAsync(string sql)
        {
            return await _homeBudgetManagementContext.Expenses.SqlQuery(sql).ToListAsync();
        }

        public async Task<List<Expense>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects)
        {
            return await _homeBudgetManagementContext.Expenses.SqlQuery(sql, sqlParametersObjects).ToListAsync();
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _homeBudgetManagementContext.Expenses.ToListAsync<Expense>();
        }

        public async Task<Expense> GetAsync(int? id)
        {
            return await _homeBudgetManagementContext.Expenses.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Expense entity)
        {
            using (DbContextTransaction transaction = _homeBudgetManagementContext.Database.BeginTransaction())
            {
                try
                {
                    Expense origExpense = await this.GetAsync(entity.Id);
                    Account account = _homeBudgetManagementContext.Accounts.FirstOrDefault();
                    account.Balance += origExpense.Amount;
                    origExpense.Amount = entity.Amount;
                    account.Balance -= entity.Amount;


                    int result = await _homeBudgetManagementContext.SaveChangesAsync();
                    transaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                }
            }
            return 0;

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
