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
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("GetAccount")]
        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            Account account = await _accountRepository.GetAccountAsync();
            if(account != null)
            {
                return Ok(account);
            } else return NotFound();
        }
        
        [Route("UpdateAccount")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]Account account)
        {
            int result = await _accountRepository.UpdateAccountAsync(account);
            if(result > 0)
            {
                return Ok();
            } else return NotFound();
        }


    }
}
