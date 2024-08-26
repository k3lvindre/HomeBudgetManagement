using HomeBudgetManagement.Application.Commands;
using HomeBudgetManagement.Application.Query;
using HomeBudgetManagement.DTO;
using HomeBudgetManagement.SharedKernel.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoFile = System.IO.File;

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
        static List<WebSocket> _webSocketConnections = new List<WebSocket>();

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
                Type = (ItemType?)getExpenseQueryRequestDto?.Type,
                DateFrom = getExpenseQueryRequestDto.DateFrom,
                DateTo = getExpenseQueryRequestDto.DateTo
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

        //Web Sockets Example
        [Route("/ws")]
        [HttpPost]
        public async Task WebSocketAction()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketConnections.Add(webSocket);
               
                while(webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;

                    var message2 = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    message2 = $"Server received:{message2}";
                    var bytes2 = Encoding.UTF8.GetBytes(message2);
                    
                    var arraySegment2 = new ArraySegment<byte>(bytes2, 0, bytes2.Length);
                    foreach (var item in _webSocketConnections)
                    {
                        if(item.State == WebSocketState.Open)
                            await item.SendAsync(arraySegment2,
                            WebSocketMessageType.Text,
                            true,
                              CancellationToken.None);
                    }


                    //If you set the `Offset` property of an `ArraySegment` to 10, it means that the segment will start from the element at index 10 of the original array.
                    //Let's illustrate this with an example in C#:
                    //int[] originalArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    //
                    //Create an ArraySegment starting from index 10 with a count of 5 elements
                    //ArraySegment<int> segment = new ArraySegment<int>(originalArray, 10, 5);
                    //
                    //// Access elements of the segment
                    //foreach (int element in segment)
                    //{
                    //    Console.WriteLine(element);
                    //}
                    //
                    //In this example, the `ArraySegment` represents a segment starting from index 10 of the `originalArray` with a count of 5 elements.Therefore, the output of the `foreach` loop will be:
                    //
                    //10
                    //11
                    //12
                    //13
                    //14
                    //
                    //Setting the `Offset` to 10 effectively means that the segment starts 10 elements into the original array.
                    //This allows you to work with a subsection of the array without having to modify or copy the original data.
                    //It's useful when you only need to operate on a portion of the array for certain operations.

                    //So here we want to start at index of 0 because we need the whole file
                    var bytes3 = IoFile.ReadAllBytes(@"C:\Users\KRicafort\Documents\sample.jpg");
                    var arraySegment3 = new ArraySegment<byte>(bytes3, 0, bytes3.Length);
                    foreach (var item in _webSocketConnections)
                    {
                        if (item.State == WebSocketState.Open)
                            await item.SendAsync(arraySegment3,
                                WebSocketMessageType.Binary,
                                true,
                                CancellationToken.None);
                    }
                }

                _webSocketConnections.Remove(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
