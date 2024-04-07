using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class BadRequestApiException : ApiException
    {
        public BadRequestApiException(string message) : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
