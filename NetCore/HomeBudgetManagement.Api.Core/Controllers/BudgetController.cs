using HomeBudgetManagement.Application.Commands;
using HomeBudgetManagement.Application.Query;
using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
//test github actionsssss

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //Add the[Produces("application/json")] attribute to the API controller.
    //Its purpose is to declare that the controller's actions support a response content type of application/json:
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BudgetController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetExpenseQuery());
            return result is not null && result.Any() ? Ok(result) : NotFound();
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(GetBudgetQueryRequestDto getExpenseQueryRequestDto)
        {
            var result = await _mediator.Send(new GetExpenseQuery()
            {
                ListOfId = getExpenseQueryRequestDto?.ListOfId,
                Type = (ItemType?)getExpenseQueryRequestDto?.Type
            });

            return result is not null && result.Any() ? Ok(result) : NotFound();
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetExpenseQuery()
            {
                ListOfId = [id]
            });

            return result is not null && result.Any() ? Ok(result.First()) : NotFound();
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
        //ProducesResponseType - This attribute is used by tools like Swagger/OpenAPI to generate accurate API documentation
        //The first parameter is the type of the expected response(e.g., the return type of the action).
        //The optional second parameter is the HTTP status code associated with the response.
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //other example is:
        //[ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create(CreateBudgetRequestDto expense)
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
            return result.IsCreated ? CreatedAtAction(nameof(Create), result.Id) : BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateBudgetRequestDto requestDto)
        {
            var command = new UpdateBudgetCommand()
            {
                Id = requestDto.Id,
                Description = requestDto.Description,
                Amount = requestDto.Amount,
                ItemType = (ItemType)requestDto.Type
            };

            var result = await _mediator.Send(command);
            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Deletes a specific budget using Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>boolean</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBudgetCommand()
            {
                Id = id
            });

            return result ? Ok(result) : BadRequest();
        }

        #region Other useful example
        //1. Using authorization policy
        //2. Returning file

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
        #endregion
    }
}
