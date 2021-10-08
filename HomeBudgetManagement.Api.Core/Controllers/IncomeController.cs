using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

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
            Account account = await _accountRepository.GetAccountAsync();
            income = await _incomeRepository.AddAsync(income);
            if (income.Id > 0)
            {
                return Accepted();
            }
            else return BadRequest();
        }

        [HttpPut("UpdateIncome")]
        public async Task<IActionResult> UpdateIncome([FromBody] Income income)
        {
            Account account = await _accountRepository.GetAccountAsync();
            int result = await _incomeRepository.SaveAsync(income);
            if (result > 0)
            {
                return Accepted();
            }
            else return BadRequest();
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


        [HttpGet, Route("DownloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (id > 0)
            {
                Income income = await _incomeRepository.GetByIdAsync(id);
                if (income != null && income.File != null)
                {
                    return File(income.File, "application/octet-stream", $"Income Report.{income.FileExtension}");
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
