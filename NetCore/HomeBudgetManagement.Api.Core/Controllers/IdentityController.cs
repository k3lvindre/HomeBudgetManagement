using HomeBudgetManagement.Api.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [ApiController]
    [Route("api/v1/identity")]
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IAuthService _authService;

        public IdentityController(UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signinManager
            , IAuthService authService)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserAccount user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new IdentityUser(user.UserName), user.Password);
                if(result.Succeeded)
                {
                    return Accepted();
                }
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.FindByNameAsync(username);
                if(result is not null)
                {
                    return Ok(result.UserName);
                }
            }
            return NotFound();
        }
          
        [HttpPost("signin")]
        public async Task<IActionResult> Sigin(UserAccount account)
        { 
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(account.UserName);

                if (user is null) return NotFound();

                var result = await _signinManager.PasswordSignInAsync(user,  account.Password, false, false);
                if(result.Succeeded)
                {
                    return Ok(_authService.GenerateToken(user));
                }
            }
            return NotFound();
        }

        public class UserAccount
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
