using DataAccessLib.Models;

namespace OrrnrrWebApi.Services
{
    public interface ITokenHoldingHistoryService
    {
        /// <summary>
        /// 토큰을 거래합니다.
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <param name="token"></param>
        /// <param name="signedPrice"></param>
        /// <param name="tradeCount"></param>
        void TradeToken(User user1, User user2, Token token, int signedPrice, int tradeCount);
    }
}
