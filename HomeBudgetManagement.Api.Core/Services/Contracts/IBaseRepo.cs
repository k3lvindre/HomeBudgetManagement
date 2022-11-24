using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Services
{
    //Unlike in HomeBudgetManagement.API project here we removed the implementation of IDisposable because .net core handles the disposal by adding scoped or transient lifetime in startup
    public interface IBaseRepo<T> where T : class //: IDisposable  
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        Task<bool> SaveAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entity);
        Task<bool> RemoveAsync(int Id);
    }
}
