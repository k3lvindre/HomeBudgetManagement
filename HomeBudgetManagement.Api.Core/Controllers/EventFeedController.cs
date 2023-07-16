using HomeBudgetManagement.Application.EventFeed;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/eventfeeds")]
    public class EventFeedController : Controller
    {
        private readonly IEventFeed _eventFeed;

        public EventFeedController(IEventFeed eventFeed)
        {
            _eventFeed = eventFeed;
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetEventByName(string name)
        {
            var result = await _eventFeed.GetByNameAsync(name);
            return Ok(result);
        }
    }
}
