using HomeBudgetManagement.Core.ValueObject;

namespace HomeBudgetManagement.Core.Domain.ExpenseAggregate
{
    //[Table("Income")] //commented out so all edm(entity data model) should only be configure
    //inside dbcontext so it is easy to maintain and for separation of concern

    //Records are value types with immutable semantics. Once a record is created, its properties cannot be modified.
    //Records are also suitable for data transfer objects (DTOs)
    public record Expense (int Id, DateTime CreatedDate, string Description, ItemType ItemType, double Amount, byte[]? File, string? FileExtension) 
            : BaseEntity(Id, CreatedDate, Description, ItemType, Amount, File, FileExtension);
}
