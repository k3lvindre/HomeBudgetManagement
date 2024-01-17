namespace HomeBudgetManagement.Core.Domain.AccountAggregate
{
    public class Account : BaseEntity
    {
        public required string AccountName { get; set; }
        public required double Balance { get; set; }
    }
}
