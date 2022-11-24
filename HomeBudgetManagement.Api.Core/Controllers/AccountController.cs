using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeBudgetManagement.Api.Core.Services;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/Accounts")]
    //With ApiController attribute.This attribute allows the data from the request to be mapped to the employee parameter on UpdateEmployee() method.
    //Either this ApiController attribute is required or the method parameter must be decorated with[FromBody] attribute.
    //Otherwise, model binding will not work as expected and the account data from the request will not be mapped to the account parameter below
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            Account account = await _accountRepository.GetAccountByIdAsync(id);
            if(account != null)
            {
                return Ok(account);
            } else return NotFound();
        }
        
        [HttpPut]
        public async Task<IActionResult> Update(Account account)
        {
            var result = await _accountRepository.UpdateAccountAsync(account);
            if(result)
            {
                return Ok();
            } else return NotFound();
        }


    }
}
