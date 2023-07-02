using HomeBudgetManagement.Core.Domain.ExpenseAggregate;

namespace HomeBudgetManagement.Application.Repository
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetExpenseByTypeAsync(string type);
    }
}
