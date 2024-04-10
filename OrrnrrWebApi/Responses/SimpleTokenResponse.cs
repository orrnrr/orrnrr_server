using DataAccessLib.Models;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class SimpleTokenResponse
    {
        [JsonPropertyName("id")]
        public required int Id { get; init; }
        [JsonPropertyName("name")]
        public required string Name { get; init; }
        [JsonPropertyName("description")]
        public required string Description { get; init; }
        [JsonPropertyName("tokenSourceId")]
        public required int TokenSourceId { get; init; }
        [JsonPropertyName("tokenSourceName")]
        public required string TokenSourceName { get; init; }

        internal static SimpleTokenResponse From(Token token)
        {
            return new SimpleTokenResponse { 
                Id = token.Id,
                Name = token.Name,
                Description = token.Description,
                TokenSourceId = token.TokenSourceId,
                TokenSourceName = token.TokenSource.Name,
            };
        }
    }
}
