using System.Web.Http;
using HomeBudgetManagement.Models;
using HomeBudgetManagement.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeBudgetManagement.API.Controllers
{
    [RoutePrefix("api/Incomes")]
    public class IncomeController : ApiController
    {
        private IIncomeRepository _incomeRepository { get; }

        public IncomeController(IIncomeRepository IncomeRepository, IAccountRepository accountRepository)
        {
            _incomeRepository = IncomeRepository;
        }

        // GET: Incomes
        [HttpGet]
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

        [HttpGet]
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

        [HttpPost]
        public async Task<IHttpActionResult> AddIncome([FromBody] Income Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    await _incomeRepository.CreateAsync(Income);
                    if (Income.Id > 0)
                    {
                        return Created<Income>("PostIncome", Income);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IHttpActionResult> UpdateIncome([FromBody] Income Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    if(await _incomeRepository.UpdateAsync(Income) > 0)
                    {
                       return Ok();
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteIncome([FromBody] List<Income> Income)
        {
            try
            {
                using (_incomeRepository)
                {
                    if (await _incomeRepository.DeleteRangeAsync(Income) > 0)
                    {
                        return Ok();
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteIncome(int id)
        {
            try
            {
                using (_incomeRepository)
                {
                    Income Income = await _incomeRepository.GetAsync(id);
                    if(Income != null)
                    {
                        if (await _incomeRepository.DeleteAsync(Income) > 0)
                        {
                            return Ok();
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