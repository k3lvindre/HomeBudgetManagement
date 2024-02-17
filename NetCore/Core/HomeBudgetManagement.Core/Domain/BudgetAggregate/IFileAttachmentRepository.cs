using HomeBudgetManagement.SharedKernel;

namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    public interface IFileAttachmentRepository : IGenericRepository<FileAttachment>
    {
        Task<FileAttachment> GetByBudgetIdAsync(int Id);
    }
}