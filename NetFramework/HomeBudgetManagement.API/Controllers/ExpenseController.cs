﻿using HomeBudgetManagement.Domain;
using HomeBudgetManagement.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace HomeBudgetManagement.API.Controllers
{
    [RoutePrefix("api/Expenses")]
    public class ExpenseController : ApiController
    {
        private IExpenseRepository _expenseRepository { get; }

        public ExpenseController(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        // GET: Expenses
        [HttpGet]
        public async Task<IHttpActionResult> Expenses()
        {
            try
            {
                using (_expenseRepository)
                {
                    List<Expense> expenses = await _expenseRepository.GetAllAsync();
                    if (expenses.Count > 0)
                    {
                        return Ok(expenses);
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
                using (_expenseRepository)
                {
                    Expense expense = await _expenseRepository.GetAsync(id);
                    if (expense != null)
                    {
                        return Ok(expense);
                    }
                    else return NotFound();
                }
            }
            else return BadRequest();
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddExpense([FromBody] Expense expense)
        {
            try
            {
                using (_expenseRepository)
                {
                    await _expenseRepository.CreateAsync(expense);
                    if (expense.Id > 0)
                    {
                        //used createdatroute to exposed url at the header
                        return CreatedAtRoute("GetExpenseById",new { id = expense.Id }, expense);
                    }
                }

                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        public async Task<IHttpActionResult> UpdateExpense([FromBody] Expense expense)
        {
            try
            {
                using (_expenseRepository)
                {
                    if (await _expenseRepository.UpdateAsync(expense) > 0)
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
        public async Task<IHttpActionResult> DeleteExpense([FromBody] List<Expense> expense)
        {
            try
            {
                using (_expenseRepository)
                {
                    if (await _expenseRepository.DeleteRangeAsync(expense) > 0)
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
        public async Task<IHttpActionResult> DeleteExpense(int id)
        {
            try
            {
                using (_expenseRepository)
                {
                    Expense expense = await _expenseRepository.GetAsync(id);
                    if(expense != null)
                    {
                        if (await _expenseRepository.DeleteAsync(expense) > 0)
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


        //download file using httpresponsemessage see incomecontroller for other approach
        [HttpGet, Route("DownloadFile/{id}")]
        public async Task<HttpResponseMessage> DownloadFile(int id)
        {
            HttpResponseMessage httpResponseMessage;
            if (id > 0)
            {
                using (_expenseRepository)
                {
                    Expense expense = await _expenseRepository.GetAsync(id);
                    if (expense != null)
                    {
                        var dataStream = new MemoryStream(expense.File);

                        httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                        httpResponseMessage.Content = new StreamContent(dataStream);
                        httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        httpResponseMessage.Content.Headers.ContentDisposition.FileName = expense.FileExtension;
                        httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                        return httpResponseMessage;
                    }
                    else
                    {
                        httpResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
                        return httpResponseMessage;
                    }
                }
            }
            else
            {
                httpResponseMessage = Request.CreateResponse(HttpStatusCode.BadRequest);
                return httpResponseMessage;
            }
        }

        [HttpGet,Route("")]
        public IHttpActionResult TestCustomIHttpActionResult()
        {
            return new TextResult("test json",Request);
        }

    }

    public class TextResult : IHttpActionResult
    {
        string _value;
        HttpRequestMessage _request;

        public TextResult(string value, HttpRequestMessage request)
        {
            _value = value;
            _request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(_value),
                RequestMessage = _request
            };

            return Task.FromResult(response);
        }
    }
}