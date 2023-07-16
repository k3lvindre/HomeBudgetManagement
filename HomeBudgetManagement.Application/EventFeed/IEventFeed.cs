
namespace HomeBudgetManagement.Application.EventFeed
{
    public interface IEventFeed
    {
        Task PublishAsync(string eventName, object content);
        Task<List<object>> GetByNameAsync(string eventName);
    }
}
