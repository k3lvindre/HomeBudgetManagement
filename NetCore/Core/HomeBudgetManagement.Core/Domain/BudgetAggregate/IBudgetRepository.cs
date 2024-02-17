using HomeBudgetManagement.SharedKernel;
using HomeBudgetManagement.SharedKernel.ValueObjects;

namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
        Task<List<Budget>> GetByTypeAsync(ItemType itemType);
    }
}