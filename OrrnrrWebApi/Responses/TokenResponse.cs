
using DataAccessLib.Models;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class TokenResponse
    {
        public TokenResponse(int id, string name, string description, int tokenSourceId, string tokenSourceName, int currentPrice, int previousPrice, int tradeAmount)
        {
            Id = id;
            Name = name;
            Description = description;
            TokenSourceId = tokenSourceId;
            TokenSourceName = tokenSourceName;
            CurrentPrice = currentPrice;
            PreviousPrice = previousPrice;
            TradeAmount = tradeAmount;
        }

        public TokenResponse() { }

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
        [JsonPropertyName("currentPrice")]
        public int CurrentPrice { get; init; }
        [JsonPropertyName("previousPrice")]
        public int PreviousPrice { get; init; }
        [JsonPropertyName("tradeAmount")]
        public int TradeAmount { get; init; }
    }
}
