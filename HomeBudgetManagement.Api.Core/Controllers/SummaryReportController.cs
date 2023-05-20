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
        private readonly ISummary<Expense> _summary;

        public SummaryReportController(ISummary<Expense> incomeSummary)
        {
            _summary = incomeSummary;
        }

        [HttpGet("{month:int}")]
        public async Task<IActionResult> Index(int month)
        {
            return Ok(await _summary.GetItemsByMonthAsync(month));
        }

        [HttpGet("month/{from}/{to}")]
        public async Task<IActionResult> Index(DateTime from, DateTime to)
        {
            return Ok(await _summary.GetItemsByDateRangeAsync(from, to));
        }
    }
}
