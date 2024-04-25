using DataAccessLib.Models;
using OrrnrrWebApi.Repositories;

namespace OrrnrrWebApi.Services
{
    public class TokenHoldingHistoryService : ITokenHoldingHistoryService
    {
        public TokenHoldingHistoryService(OrrnrrContext orrnrrContext) {
            OrrnrrContext = orrnrrContext;
        }

        private OrrnrrContext OrrnrrContext { get; }

        public void TradeToken(User buyer, User seller, Token token, int signedPrice, int tradeCount)
        {
            var newBuyerTokenHoldingHistory = OrrnrrContext.TokenHoldingsHistories.CopyLatestTokenHoldingHistory(buyer, token);
            var newSellerTokenHoldingHistory = OrrnrrContext.TokenHoldingsHistories.CopyLatestTokenHoldingHistory(seller, token);

            newSellerTokenHoldingHistory.SendToken(newBuyerTokenHoldingHistory, signedPrice, tradeCount);

            OrrnrrContext.TokenHoldingsHistories.Add(newBuyerTokenHoldingHistory);
            OrrnrrContext.TokenHoldingsHistories.Add(newSellerTokenHoldingHistory);
        }
    }
}
