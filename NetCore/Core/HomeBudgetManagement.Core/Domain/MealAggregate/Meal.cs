using HomeBudgetManagement.SharedKernel;

namespace HomeBudgetManagement.Core.Domain.MealAggregate
{
    //[Table("Income")] //commented out so all edm(entity data model) should only be configure
    //inside dbcontext so it is easy to maintain and for separation of concern

    public class Meal : BaseEntity, IAggregateRoot
    {
        public FileAttachment? FileAttachment { get; set; }
    }
}
