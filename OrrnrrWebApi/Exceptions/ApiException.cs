using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiException(HttpStatusCode statusCode, string message, object? content = null) : Exception(message)
    {
        public ApiException(HttpStatusCode statusCode, ErrorCode errorCode)
            : this(statusCode, errorCode.GetMessage(), errorCode.GetCode())
        {
        }

        public HttpStatusCode StatusCode { get; } = statusCode;
        public object? Content { get; } = content;
    }

    public class UnauthorizedApiExeption(string message, string? code = null)
        : ApiException(HttpStatusCode.Unauthorized, message, code)
    {
    }

    public class NonPositiveNumberApiException(string paramName, object? content = null) 
        : ApiException(HttpStatusCode.BadRequest, $"{paramName}의 값은 0이하일 수 없습니다.", content)
    {
    }

    public class InternalServerErrorApiException(string message)
        : ApiException(HttpStatusCode.InternalServerError, message)
    {
    }

    public class ConflictApiException(string message, object? content = null)
        : ApiException(HttpStatusCode.Conflict, message, content)
    {
    }

    public class BadRequestApiException(string message, object? content = null)
        : ApiException(HttpStatusCode.BadRequest, message, content)
    {
    }

    public class NullParameterApiException(string paramName, object? content = null)
        : ApiException(HttpStatusCode.BadRequest, $"{paramName}의 값은 null일 수 없습니다.", content)
    { 
    }

    public class NotFoundApiException(string message, object? content = null)
        : ApiException(HttpStatusCode.NotFound, message, content)
    {
        public NotFoundApiException(ErrorCode errorCode)
            : this(errorCode.GetMessage(), errorCode.GetCode())
        {
        }
    }


    internal static class ExceptionExtentions
    {
        internal static ApiFailureResult GetApiFailureResult<T>(this T exception) where T : ApiException
        {
            return new ApiFailureResult { Content = exception.Content, Message = exception.Message };
        }

        internal static ApiFailureResult GetApiFailureResult(this Exception exception)
        {
            return new ApiFailureResult { Message = exception.Message };
        }
    }
}
