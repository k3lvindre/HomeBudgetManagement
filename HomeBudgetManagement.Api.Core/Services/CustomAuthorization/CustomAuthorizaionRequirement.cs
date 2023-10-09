using Microsoft.AspNetCore.Authorization;

namespace HomeBudgetManagement.Api.Core.Services.CustomAuthorization
{
    public class CustomAuthorizaionRequirement : IAuthorizationRequirement
    {
        //Constructor argument and field is not required unless you want especial 
        //custom implementation like this.
        //this is injected in startup.cs like this
        //services.AddAuthorization(options =>
        //    {
        //        options.AddPolicy("AtLeast21", policy =>
        //            policy.Requirements.Add(new CustomAuthorizaionRequirement(21)));
        //    });

        //this requirement should have a handler
        //see CustomAuthorizaionHandler class and this was injected like this
        //services.AddSingleton<IAuthorizationHandler, CustomAuthorizaionHandler>();
        public CustomAuthorizaionRequirement(string nickName)
            => NickName = nickName;

        public string NickName { get; }
    }
}
