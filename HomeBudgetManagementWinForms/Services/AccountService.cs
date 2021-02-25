using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using HomeBudgetManagement.Models;
using System.Configuration;

namespace HomeBudgetManagementWinForms.Services
{
    public class AccountService
    {
        static HttpClient _client;
        public AccountService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(ConfigurationSettings.AppSettings.Get("HomeBudgetApi"));
        }


        public async Task<Account> GetAccountAsync()
        {
            using (_client)
            {
                HttpResponseMessage result = await _client.GetAsync("api/Account/Get");
                if (result.IsSuccessStatusCode)
                {
                    Account account = JsonSerializer.Deserialize<Account>(await result.Content.ReadAsStringAsync());
                    return account;
                }
            }
           
            return null;
        }
    }
}
