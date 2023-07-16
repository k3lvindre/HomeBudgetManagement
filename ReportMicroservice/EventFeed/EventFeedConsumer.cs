using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ReportMicroservice.EventFeed
{
    internal class EventFeedConsumer : IEvenFeedConsumer
    {

        static HttpClient _client;
        private readonly ILogger<EventFeedConsumer> _logger;

        public EventFeedConsumer(HttpClient httpClient, ILogger<EventFeedConsumer> logger)
        {
            _client = httpClient;
            _logger = logger;
            _client.BaseAddress = new Uri("http://localhost:4555/api/eventfeeds");
        }
        public async Task ReadEventsAsync()
        {
            try
            {
                var result = await _client.GetAsync($"{_client.BaseAddress}/name/ExpenseModifiedEvent");
                _logger.LogInformation(result.IsSuccessStatusCode.ToString());

                if (result.IsSuccessStatusCode)
                {
                    var obj = result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
