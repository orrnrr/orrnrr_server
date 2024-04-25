using DataAccessLib.Models;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Types;
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

            if (!Enum.TryParse(OrderType, true, out OrderCategory orderType))
            {
                throw new BadRequestApiException("존재하지 않는 주문 유형입니다.");
            }

            if (IsBuyOrder is null)
            {
                throw new BadRequestApiException("매수주문여부는 null일 수 없습니다.");
            }

            if (Count is null)
            {
                throw new BadRequestApiException("주문 개수는 null일 수 없습니다.");
            }

            if (Count <= 0)
            {
                throw new BadRequestApiException("주문 개수는 0이하일 수 없습니다.");
            }

            if (orderType != OrderCategory.Market)
            {
                if (Price is null)
                {
                    throw new BadRequestApiException("시장가 주문이 아닌 경우 주문가격은 null일 수 없습니다.");
                }

                if (Price <= 0)
                {
                    throw new BadRequestApiException("주문 가격은 0이하일 수 없습니다.");
                }
            }
        }
    }
}
