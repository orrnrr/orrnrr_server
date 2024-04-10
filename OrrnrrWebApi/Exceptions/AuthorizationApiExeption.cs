using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class AuthorizationApiExeption : ApiException
    {
        public AuthorizationApiExeption(HttpStatusCode statusCode, string message, string? code = null) : base(statusCode, message, code)
        {
        }
    }
}
