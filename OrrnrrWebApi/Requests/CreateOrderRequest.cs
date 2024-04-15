using OrrnrrWebApi.Exceptions;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Requests
{
    public class CreateOrderRequest : IRequest
    {
        [JsonPropertyName("tokenId")]
        public int? TokenId { get; set; }
        [JsonPropertyName("orderType")]
        public string? OrderType { get; set; }
        [JsonPropertyName("isBuyOrder")]
        public bool? IsBuyOrder { get; set; }
        [JsonPropertyName("price")]
        public int? Price { get; set; }
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        public void ThrowIfNotValid()
        {
            if (TokenId is null)
            {
                throw new BadRequestApiException("주문할 토큰 아이디는 null일 수 없습니다.");
            }

            if (OrderType is null)
            {
                throw new BadRequestApiException("주문 유형을 지정하지 않았습니다.");
            }
        }
    }
}
