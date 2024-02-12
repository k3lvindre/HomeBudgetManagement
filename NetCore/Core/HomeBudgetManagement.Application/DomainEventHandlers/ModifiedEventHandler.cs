using HomeBudgetManagement.Core.Events;
using HomeBudgetManagement.SharedKernel;
using MediatR;

namespace HomeBudgetManagement.Application.DomainEventHandlers
{
    //you can also use event bus here to publish events for mircorservice in case you have one.
    //that is called integration events
    //but the example here is called domain events
    public class ModifiedEventHandler<T> : INotificationHandler<ModifiedEvent<T>>
    {
        private readonly IEventFeed _eventFeed;

        public ModifiedEventHandler(IEventFeed eventFeed)
        {
            _eventFeed = eventFeed;
        }

        public async Task Handle(ModifiedEvent<T> notification, CancellationToken cancellationToken)
        {

            await _eventFeed.PublishAsync(nameof(ModifiedEvent<T>), notification.Entity!);
        }
    }
}
