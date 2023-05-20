using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;
using System.Net.Http;
using System.IO;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/Expenses")]
    [ApiController]
    //Add the[Produces("application/json")] attribute to the API controller.
    //Its purpose is to declare that the controller's actions support a response content type of application/json:
    [Produces("application/json")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository expenseRepository,
                                 IAccountRepository accountRepository)
        {
            _expenseRepository = expenseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Expense> expenses = await _expenseRepository.GetAllAsync();
            if (expenses.Any())
            {
                return Ok(expenses);
            }
            else return NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBydId(int id)
        {
            Expense expense =  await _expenseRepository.GetByIdAsync(id);
            if(expense == null)
            {
               return  NotFound();
            }
            else  return Ok(expense);
        }

        //Adding triple-slash comments to an action enhances the Swagger UI by adding the description to the section header.
        //Add a<remarks> element to the Create action method documentation.
        //It supplements information specified in the<summary> element and provides a more robust Swagger UI.
        //The<remarks> element content can consist of text, JSON, or XML.
        
        /// <summary>
        /// Creates a expense.
        /// </summary>
        /// <param name="expense"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddExpense(Expense expense)
        {
            //Account account = await _accountRepository.GetAccountAsync();
            //if(account.Balance >= expense.Amount)
            //{
                expense = await _expenseRepository.AddAsync(expense);
                if (expense.Id > 0)
                {
                    return CreatedAtAction(nameof(Get), new { id = expense.Id }, expense);
                }
                else return BadRequest();
            //}
            //else return BadRequest("Insuficient Balance!");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense(Expense expense)
        {
            //Account account = await _accountRepository.GetAccountAsync();
            //if (account.Balance >= expense.Amount)
            //{
                var result = await _expenseRepository.SaveAsync(expense);
                if (result)
                {
                    return Ok();
                }
                else return BadRequest();
            //} 
            //else return BadRequest("Insuficient Balance!");
        }

        /// <summary>
        /// Deletes a specific Expense.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _expenseRepository.RemoveAsync(id);

            if (result)
            {
                return Ok();
            }
            else return BadRequest();
        }


        [HttpGet, Route("{id}/file")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            if (id > 0)
            {
                Expense expense = await _expenseRepository.GetByIdAsync(id);
                if (expense != null && expense.File != null)
                {
                    var dataStream = new MemoryStream(expense.File);
                    return File(expense.File, "application/octet-stream",$"Expense Report.{expense.FileExtension}");
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
