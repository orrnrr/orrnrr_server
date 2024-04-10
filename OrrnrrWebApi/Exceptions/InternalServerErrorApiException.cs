using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class InternalServerErrorApiException : ApiException
    {
        public InternalServerErrorApiException(string message) : base(HttpStatusCode.InternalServerError, message)
        {
        }
    }
}
