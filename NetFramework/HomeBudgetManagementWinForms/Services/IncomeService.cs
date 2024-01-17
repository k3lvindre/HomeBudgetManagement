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
    public class IncomeService
    {
        static HttpClient httpClient;
        public IncomeService()
        {
            httpClient =  new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationSettings.AppSettings.Get("HomeBudgetApi"));
        }

        public async Task<List<Income>> GetAllIncomesAsync()
        {
            List<Income> Incomes = new List<Income>();
          
            HttpResponseMessage result = await httpClient.GetAsync("Income/List");

            if(result.IsSuccessStatusCode)
            {
              Incomes =  JsonSerializer.Deserialize<List<Income>>(await result.Content.ReadAsStringAsync());
            }
            return Incomes;
        }

        public async Task<Income> GetById(int id)
        {
            Income Incomes = new Income();
          
            HttpResponseMessage result = await httpClient.GetAsync("Income/" + id);

            if(result.IsSuccessStatusCode)
            {
              Incomes =  JsonSerializer.Deserialize<Income>(await result.Content.ReadAsStringAsync());
            }

            return Incomes;
        }

        public async Task<Income> CreateIncome(Income Income)
        {
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Income>(Income),Encoding.Default,"application/json");

            HttpResponseMessage result = await httpClient.PostAsync("Income/PostIncome", param);

            if(result.IsSuccessStatusCode)
            {
              Income =  JsonSerializer.Deserialize<Income>(await result.Content.ReadAsStringAsync());
            }

            return Income;
        }

        public async Task<bool> UpdateIncome(Income Income)
        {
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent param = new StringContent(JsonSerializer.Serialize<Income>(Income),Encoding.Default,"application/json");

            HttpResponseMessage result = await httpClient.PutAsync("Income/UpdateIncome", param);

            if(result.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteIncome(int id)
        {
            
            HttpResponseMessage result = await httpClient.DeleteAsync($"Income/Delete/{id}");

            if(result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteRangeIncome(List<Income> Incomes)
        {
            
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent param = new StringContent(JsonSerializer.Serialize<List<Income>>(Incomes),Encoding.Default,"application/json");

            HttpResponseMessage result = await httpClient.PostAsync("Income/DeleteRange", param);

            if(result.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<byte[]> DownloadFile(int id)
        {
            HttpResponseMessage result = await httpClient.GetAsync("Income/downloadfile/" + id);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsByteArrayAsync();
            }

            return null;
        }

    }
}
