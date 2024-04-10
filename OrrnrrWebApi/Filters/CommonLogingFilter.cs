using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace OrrnrrWebApi.Filters
{
    public class CommonLogingFilter : IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var connection = context.HttpContext.Connection;
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            var requestDateTime = DateTime.ParseExact(request.Headers["requestDateTime"]!, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None);

            var now = DateTime.Now;
            var excuteTime = now - requestDateTime;

            Console.WriteLine($"{now:yyyy-MM-dd HH:mm:ss.fff} {connection.RemoteIpAddress} {request.Method} {request.Path.Value} {request.Host} 응답 {response.StatusCode} {excuteTime.TotalMilliseconds:#,##0.0}ms");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var connection = context.HttpContext.Connection;
            var request = context.HttpContext.Request;
            var nowStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            request.Headers.Append("requestDateTime", nowStr);

            Console.WriteLine($"{nowStr} {connection.RemoteIpAddress} {request.Method} {request.Path.Value} 요청");
        }
    }
}
