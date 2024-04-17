using MediatR;

namespace HomeBudgetManagement.Core.Events
{
    public class ModifiedEvent<T>(T entity) : INotification
    {
        public T Entity { get; } = entity;
    }
}
