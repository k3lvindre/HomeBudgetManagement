using HomeBudgetManagement.Application.Repository;
using HomeBudgetManagement.Core.Domain.ExpenseAggregate;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetManagement.Infrastructure.EntityFramework.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly HomeBudgetManagementDbContext _context;

        public ExpenseRepository(HomeBudgetManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetExpenseByTypeAsync(string type)
        {
            return await _context.Expenses.Where(x => x.Type == type).ToListAsync<Expense>();
        }
    }

    //public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    //{
    //    public ExpenseRepository(HomeBudgetManagementDbContext dbContext) : base(dbContext)
    //    {
    //    }

    //    //sample override
    //    public override int Add(Expense entity)
    //    {
    //        return base.Add(entity);
    //    }

    //    //public async Task<List<Expense>> GetAllAsync()
    //    //{
    //    //    return await _context.Expenses.ToListAsync();
    //    //}

    //    //public async Task<Expense> AddAsync(Expense expense)
    //    //{

    //    //    using (IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync())
    //    //    {
    //    //        try
    //    //        {
    //    //            await _context.Expenses.AddAsync(expense);

    //    //            //Move to pub/sub
    //    //            //Account account = await _accountRepository.GetAccountAsync();
    //    //            //account.Balance -= expense.Amount;

    //    //            await _context.SaveChangesAsync();
    //    //            await dbContextTransaction.CommitAsync();
    //    //        }
    //    //        catch (Exception)
    //    //        {
    //    //           await dbContextTransaction.RollbackAsync();
    //    //        }
    //    //    }

    //    //    return expense;
    //    //}

    //    //public async Task<bool> AddRangeAsync(List<Expense> expenses)
    //    //{
    //    //    using (IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync())
    //    //    {
    //    //        try
    //    //        {
    //    //            await _context.Expenses.AddRangeAsync(expenses);


    //    //            //Move to pub/sub
    //    //            //Account account = await _accountRepository.GetAccountAsync();
    //    //            //account.Balance -= expenses.Sum(x => x.Amount);

    //    //            await _context.SaveChangesAsync();
    //    //            await dbContextTransaction.CommitAsync();
    //    //        }
    //    //        catch (Exception)
    //    //        {
    //    //            await dbContextTransaction.RollbackAsync();
    //    //            return false;
    //    //        }
    //    //    }

    //    //    return true;
    //    //}

    //    //public async Task<Expense> GetByIdAsync(int Id)
    //    //{
    //    //    return await _context.Expenses.FindAsync(Id);
    //    //}

    //    //public async Task<bool> SaveAsync(Expense expense)
    //    //{
    //    //    using (IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync())
    //    //    {
    //    //        try
    //    //        {
    //    //            Expense expenseFromDb = await _context.Expenses.FindAsync(expense.Id);
    //    //            EntityEntry<Expense> entry = _context.Entry<Expense>(expenseFromDb);


    //    //            //Move to pub/sub
    //    //            //Account account = await _accountRepository.GetAccountAsync();
    //    //            ////Add the original balance for correct balance calculation
    //    //            //account.Balance += Convert.ToDouble(entry.OriginalValues["Amount"]);
    //    //            //account.Balance -= expense.Amount;

    //    //            entry.Property(x => x.Amount).CurrentValue = expense.Amount;
    //    //            entry.State = EntityState.Modified;

    //    //            await _context.SaveChangesAsync();
    //    //            await dbContextTransaction.CommitAsync();
    //    //        }
    //    //        catch (Exception)
    //    //        {

    //    //            await dbContextTransaction.RollbackAsync();
    //    //            return false;
    //    //        }
    //    //    }

    //    //    return true;
    //    //}

    //    //public async Task<bool> RemoveAsync(int id)
    //    //{
    //    //    using (IDbContextTransaction dbContextTransaction = await _context.Database.BeginTransactionAsync())
    //    //    {
    //    //        try
    //    //        {
    //    //            Expense expense = await _context.Expenses.FindAsync(id);
    //    //            if (expense != null)
    //    //            {
    //    //                _context.Expenses.Remove(expense);

    //    //                //Move to pub/sub
    //    //                //Account account = await _accountRepository.GetAccountAsync();
    //    //                //account.Balance += expense.Amount;

    //    //                //result = await _context.SaveChangesAsync();
    //    //                await dbContextTransaction.CommitAsync();
    //    //            }
    //    //        }
    //    //        catch (Exception)
    //    //        {
    //    //            await dbContextTransaction.RollbackAsync();
    //    //            return false;
    //    //        }
    //    //    }

    //    //    return true;
    //    //}
    //}
}
