using HomeBudgetManagement.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/eventfeeds")]
    public class EventFeedController(IEventFeed eventFeed) : Controller
    {
        private readonly IEventFeed _eventFeed = eventFeed;

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetEventByName(string name)
            => Ok(await _eventFeed.GetByNameAsync(name));
    }
}
