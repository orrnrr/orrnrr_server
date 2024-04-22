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

            var existsOrders = OrrnrrContext.TokenOrderHistories.GetCanSellOrders(token, price);
            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                (int transactionCount, TradeActionType tradeActionType) = newOrder.Sign(existsOrder);

                var tradeAction = OrrnrrContext.TradeActions
                    .Where(x => x.Name == tradeActionType.GetActionName())
                    .First();

                var newTrade = new TransactionHistory(existsOrder, newOrder, tradeAction, transactionCount);
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
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 토큰은 주문할 수 없습니다.");

            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            var user = OrrnrrContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 유저입니다.");

            int existsBuyPayment = OrrnrrContext.TokenOrderHistories
                .Where(x => x.User == user)
                .Where(x => x.Token == token)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsBuyOrder)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Sum(x => x.OrderPrice * (x.OrderCount - x.CompleteCount));

            if (!user.CanPay(existsBuyPayment + price * count))
            {
                throw new BadRequestApiException("잔고가 충분하지 않습니다.");
            }

            var newOrder = TokenOrderHistory.CreateBuyOrder(user, token, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories.GetCanBuyOrders(token, price);

            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                Console.WriteLine(JsonSerializer.Serialize(existsOrder));

                (int transactionCount, TradeActionType tradeActionType) = newOrder.Sign(existsOrder);

                var tradeAction = OrrnrrContext.TradeActions
                    .Where(x => x.Name == tradeActionType.GetActionName())
                    .First();

                var newTrade = new TransactionHistory(newOrder, existsOrder, tradeAction, transactionCount);
                OrrnrrContext.TransactionHistories.Add(newTrade);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        public TokenOrderHistory CreateMarketOrder(int userId, int tokenId, bool isBuyOrder, int count)
        {
            throw new NotImplementedException();
        }
    }
}
