using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HomeBudgetManagement.Models;
using System.Configuration;
using System.Text;

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
            _client.DefaultRequestHeaders.Add("api-key", "12345");

            byte[] authToken = Encoding.ASCII.GetBytes("kelvin:password");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

            HttpResponseMessage result = await _client.GetAsync("Account/Get");
            if (result.IsSuccessStatusCode)
            {
                Account account = JsonSerializer.Deserialize<Account>(await result.Content.ReadAsStringAsync());
                return account;
            }
            return null;
        }


        public async Task<Account> GetAccountV2Async()
        {
           HttpResponseMessage result = await _client.GetAsync("Account/GetAccount");
           if (result.IsSuccessStatusCode)
           {
               Account account = JsonSerializer.Deserialize<Account>(await result.Content.ReadAsStringAsync());
               return account;
           }

            return null;
        }
    }
}
