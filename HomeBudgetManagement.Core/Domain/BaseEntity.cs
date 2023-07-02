namespace HomeBudgetManagement.Core.Domain
{
    public abstract class BaseEntity
    {
        public required int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
