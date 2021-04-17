using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HomeBudgetManagement.Domain;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.API.Controllers
{
    [RoutePrefix("api/Account")]
    [AcceptHeaderFilter]
    [BasicAuthentication]
    public class AccountController : ApiController
    {
        IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: api/Account/5
        [HttpGet, Route("Get")]
        public async Task<IHttpActionResult> GetAccount()
        {
            Account account = await _accountRepository.GetAsync();
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }
    }
}