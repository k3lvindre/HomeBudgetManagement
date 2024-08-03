namespace HomeBudgetManagement.Core.Domain.MealAggregate
{
    //Records are value types with immutable semantics. Once a record is created, its properties cannot be modified.
    //Records are also suitable for data transfer objects (DTOs)
    public record FileAttachment(int Id, byte[] Content, string FileExtension);
}
