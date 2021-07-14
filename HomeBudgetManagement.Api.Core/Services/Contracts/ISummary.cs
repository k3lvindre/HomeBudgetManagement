using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface ISummary<T> where T: class
    {
        Task<List<T>> GetItemsByMonthAsync(int month);
        Task<List<T>> GetItemsByDateRangeAsync(DateTime from, DateTime to);
    }
}
