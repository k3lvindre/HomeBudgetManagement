using System.Threading.Tasks;
using System.Web.Http;
using HomeBudgetManagement.Domain;
using HomeBudgetManagement.Models;

namespace HomeBudgetManagement.API.Controllers
{
    [RoutePrefix("api/Accounts")]
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
        [HttpGet]
        public async Task<IHttpActionResult> GetAccount()
        {
            using(_accountRepository)
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
}