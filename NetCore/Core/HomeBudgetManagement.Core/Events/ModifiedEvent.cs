using HomeBudgetManagement.Core.Domain.BudgetAggregate;
using MediatR;

namespace HomeBudgetManagement.Core.Events
{
    public class ModifiedEvent<T> : INotification
    {
        public T Entity { get; }

        public ModifiedEvent(T entity)
        {
            Entity = entity;
        }
    }
}
