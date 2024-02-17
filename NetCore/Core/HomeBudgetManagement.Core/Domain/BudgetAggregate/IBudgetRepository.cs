using HomeBudgetManagement.Core.ValueObject;
using HomeBudgetManagement.SharedKernel;

namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
        Task<List<Budget>> GetByTypeAsync(ItemType itemType);
    }
}