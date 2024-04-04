using DataAccessLib.Models;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class TokenSourceResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }
        [JsonPropertyName("name")]
        public required string Name { get; init; }

        internal static TokenSourceResponse From(TokenSource tokenSource)
        {
            return new TokenSourceResponse { 
                Name = tokenSource.Name,
                Id = tokenSource.Id,
            };
        }
    }
}
