using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using System.Security.Principal;

namespace HomeBudgetManagement.API
{
    //Created Custom basic authentication, I used AuthorizationFilterAttribute instead of AuthorizeAttribute or IAuthorizationFilter so i don't have to add owin
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeAuthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                // spliting decodeauthToken using ':'   
                var arrUserNameandPassword = decodeAuthToken.Split(':');

                // at 0th postion of array we get username and at 1st we get password  
                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(
                           new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public static bool IsAuthorizedUser(string Username, string Password)
        {
            // In this method we can handle our database logic here...  
            return Username == "kelvin" && Password == "password";
        }
    }
}