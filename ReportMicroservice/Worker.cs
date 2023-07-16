using ReportMicroservice.EventFeed;

namespace ReportMicroservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEvenFeedConsumer _evenFeedConsumer;

        public Worker(ILogger<Worker> logger, IEvenFeedConsumer evenFeedConsumer)
        {
            _logger = logger;
            _evenFeedConsumer = evenFeedConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _evenFeedConsumer.ReadEventsAsync();
                await Task.Delay(3000, stoppingToken);
            }
        }
    }
}