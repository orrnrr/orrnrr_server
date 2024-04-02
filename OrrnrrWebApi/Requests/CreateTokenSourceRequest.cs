using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Requests
{
    public class CreateTokenSourceRequest : ITokenSourceRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; init; }
        [JsonPropertyName("requestUrl")]
        public required string RequestUrl { get; init; }
    }
}
