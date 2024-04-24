using DataAccessLib.Models;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Responses
{
    public class TokenOrderHistoryResponse
    {
        private TokenOrderHistoryResponse(TokenOrderHistory order)
            : this(order.Id, order.OrderPrice, order.CompleteCount, order.OrderDateTime)
        {
        }

        private TokenOrderHistoryResponse(int id, int? orderPrice, int completeCount, DateTime orderDateTime)
        {
            Id = id;
            OrderPrice = orderPrice;
            CompleteCount = completeCount;
            OrderDateTime = orderDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static TokenOrderHistoryResponse From(TokenOrderHistory order) {
            return new TokenOrderHistoryResponse(order);
        }

        [JsonPropertyName("id")]
        public int Id { get; }
        [JsonPropertyName("orderPrice")]
        public int? OrderPrice { get; }
        [JsonPropertyName("completeCount")]
        public int CompleteCount { get; }
        [JsonPropertyName("orderDate")]
        public string OrderDateTime { get; }
    }
}
