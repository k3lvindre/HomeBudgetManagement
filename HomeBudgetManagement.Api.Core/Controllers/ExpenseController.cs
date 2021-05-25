﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("Expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            List<Expense> expenses = await _expenseRepository.GetAllAsync();
            if (expenses.Any())
            {
                return Ok(expenses);
            }
            else return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBydId(int id)
        {
            Expense expense =  await _expenseRepository.GetByIdAsync(id);
            if(expense == null)
            {
               return  NotFound();
            }
            else  return Ok(expense);
        }

        [HttpPost("PostExpense")]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense)
        {
            expense =  await _expenseRepository.AddAsync(expense);
            if (expense.Id > 0)
            {
                return Accepted();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateExpense")]
        public async Task<IActionResult> UpdateExpense([FromBody] Expense expense)
        {
            int result = await _expenseRepository.SaveAsync(expense);

            if (result > 0)
            {
                return Accepted();
            }
            else return BadRequest();

        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int result = await _expenseRepository.RemoveAsync(id);

            if (result > 0)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}