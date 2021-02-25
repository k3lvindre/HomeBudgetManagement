using System.Web.Http;
using HomeBudgetManagement.Models;
using HomeBudgetManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Linq;
using HomeBudgetManagement.API.Services;

namespace HomeBudgetManagement.API.Controllers
{
    [RoutePrefix("api/Income")]
    public class IncomeController : ApiController
    {
        private IIncomeRepository _incomeRepository { get; }
        private IAccountRepository _accountRepository { get; }

        public IncomeController(IIncomeRepository IncomeRepository, IAccountRepository accountRepository)
        {
            _incomeRepository = IncomeRepository;
            _accountRepository = accountRepository;
        }

        // GET: Incomes
        [HttpGet, Route("List")]
        public async Task<IHttpActionResult> Incomes()
        {
            try
            {
                using (_incomeRepository)
                {
                    List<Income> Incomes = await _incomeRepository.GetAllAsync();
                    if (Incomes.Count > 0)
                    {
                        return Ok(Incomes);
                    }
                    else return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            if (id > 0)
            {
                using (_incomeRepository)
                {
                    Income Income = await _incomeRepository.GetAsync(id);
                    if (Income != null)
                    {
                        return Ok(Income);
                    }
                    else return NotFound();
                }
            }
            else return BadRequest();
        }

        [HttpPost, Route("PostIncome")]
        public async Task<IHttpActionResult> AddIncome(Income Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    await _incomeRepository.CreateAsync(Income);
                    if (Income.Id > 0)
                    {
                        bool result = false;
                        Account account = await _accountRepository.GetAsync();
                        if(account != null)
                        {
                            account.Balance += Income.Amount;
                            result = await _accountRepository.SaveAsync(account);
                        }

                        if (result)
                        {
                            return Created<Income>("PostIncome", Income);
                        }
                        else
                        {
                            await _incomeRepository.DeleteAsync(Income);
                            return BadRequest();
                        }
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut, Route("UpdateIncome")]
        public async Task<IHttpActionResult> UpdateIncome(Income Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    Income origIncome = await _incomeRepository.GetAsync(Income.Id);

                    Account account = await _accountRepository.GetAsync();
                    account.Balance -= origIncome.Amount;

                    if(await _incomeRepository.UpdateAsync(Income) > 0)
                    {
                        account.Balance += Income.Amount;
                        if( await _accountRepository.SaveAsync(account)) return Ok();
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("DeleteRange")]
        public async Task<IHttpActionResult> DeleteIncome(List<Income> Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    int result = await _incomeRepository.DeleteRangeAsync(Income);
                    if (result > 0)
                    {
                        Account account = await _accountRepository.GetAsync();
                        account.Balance -= (from i in Income
                                            select i.Amount).CustomSum();
                        if (await _accountRepository.SaveAsync(account)) return Ok();
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("Delete/{id}")]
        public async Task<IHttpActionResult> DeleteIncome(int id)
        {
            try
            {
                using (_incomeRepository)
                {
                    Income Income = await _incomeRepository.GetAsync(id);
                    if(Income != null)
                    {
                        int result = await _incomeRepository.DeleteAsync(Income);
                        if (result > 0)
                        {
                            Account account = await _accountRepository.GetAsync();
                            account.Balance -= Income.Amount;
                            if (await _accountRepository.SaveAsync(account)) return Ok();
                        }
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("DownloadFile/{id}")]
        public async Task<IHttpActionResult> DownloadFile(int id)
        {
            if (id > 0)
            {
                using (_incomeRepository)
                {
                    Income income = await _incomeRepository.GetAsync(id);
                    if (income != null)
                    {
                        return Ok(income.File);
                    }
                    else return NotFound();
                }
            }
            else return BadRequest();
        }

    }


}