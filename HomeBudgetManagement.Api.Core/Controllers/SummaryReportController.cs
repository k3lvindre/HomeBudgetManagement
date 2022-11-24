using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/SummaryReports")]
    [ApiController]
    public class SummaryReportController : Controller
    {
        private readonly IIncomeSummary _incomeSummary;

        public SummaryReportController(IIncomeSummary incomeSummary)
        {
            _incomeSummary = incomeSummary;
        }

        [HttpGet("{month:int}")]
        public async Task<IActionResult> Index(int month)
        {
            return Ok(await _incomeSummary.GetItemsByMonthAsync(month));
        }

        [HttpGet("month/{from}/{to}")]
        public async Task<IActionResult> Index(DateTime from, DateTime to)
        {
            return Ok(await _incomeSummary.GetItemsByDateRangeAsync(from, to));
        }
    }
}
