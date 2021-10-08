using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HomeBudgetManagement.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Configuration;
namespace HomeBudgetManagementWinForms.Services
{
    public class ExpenseService
    {
        static HttpClient httpClient;
        public ExpenseService()
        {
            httpClient =  new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings.Get("HomeBudgetApi"));
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            List<Expense> expenses = new List<Expense>();
            using (httpClient)
            {
                HttpResponseMessage result = await httpClient.GetAsync("Expense/List");

                if(result.IsSuccessStatusCode)
                {
                  expenses =  JsonSerializer.Deserialize<List<Expense>>(await result.Content.ReadAsStringAsync());
                }
            }
            return expenses;
        }

        public async Task<Expense> GetById(int id)
        {
            Expense expenses = new Expense();
            using (httpClient)
            {
                HttpResponseMessage result = await httpClient.GetAsync("Expense/" + id);

                if(result.IsSuccessStatusCode)
                {
                  expenses =  JsonSerializer.Deserialize<Expense>(await result.Content.ReadAsStringAsync());
                }
            }
            return expenses;
        }

        public async Task<Expense> CreateExpense(Expense expense)
        {
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent param = new StringContent(JsonSerializer.Serialize<Expense>(expense),Encoding.Default,"application/json");

                HttpResponseMessage result = await httpClient.PostAsync("Expense/PostExpense", param);

                if(result.IsSuccessStatusCode)
                {
                  expense =  JsonSerializer.Deserialize<Expense>(await result.Content.ReadAsStringAsync());
                }
            }
            return expense;
        }

        public async Task<bool> UpdateExpense(Expense expense)
        {
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent param = new StringContent(JsonSerializer.Serialize<Expense>(expense),Encoding.Default,"application/json");

                HttpResponseMessage result = await httpClient.PutAsync("Expense/UpdateExpense", param);

                if(result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteExpense(int id)
        {
            using (httpClient)
            {
                HttpResponseMessage result = await httpClient.DeleteAsync($"Expense/Delete/{id}");

                if(result.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> DeleteRangeExpense(List<Expense> expenses)
        {
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpContent param = new StringContent(JsonSerializer.Serialize<List<Expense>>(expenses),Encoding.Default,"application/json");

                HttpResponseMessage result = await httpClient.PostAsync("Expense/DeleteRange", param);

                if(result.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<byte[]> DownloadFile(int id)
        {
            using (httpClient)
            {
                HttpResponseMessage result = await httpClient.GetAsync("Expense/downloadfile/" + id);

                if (result.IsSuccessStatusCode)
                {
                    return  await result.Content.ReadAsByteArrayAsync();
                }
            }
            return null;
        }
    }
}
