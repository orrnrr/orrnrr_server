using System.Net;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiFailureResult
    {
        [JsonPropertyName("content")]
        public object? Content { get; init; }
     
        [JsonPropertyName("message")]
        public required string Message { get; init; }
    }
}
