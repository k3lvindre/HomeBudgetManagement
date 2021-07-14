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
    public class SummaryReportController : Controller
    {
        private readonly IIncomeSummary _incomeSummary;

        public SummaryReportController(IIncomeSummary incomeSummary)
        {
            _incomeSummary = incomeSummary;
        }

        [HttpGet("GetIncomeByMonth/{month}")]
        public async Task<IActionResult> Index(int month)
        {
            return Ok(await _incomeSummary.GetItemsByMonthAsync(month));
        }

        [HttpPost("GetIncomeByDateRange")]
        public async Task<IActionResult> Index(DateTime from, DateTime to)
        {
            return Ok(await _incomeSummary.GetItemsByDateRangeAsync(from, to));
        }
    }
}
