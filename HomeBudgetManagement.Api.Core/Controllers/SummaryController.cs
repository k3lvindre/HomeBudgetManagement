using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("SummaryReport")]
    [ApiController]
    public class SummaryController : Controller
    {
        private readonly IIncomeSummary _incomeSummary;

        public SummaryController(IIncomeSummary incomeSummary)
        {
            _incomeSummary = incomeSummary;
        }

        [HttpGet("GetIncomeByMonth/{month}")]
        public async Task<IActionResult> Index(int month)
        {
            return Ok(await _incomeSummary.GetItemsByMonthAsync(month));
        }
    }
}
