using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

namespace HomeBudgetManagement.Api.Core.Services.CustomAuthorization
{
    public class CustomAuthorizaionHandler : AuthorizationHandler<CustomAuthorizaionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
            , CustomAuthorizaionRequirement requirement)
        {
            var nickName = context.User.FindFirst(
            c => c.Type == "NickName" && c.Issuer == "http://localhost:5000");

            if (nickName is null)
            {
                return Task.CompletedTask;
            }

            //if we dont return Succeed 
            if (nickName.Value == requirement.NickName)
            {
                context.Succeed(requirement);
            } 
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
