namespace ReportMicroservice.EventFeed
{
    public interface IEvenFeedConsumer
    {
        Task ReadEventsAsync();
    }
}
