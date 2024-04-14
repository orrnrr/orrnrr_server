using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class AuthorizationApiExeption(HttpStatusCode statusCode, string message, string? code = null) : ApiException(statusCode, message, code)
    {
    }
}
