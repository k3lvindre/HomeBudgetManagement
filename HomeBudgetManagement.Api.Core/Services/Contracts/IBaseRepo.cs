using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface IBaseRepo<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        Task<int> SaveAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(List<T> entity);
        Task<int> RemoveAsync(int Id);
    }
}
