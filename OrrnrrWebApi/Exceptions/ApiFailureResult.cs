using System.Net;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Exceptions
{
    public class ApiFailureResult
    {
        [JsonPropertyName("code")]
        public string? Code { get; init; }
     
        [JsonPropertyName("message")]
        public required string Message { get; init; }
    }
}
