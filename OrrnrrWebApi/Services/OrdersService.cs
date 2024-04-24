using DataAccessLib.Models;
using DataAccessLib.Types;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Repositories;
using OrrnrrWebApi.Sort;
using OrrnrrWebApi.Types;
using System.Text.Json;
using System.Transactions;

namespace OrrnrrWebApi.Services
{
    internal class OrdersService : IOrdersService
    {
        private OrrnrrContext OrrnrrContext { get; }
        public OrdersService(OrrnrrContext context)
        {
            OrrnrrContext = context;
        }

        public TokenOrderHistory CreateLimitOrder(int userId, int tokenId, bool isBuyOrder, int price, int count)
        {
            return isBuyOrder switch
            {
                true => CreateLimitBuyOrder(userId, tokenId, price, count),
                false => CreateLimitSellOrder(userId, tokenId, price, count),
            };
        }

        private TokenOrderHistory CreateLimitSellOrder(int userId, int tokenId, int price, int count)
        {
            var token = OrrnrrContext.Tokens
                .Where(x => x.Id == tokenId)
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 토큰은 주문할 수 없습니다.");

            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            
            var user = OrrnrrContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 유저입니다.");

            int numberOwned = OrrnrrContext.TokenHoldingsHistories
                .Where(x => x.UserId == userId)
                .Where(x => x.TokenId == tokenId)
                .Select(x => x.TokenCount)
                .FirstOrDefault();

            var numberSell = OrrnrrContext.TokenOrderHistories
                .Where(x => x.UserId == userId)
                .Where(x => x.TokenId == tokenId)
                .Where(x => !x.IsBuyOrder)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => !x.IsCanceled)
                .Sum(x => x.OrderCount - x.CompleteCount);

            if (numberOwned < numberSell + count)
            {
                throw new BadRequestApiException("소유한 토큰 개수가 충분하지 않습니다.");
            }

            var newOrder = TokenOrderHistory.CreateSellOrder(user, token, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories.GetCanSellOrdersForLimit(token, price).ToArray();
            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = existsOrder.OrderPrice!.Value;

                (int transactionCount, TradeActionType tradeActionType) = newOrder.Sign(existsOrder, signedPrice);

                var tradeAction = OrrnrrContext.TradeActions
                    .Where(x => x.Name == tradeActionType.GetActionName())
                    .First();

                var newTrade = new TransactionHistory(existsOrder, newOrder, tradeAction, transactionCount, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTrade);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        private TokenOrderHistory CreateLimitBuyOrder(int userId, int tokenId, int price, int count)
        {
            var token = OrrnrrContext.Tokens
                .Where(x => x.Id == tokenId)
                .FirstOrDefault() ?? throw new NotFoundApiException("토큰 정보가 존재하지 않습니다.");

            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            var user = OrrnrrContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault() ?? throw new UnauthorizedApiExeption("유저 정보가 존재하지 않습니다.");

            int existsBuyPayment = OrrnrrContext.TokenOrderHistories
                .Where(x => x.User == user)
                .Where(x => x.Token == token)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsBuyOrder)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => x.OrderPrice.HasValue)
                .Sum(x => x.OrderPrice!.Value * (x.OrderCount - x.CompleteCount));

            if (!user.CanPay(existsBuyPayment + price * count))
            {
                throw new BadRequestApiException("잔고가 충분하지 않습니다.");
            }

            var newOrder = TokenOrderHistory.CreateBuyOrder(user, token, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories.GetCanBuyOrdersForLimit(token, price).ToArray();

            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = existsOrder.OrderPrice!.Value;
                (int transactionCount, TradeActionType tradeActionType) = newOrder.Sign(existsOrder, signedPrice);

                var tradeAction = OrrnrrContext.TradeActions
                    .Where(x => x.Name == tradeActionType.GetActionName())
                    .First();

                var newTrade = new TransactionHistory(newOrder, existsOrder, tradeAction, transactionCount, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTrade);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        public TokenOrderHistory CreateMarketOrder(int userId, int tokenId, bool isBuyOrder, int count)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            var user = OrrnrrContext.Users
                .Where(user => user.Id == userId)
                .FirstOrDefault() ?? throw new UnauthorizedApiExeption("유저 정보가 존재하지 않습니다.");

            var token = OrrnrrContext.Tokens
                .Where(token => token.Id == tokenId)
                .FirstOrDefault() ?? throw new NotFoundApiException("토큰 정보가 존재하지 않습니다.");

            var newOrder = new TokenOrderHistory(user, token, isBuyOrder, count);

            var matchedOrders = OrrnrrContext.TokenOrderHistories
                .GetMatchingOrders(newOrder)
                .ToArray();

            foreach(var matchedOrder in matchedOrders)
            {
                int signedPrice = matchedOrder.OrderPrice!.Value;

                (int tradeCount, TradeActionType tradeActionType) = newOrder.Sign(matchedOrder, signedPrice);

                var tradeAction = OrrnrrContext.TradeActions
                    .Where(x => x.Name == tradeActionType.GetActionName())
                    .First();

                var newTrade = new TransactionHistory(newOrder, matchedOrder, tradeAction, tradeCount, signedPrice);
            }
        }
    }
}
