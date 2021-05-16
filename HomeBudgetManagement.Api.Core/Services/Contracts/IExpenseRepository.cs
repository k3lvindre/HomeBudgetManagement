using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Api.Core.Services
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetAllAsync();
    }
}
