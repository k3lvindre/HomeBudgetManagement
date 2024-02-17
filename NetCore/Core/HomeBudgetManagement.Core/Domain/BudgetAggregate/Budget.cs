using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using HomeBudgetManagement.Core.ValueObject;

namespace HomeBudgetManagement.Core.Domain.BudgetAggregate
{
    //[Table("Income")] //commented out so all edm(entity data model) should only be configure
    //inside dbcontext so it is easy to maintain and for separation of concern

    //Records are value types with immutable semantics. Once a record is created, its properties cannot be modified.
    //Records are also suitable for data transfer objects (DTOs)
    public record Budget (int Id, string Description, double Amount, ItemType ItemType, DateTime CreatedDate, FileAttachment? FileAttachment) 
            : BaseEntity(Id, Description, Amount, ItemType, CreatedDate);
}
