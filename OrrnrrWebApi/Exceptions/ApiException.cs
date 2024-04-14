using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiException(HttpStatusCode statusCode, string message, object? content = null) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
        public object? Content { get; } = content;
    }
}
