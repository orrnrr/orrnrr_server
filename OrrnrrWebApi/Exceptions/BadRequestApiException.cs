using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class BadRequestApiException(string message) : ApiException(HttpStatusCode.BadRequest, message)
    {
    }
}
