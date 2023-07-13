using HomeBudgetManagement.Application.EventFeed;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using System.Text;

namespace HomeBudgetManagement.Infrastructure.EventFeed
{
    internal class EventFeed : IEventFeed
    {
        IEventStoreConnection conn;
        public async Task ConnectAsync()
        {
            var conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
            await conn.ConnectAsync();
        }

        public async Task PublishAsync(object content)
        {
            const string streamName = "newstream";
            const string eventType = "event-type";
            const string data = "{ \"a\":\"2\"}";
            const string metadata = "{}";

            var eventPayload = new EventData(
                eventId: Guid.NewGuid(),
                type: eventType,
                isJson: true,
                data: Encoding.UTF8.GetBytes(,
                metadata: Encoding.UTF8.GetBytes(metadata)
            );
            var result = await conn.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventPayload);
        }
    }
}
