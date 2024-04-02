using System.Net;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiFailureResult
    {
        [JsonPropertyName("message")]
        public required string Message { get; init; }
    }
}
