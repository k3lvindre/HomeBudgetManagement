using HomeBudgetManagement.Application.Commands;
using HomeBudgetManagement.Application.Query;
using HomeBudgetManagement.DTO;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //Add the[Produces("application/json")] attribute to the API controller.
    //Its purpose is to declare that the controller's actions support a response content type of application/json:
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MealController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetMealsQuery());
            return result is not null && result.Any() ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetMealsQuery());
            var item = result?.FirstOrDefault(x => x.Id == id);
            return item is not null ? Ok(item) : NotFound();
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
        public async Task<IActionResult> Create(CreateMealRequestDto expense)
        {
            var command = new CreateMealCommand()
            {
                Name = expense.Name,
                Description = expense.Description,
                Price = expense.Price,
                File = expense.File,
                FileExtension = expense.FileExtension,
            };

            var result = await _mediator.Send(command);
            return result.IsCreated ? CreatedAtAction(nameof(Create), result.Id) : BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateMealRequestDto requestDto)
        {
            var command = new UpdateMealCommand()
            {
                Id = requestDto.Id,
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
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
            var result = await _mediator.Send(new DeleteMealCommand()
            {
                Id = id
            });

            return result ? Ok(result) : BadRequest();
        }
    }
}
