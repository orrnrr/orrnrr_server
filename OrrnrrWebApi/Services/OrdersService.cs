using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Exceptions;
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

            var existsOrders = OrrnrrContext.TokenOrderHistories
                .Where(x => x.TokenId == tokenId)
                .Where(x => x.CompleteCount < x.OrderCount)
                .Where(x => x.OrderPrice >= price)
                .Where(x => x.IsBuyOrder != isBuyOrder)
                .Where(x => !x.IsCanceled)
                .OrderByDescending(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);

            var existsOrders2 = OrrnrrContext.

        }

        public TokenOrderHistory CreateMartetOrder(int userId, int tokenId, bool isBuyOrder, int count)
        {
            throw new NotImplementedException();
        }
    }
}
