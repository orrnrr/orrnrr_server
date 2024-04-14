using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ConflictApiException(string message) : ApiException(HttpStatusCode.Conflict, message)
    {
    }
}
