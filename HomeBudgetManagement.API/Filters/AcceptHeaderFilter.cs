using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HomeBudgetManagement.API
{
    //Custom filter attribute to check wether request has the x-api-key accept header and see if it matches the hard coded value here
    public class AcceptHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                Debug.WriteLine("AcceptHeaderFilter  invoked");
                string apiKey = actionContext.Request.Headers.GetValues("api-key")?.FirstOrDefault();
                if (apiKey != "12345")
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unauthorized");
            }
        }
    }
}