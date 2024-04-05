using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Requests
{
    public class TokenSourceRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}
