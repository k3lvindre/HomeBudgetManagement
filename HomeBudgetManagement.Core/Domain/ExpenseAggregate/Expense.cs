namespace HomeBudgetManagement.Core.Domain.ExpenseAggregate
{
    //[Table("Expense")] //commented out so all edm should only be configure
    //inside dbcontext so it is easy to maintain and for separation of concern
    public class Expense : BaseEntity
    {
        public required string Description { get; set; }
        public required string Type { get; set; }
        public double Amount { get; set; }
        public byte[]? File { get; set; }
        public string? FileExtension { get; set; }
        public int AccountId { get; set; }
    }
}
