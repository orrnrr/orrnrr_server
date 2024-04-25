using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Repositories;
using System.Data;

namespace OrrnrrWebApi.Services
{
    public class TransactionService : ITransactionService
    {
        public TransactionService(OrrnrrContext context, ITokenHoldingHistoryService tokenHoldingHistoryService)
        {
            OrrnrrContext = context;
            TokenHoldingHistoryService = tokenHoldingHistoryService;
        }

        private OrrnrrContext OrrnrrContext { get; }
        private ITokenHoldingHistoryService TokenHoldingHistoryService { get; }


        public Result<TransactionHistory> CreateTransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, int signedPrice)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(IsolationLevel.Serializable);

            var buyOrder = order1.IsBuyOrder ? order1 : order2;
            var sellOrder = order1.IsBuyOrder ? order2 : order1;

            if (buyOrder.IsCanceled || sellOrder.IsCanceled)
            {
                throw new InvalidOperationException("거래를 성사시키지 못했습니다. 취소된 주문이 포함되어 있습니다.");
            }

            if (buyOrder.Token.Id != sellOrder.Token.Id)
            {
                throw new InvalidOperationException("거래를 성사시키지 못했습니다. 서로 다른 토큰에 대한 주문입니다.");
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

            bool successToPay = buyOrder.User.TryPayTo(sellOrder.User, payment);
            if (!successToPay)
            {
                return new Result<TransactionHistory>(ErrorCode.HaveNotEnoughBalance);
            }

            TokenHoldingHistoryService.TradeToken(buyOrder.User, sellOrder.User, buyOrder.Token, signedPrice, tradeCount);


            TradeAction tradeAction = OrrnrrContext.TradeActions.GetTradeAction(tradeActionType) ?? throw new InvalidOperationException("trada_action이 존재하지 않습니다.");

            TransactionHistory createdTransactionHistory = new TransactionHistory(order1, order2, tradeAction, tradeCount, signedPrice);

            return new Result<TransactionHistory>(createdTransactionHistory);
        }
    }
}
