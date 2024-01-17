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
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace HomeBudgetManagement.MVC.Core
{
    public class AccountController : Controller
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly HomeBudgetManagementApiConfig _homeBudgetManagementApiConfig;

        public AccountController(IOptions<HomeBudgetManagementApiConfig> homeBudgetManagementApiConfig)
        {
            _homeBudgetManagementApiConfig = homeBudgetManagementApiConfig.Value;
            if(_httpClient.BaseAddress == null) _httpClient.BaseAddress = new Uri(_homeBudgetManagementApiConfig.Address);
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {

            HttpResponseMessage result = await _httpClient.GetAsync("Account/GetAccount");
            if (result.IsSuccessStatusCode)
            {
                Account account = JsonSerializer.Deserialize<Account>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(new AccountViewModel() { Id = account.Id, Balance = account.Balance, BalanceToAdd = 0 });
            } else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Account(AccountViewModel model)
        {

            Account account = new Account()
            {
                Id = model.Id,
                Balance = model.Balance + model.BalanceToAdd
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_homeBudgetManagementApiConfig.MediaType));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Account>(account), Encoding.Default, _homeBudgetManagementApiConfig.MediaType);

            HttpResponseMessage result = await _httpClient.PutAsync("Account/UpdateAccount", param);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Account");
            }
            else
            {
                return View("Error", new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
