using HomeBudgetManagement.Application.Commands;
using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/v1/[thiswillreplacebycontrollername]")]
    [ApiController]
    //Add the[Produces("application/json")] attribute to the API controller.
    //Its purpose is to declare that the controller's actions support a response content type of application/json:
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BudgetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    List<Expense> expenses =  await _unitOfWork.Expenses.GetAllAsync();
        //    if (expenses.Any())
        //    {
        //        return Ok(expenses);
        //    }
        //    else return NotFound();
        //}

        //[HttpGet]
        //[Authorize(Policy = "NickNamePolicy")]
        //public async Task<IActionResult> GetExpenses()
        //{
        //    var query = new GetExpenseQuery()
        //    {
        //        ExpenseIds = request.ExpenseIds
        //    };

        //    var result = await _mediator.Send(query);

        //    if (result.Any())
        //    {
        //        return Ok(result);
        //    }
        //    else return NotFound();
        //}

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
        public async Task<IActionResult> AddExpense(CreateBudgetRequestDto expense)
        {
            var command = new CreateBudgetCommand()
            {
                Description = expense.Description,
                Amount = expense.Amount,
                ItemType = (ItemType)expense.Type,
                File = expense.File,
                FileExtension = expense.FileExtension,
            };

            var result = await _mediator.Send(command);
            if (result.IsCreated)
            {
                return CreatedAtAction(nameof(AddExpense), result.Id);
            }
            else return BadRequest();
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateExpense(CreatePayoutRequestDto expense)
        //{
        //    //Account account =  _accountRepository.GetAccountAsync();
        //    //if (account.Balance >= expense.Amount)
        //    //{
        //    //_unitOfWork.Expenses.Update(expense);
        //    //var result = await _unitOfWork.SaveChangesAsync();
        //    var command = new CreateExpenseCommand(expense.De)

        //    var result = _mediator.Send();
        //        if (result > 0)
        //        {
        //            return Ok();
        //        }
        //        else return BadRequest();
        //    //} 
        //    //else return BadRequest("Insuficient Balance!");
        //}

        /// <summary>
        /// Deletes a specific Expense.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var item = await _unitOfWork.Expenses.GetByIdAsync(id);
        //    _unitOfWork.Expenses.Delete(item);
        //    var result =  await _unitOfWork.SaveChangesAsync();

        //    if (result > 0)
        //    {
        //        return Ok();
        //    }
        //    else return BadRequest();
        //}


        //[HttpGet, Route("{id}/file")]
        //public async Task<IActionResult> DownloadFile(int id)
        //{
        //    if (id > 0)
        //    {
        //        Expense expense =  await _unitOfWork.Expenses.GetByIdAsync(id);
        //        if (expense != null && expense.File != null)
        //        {
        //            var dataStream = new MemoryStream(expense.File);
        //            return File(expense.File, "application/octet-stream",$"Expense Report.{expense.FileExtension}");
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    return BadRequest();
        //}
    }
}
