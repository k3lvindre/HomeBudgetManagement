using HomeBudgetManagement.Models;
using HomeBudgetManagement.MVC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeBudgetManagement.MVC.Core
{
    public class IncomeController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly HomeBudgetManagementApiConfig _homeBudgetManagementApiConfig;

        public IncomeController(IOptions<HomeBudgetManagementApiConfig> homeBudgetManagementApiConfig)
        {
            _homeBudgetManagementApiConfig = homeBudgetManagementApiConfig.Value;
            if (_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(_homeBudgetManagementApiConfig.Address);
        }

        [HttpGet]
        public async Task<IActionResult> Income()
        {
            List<Income> incomes = new List<Income>();
            HttpResponseMessage result = await _httpClient.GetAsync("Income/List");

            if (result.IsSuccessStatusCode)
            {
                incomes = JsonSerializer.Deserialize<List<Income>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(incomes);
            }
            else return View(incomes);

        }

        //Used get because this is being called using Url only
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"Income/Delete/{id}");

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Income");
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync($"Income/{id}");

            if (result.IsSuccessStatusCode)
            {
                Income income = JsonSerializer.Deserialize<Income>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(income);
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Income income)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_homeBudgetManagementApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Income>(income), Encoding.Default, _homeBudgetManagementApiConfig.MediaType);

            HttpResponseMessage result = await _httpClient.PutAsync($"Income/UpdateIncome", param);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit", new { id = income.Id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Income income)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_homeBudgetManagementApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Income>(income), Encoding.Default, _homeBudgetManagementApiConfig.MediaType);

            HttpResponseMessage result = await _httpClient.PostAsync($"Income/PostIncome", param);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Income");
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Income());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}
