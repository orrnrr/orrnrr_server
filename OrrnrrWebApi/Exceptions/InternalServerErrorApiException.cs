using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class InternalServerErrorApiException(string message) : ApiException(HttpStatusCode.InternalServerError, message)
    {
    }
}
