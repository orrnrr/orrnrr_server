using DataAccessLib.Models;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Repositories;

namespace OrrnrrWebApi.Services
{
    public class TransactionService : ITransactionService
    {
        public TransactionService(OrrnrrContext context)
        {
            OrrnrrContext = context;
        }

        private OrrnrrContext OrrnrrContext { get; }
        
        public Result<TransactionHistory> CreateTransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, int signedPrice)
        {
            var buyOrder = order1.IsBuyOrder ? order1 : order2;
            var sellOrder = order1.IsBuyOrder ? order2 : order1;

            var buyer = buyOrder.User;
            var seller = sellOrder.User;

            if (buyOrder.IsCanceled || sellOrder.IsCanceled)
            {
                throw new InvalidOperationException("거래를 성사시키지 못했습니다. 취소된 주문이 포함되어 있습니다.");
            }

            if (buyOrder.IsBuyOrder == sellOrder.IsBuyOrder)
            {
                throw new InvalidOperationException("거래를 성사시키지 못했습니다. 두 주문이 모두 매수주문이거나 매도주문입니다.");
            }
            
            var tradeCount = Math.Min(buyOrder.ExecutableCount, sellOrder.ExecutableCount);
            
            if (tradeCount <= 0)
            {
                throw new InvalidOperationException("거래를 성사시키지 못했습니다. 거래가능수량이 0 이하입니다.");
            }

            int payment = signedPrice * tradeCount;

            bool successToPay = buyer.TryPayTo(seller, payment);
            if (!successToPay)
            {
                return new Result<TransactionHistory>(ErrorCode.InsufficientBalance);
            }

#error 여기서부터 다시 작성
            OrrnrrContext.TokenHoldingsHistories.GetTokenHoldingHistory(buyer);

            Result<(int, TradeActionType)> signResult = order1.Sign(order2, signedPrice);
            if (signResult.IsError)
            {
                return new Result<TransactionHistory>(signResult.Error);
            }

            (int tradeCount, TradeActionType tradeActionType) = signResult.Value;

            TradeAction tradeAction = OrrnrrContext.TradeActions.GetTradeAction(tradeActionType) ?? throw new InvalidOperationException("trada_action이 존재하지 않습니다.");

            TransactionHistory createdTransactionHistory = new TransactionHistory(order1, order2, tradeAction, tradeCount, signedPrice);

            return new Result<TransactionHistory>(createdTransactionHistory);
        }
    }
}
