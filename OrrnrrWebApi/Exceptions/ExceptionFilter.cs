using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ApiException apiException)
            {
                context.HttpContext.Response.StatusCode = (int)apiException.StatusCode;
                context.Result = new ObjectResult(apiException.Result);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is Exception exception)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new ObjectResult(exception.Message);
                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
