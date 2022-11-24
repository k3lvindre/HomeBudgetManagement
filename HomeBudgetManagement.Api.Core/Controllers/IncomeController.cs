using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/incomes")]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Income> incomes = await _incomeRepository.GetAllAsync();
            if (incomes.Any())
            {
                return Ok(incomes);
            }
            else return NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBydId(int id)
        {
            Income income = await _incomeRepository.GetByIdAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            else return Ok(income);
        }

        [HttpPost]
        public async Task<IActionResult> AddIncome(Income income)
        {
            //Account account = await _accountRepository.GetAccountAsync();
            income = await _incomeRepository.AddAsync(income);
            if (income.Id > 0)
            {
                return Accepted();
            }
            else return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIncome(Income income)
        {
            //Account account = await _accountRepository.GetAccountAsync();
            var result = await _incomeRepository.SaveAsync(income);
            if (result)
            {
                return Accepted();
            }
            else return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _incomeRepository.RemoveAsync(id);

            if (result)
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
