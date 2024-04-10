using System.Net;

namespace OrrnrrWebApi.Exceptions
{
    public class ConflictApiException : ApiException
    {
        public ConflictApiException(string message) : base(HttpStatusCode.Conflict, message)
        {
        }
    }
}
