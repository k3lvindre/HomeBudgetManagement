namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    //[Table("Income")] //commented out so all edm(entity data model) should only be configure
    //inside dbcontext so it is easy to maintain and for separation of concern

    public class Budget : BaseEntity
    {
        public FileAttachment? FileAttachment { get; set; }
    }
}
