using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
        public ApiFailureResult Result { get => new ApiFailureResult { Message = Message }; }
    }
}
