using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Repositories;
using OrrnrrWebApi.Sort;
using OrrnrrWebApi.Types;
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
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);

            var token = OrrnrrContext.Tokens
                .Where(x => x.Id == tokenId)
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 토큰은 주문할 수 없습니다.");

            var user = OrrnrrContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault() ?? throw new BadRequestApiException("존재하지 않는 유저입니다.");

            if (isBuyOrder)
            {
                int payment = price * count;
                if (!user.CanPay(payment))
                {
                    throw new BadRequestApiException("잔고가 충분하지 않습니다.");
                }
            }
            else
            {
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

                if (numberOwned < numberSell)
                {
                    throw new BadRequestApiException("소유한 토큰 개수가 충분하지 않습니다.");
                }
            }

            var newOrder = TokenOrderHistory.Of(user, token, isBuyOrder, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories.GetExecutableOrders(token, price, isBuyOrder);
            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                var newTrade = TransactionHistory.Sign(newOrder, existsOrder);
                OrrnrrContext.TransactionHistories.Add(newTrade);
                OrrnrrContext.SaveChanges();
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
