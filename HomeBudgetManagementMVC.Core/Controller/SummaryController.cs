using HomeBudgetManagement.MVC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.MVC.Core
{
    public class SummaryController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly HomeBudgetManagementApiConfig _homeBudgetManagementApiConfig;

        public SummaryController(IOptions<HomeBudgetManagementApiConfig> homeBudgetManagementApiConfig)
        {
            _homeBudgetManagementApiConfig = homeBudgetManagementApiConfig.Value;
            if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(_homeBudgetManagementApiConfig.Address);
        }

        [HttpGet]
        public IActionResult IncomeSummaryReport(List<Income> incomes)
        {
            return View(incomes);
        }

        [HttpGet]
        public async Task<IActionResult> IncomeFilterByMonthReport(int month)
        {
            List<Income> incomes = new List<Income>();
            HttpResponseMessage result = await _httpClient.GetAsync($"SummaryReport/GetIncomeByMonth/{month}");

            if(result.IsSuccessStatusCode)
            {
                 incomes = JsonSerializer.Deserialize<List<Income>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }

            ViewBag.Total = incomes.Sum(x => x.Amount);

            return View("IncomeSummaryReport", incomes);
        }

        [HttpGet]
        public IActionResult IncomeFilterByDateRange(DateTime dateFrom, DateTime dateTo)
        {
            return View("IncomeSummaryReport", new List<Income>());
        }
    }
}
