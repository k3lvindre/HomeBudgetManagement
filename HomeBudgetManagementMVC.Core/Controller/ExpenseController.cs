using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net.Http;
using System;
using System.Text;
using System.Text.Json;
using HomeBudgetManagement.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using HomeBudgetManagement.MVC.Core.Models;
using System.Net.Http.Headers;

namespace HomeBudgetManagement.MVC.Core
{
    public class ExpenseController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly HomeBudgetManagementApiConfig _homeBudgetManagementApiConfig;

        public ExpenseController(IOptions<HomeBudgetManagementApiConfig> homeBudgetManagementApiConfig)
        {
            _homeBudgetManagementApiConfig = homeBudgetManagementApiConfig.Value;
            if(_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(_homeBudgetManagementApiConfig.Address);
        }

        [HttpGet]
        public async Task<IActionResult>  Expense()
        {
            List<Expense> expenses = null;

            HttpResponseMessage result = await  _httpClient.GetAsync("Expense/List");

            if(result.IsSuccessStatusCode)
            {
                expenses = JsonSerializer.Deserialize<List<Expense>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }

            return View(expenses);
        }

        [HttpDelete]
        public async Task<IActionResult>  Delete(int id)
        {
            HttpResponseMessage result = await  _httpClient.DeleteAsync($"Expense/Delete/{id}");

            if(result.IsSuccessStatusCode)
            {
                return RedirectToAction("Expense");
            } else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

         
        [HttpGet]
        public async Task<IActionResult>  Edit(int id)
        {
            HttpResponseMessage result = await  _httpClient.GetAsync($"Expense/{id}");

            if(result.IsSuccessStatusCode)
            {
                Expense expense = JsonSerializer.Deserialize<Expense>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(expense);
            } else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
                 
        [HttpPost]
        public async Task<IActionResult>  Edit(Expense expense)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_homeBudgetManagementApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Expense>(expense), Encoding.Default, _homeBudgetManagementApiConfig.MediaType);

            HttpResponseMessage result = await  _httpClient.PutAsync($"Expense/UpdateExpense", param);

            if(result.IsSuccessStatusCode)
            {
                return View(expense);
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }       
        
        [HttpPost]
        public async Task<IActionResult>  Add(Expense expense)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_homeBudgetManagementApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Expense>(expense), Encoding.Default, _homeBudgetManagementApiConfig.MediaType);

            HttpResponseMessage result = await  _httpClient.PostAsync($"Expense/PostExpense", param);

            if(result.IsSuccessStatusCode)
            {
                return RedirectToAction("Expense");
            } else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Expense());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}
