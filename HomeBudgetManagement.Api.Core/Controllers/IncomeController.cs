using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("income")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAccountRepository _accountRepository;

        public IncomeController(IIncomeRepository incomeRepository,
                                IAccountRepository accountRepository)
        {
            _incomeRepository = incomeRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            List<Income> incomes = await _incomeRepository.GetAllAsync();
            if (incomes.Any())
            {
                return Ok(incomes);
            }
            else return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBydId(int id)
        {
            Income income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            else return Ok(income);
        }

        [HttpPost("PostIncome")]
        public async Task<IActionResult> AddIncome([FromBody] Income income)
        {
            Account account = await _accountRepository.GetFirstAccountAsync();
            if (account.Balance >= income.Amount)
            {
                income = await _incomeRepository.AddAsync(income);
                if (income.Id > 0)
                {
                    return Accepted();
                }
                else return BadRequest();
            }
            else return BadRequest("Insuficient Balance!");

        }

        [HttpPut("UpdateIncome")]
        public async Task<IActionResult> UpdateIncome([FromBody] Income income)
        {
            Account account = await _accountRepository.GetFirstAccountAsync();
            if (account.Balance >= income.Amount)
            {
                int result = await _incomeRepository.SaveAsync(income);
                if (result > 0)
                {
                    return Accepted();
                }
                else return BadRequest();
            }
            else return BadRequest("Insuficient Balance!");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int result = await _incomeRepository.RemoveAsync(id);

            if (result > 0)
            {
                return Ok();
            }
            else return BadRequest();
        }

    }
}
