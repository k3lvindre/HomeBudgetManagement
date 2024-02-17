namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    public record FileAttachment(int Id, int BudgetId, byte[] Content, string FileExtension);
}
