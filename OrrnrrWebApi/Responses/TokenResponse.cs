
using DataAccessLib.Models;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class TokenResponse
    {
        private TokenResponse(int id, string name, int tokenSourceId, string description)
        {
            Id = id;
            Name = name;
            TokenSourceId = tokenSourceId;
            Description = description;
        }

        [JsonPropertyName("id")]
        public int Id { get; }
        [JsonPropertyName("name")]
        public string Name { get; }
        [JsonPropertyName("tokenSourceId")]
        public int TokenSourceId { get; }
        public string Description { get; }

        internal static TokenResponse From(Token token)
        {
            return new TokenResponse(
                id: token.Id
                , name: token.Name
                , tokenSourceId: token.TokenSourceId
                , description: token.Description
            );
        }
    }
}
