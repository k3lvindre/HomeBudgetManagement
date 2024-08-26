using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherController : ControllerBase
    {
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
