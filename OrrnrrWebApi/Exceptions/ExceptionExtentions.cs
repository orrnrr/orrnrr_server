using Microsoft.AspNetCore.Http.HttpResults;

namespace OrrnrrWebApi.Exceptions
{
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
