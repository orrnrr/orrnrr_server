using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class TokenSourceResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }
        [JsonPropertyName("name")]
        public required string Name { get; init; }
    }
}
