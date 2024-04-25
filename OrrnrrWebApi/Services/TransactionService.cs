using DataAccessLib.Models;
using DataAccessLib.Types;
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
        
        public TransactionHistory CreateTransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, int signedPrice)
        {
            (int tradeCount, TradeActionType tradeActionType) = order1.Sign(order2, signedPrice);

            TradeAction tradeAction = OrrnrrContext.TradeActions.GetTradeAction(tradeActionType) ?? throw new InvalidOperationException("trada_action이 존재하지 않습니다.");

            return new TransactionHistory(order1, order2, tradeAction, tradeCount, signedPrice);
        }
    }
}
